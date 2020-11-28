using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
//using System.Runtime.Remoting;
//using System.Runtime.Remoting.Lifetime;
//using System.Runtime.Remoting.Channels;
using System.Globalization;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Threading;
using System.Linq;
using System.Text;
using System.ServiceProcess;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace MIDRetail.StoreDelete
{
	// This is structured a little strangely because it was originally a console application.
	public class StoreDeleteMain
	{
		private string _appControlServer;
		private string _appStoreServer;
		private string _appHierarchyServer;
		private string _appApplicationServer;
		private string _appSchedulerServer;
		private int _copyConcurrentProcesses = 1;
		private int _deleteConcurrentProcesses = 1;
		private int _batchSize = 500;
		private bool _analysisOnly = true;
		private bool _skipAnalysis = false;
		private double _rowPercentageThreshold = 20.00;
		private int _minimumRowCount = 50000;
		private int _maximumRowCount = 500000;

		public StoreDeleteMain(string appControl, string appStore, string appHierarchy, string appApplication, string appScheduler, int copyConcurrent, int deleteConcurrent, int batchSize,
			bool analyisisOnly, bool skipAnalysis, double rowPct, int minCount, int maxCount)
		{
			_appControlServer = appControl;
			_appStoreServer = appStore;
			_appHierarchyServer = appHierarchy;
			_appApplicationServer = appApplication;
			_appSchedulerServer = appScheduler;
			_copyConcurrentProcesses = copyConcurrent;
			_deleteConcurrentProcesses = deleteConcurrent;
			_batchSize = batchSize;
			_analysisOnly = analyisisOnly;
			_skipAnalysis = skipAnalysis;
			_rowPercentageThreshold = rowPct;
			_minimumRowCount = minCount;
			_maximumRowCount = maxCount;
			
		}

		public StoreDeleteMain()
		{
			
		}

		public int Process(bool analysisOnly, bool skipAnalysis, StoreDeleteCommon.SendMessageDelegate msgDelegate)
		{
			log4net.ILog log = log4net.LogManager.GetLogger("Activity");
			DeleteStoreWorker deleteStore = new DeleteStoreWorker(log);
			int returnCode = deleteStore.Process(analysisOnly, skipAnalysis, msgDelegate);
			return returnCode;
		}

		public class DeleteStoreWorker
		{
			string sourceModule = "StoreDelete.cs";
			string eventLogID = "MIDStoreDelete";
			SessionAddressBlock _SAB;
			SessionSponsor _sponsor;
			IMessageCallback _messageCallback;
			string message = null;
			bool errorFound = false;
			System.Runtime.Remoting.Channels.IChannel channel;
			private string _appControlServer;
			private string _appStoreServer;
			private string _appHierarchyServer;
			private string _appApplicationServer;
			private string _appSchedulerServer;
			private bool _controlServerStopped = false;
			private bool _storeServerStopped = false;
			private bool _hierarchyServerStopped = false;
			private bool _applicationServerStopped = false;
			private bool _schedulerServerStopped = false;
			private log4net.ILog _log;
			private StoreData _storeData = null;
			private SystemData _systemData = null;

			private string _recoveryModel = string.Empty;
			private int _numStoreDeleted = 0;
			private DataTable _dtPreDelete = null;
			private DataTable _dtPostDelete = null;
			private int _copyConcurrentProcesses = 1;
			private int _deleteConcurrentProcesses = 1;
			private int _batchSize = 500;
			private bool _analysisOnly = true;
			private bool _skipAnalysis = false;
			private double _rowPercentageThreshold = 20.00;
			private int _minimumRowCount = 50000;
			private int _maximumRowCount = 500000;
			private string _connectionString;
			private int _maxTableNameLength = 100;
			private int _maxStoreIdLength = 20;
			private StringBuilder _storedProcedure;
			private List<string> _workTableList;

			public DeleteStoreWorker(log4net.ILog log)
			{
				_log = log;
			}

			public int Process(bool AnalysisOnly, bool skipAnalysis, StoreDeleteCommon.SendMessageDelegate msgDelegate)
			{
				eMIDMessageLevel highestMessage = eMIDMessageLevel.None;
				GlobalOptionsProfile gop = new GlobalOptionsProfile(-1);
				GlobalOptions go = new GlobalOptions();
				string msg = string.Empty;
				try
				{
					_messageCallback = new BatchMessageCallback();
					_sponsor = new SessionSponsor();
					_SAB = new SessionAddressBlock(_messageCallback, _sponsor);

					if (!EventLog.SourceExists(eventLogID))
					{
						EventLog.CreateEventSource(eventLogID, null);
					}

					WriteEventLog("Store Delete Process", EventLogEntryType.Information);

					// Register callback channel

					//try
					//{
					//    channel = _SAB.OpenCallbackChannel();
					//}
					//catch (Exception exception)
					//{
					//    errorFound = true;
					//    WriteEventLog("Error opening port #0 - " + exception.Message, EventLogEntryType.Error);
					//    return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
					//}

					LogInfo("===============================");
					//==========================
					// Configuration Settings
					//==========================
					GetConfigSettings(AnalysisOnly, skipAnalysis);
					
                    if (!AnalysisOnly)
                    { 
					    //=====================================
					    // Be sure Server Sessions are DOWN
					    //=====================================
					    if (AreServicesStopped())
					    {
						    _workTableList = new List<string>();
						    _connectionString = MIDConfigurationManager.AppSettings["ConnectionString"];

						    //=========================
						    // Get main DB connection
						    //=========================
						    _storeData = new StoreData(_connectionString);
						    if (!_storeData.ConnectionIsOpen)
						    {
							    _storeData.OpenUpdateConnection();
						    }
						    LogInfo("DataBase: " + _storeData.GetDatabaseName());

						    //============================================
						    // Check for Store Delete already in progress
						    //============================================
						    gop.LoadOptions();
						    if (gop.IsStoreDeleteInProgress)
						    {
							    LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
							    //LogError("The system indicates that a Store Delete is already running. Only one Store Delete process may be run at a time.");
							    WriteEventLog("The system indicates that a Store Delete is already running. Only one Store Delete process may be run at a time.", EventLogEntryType.Error);
							    LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
							    // Switch to AnalysisOnly mode so processwon't continue
							    _analysisOnly = true;
							    errorFound = true;
						    }
						    else
						    {
							    //============================================================
							    //Update Global Options Store Delete In progress to true;
							    //============================================================
							    // Moved to further down to when deleting actually begins.
							    //go.OpenUpdateConnection();
							    //go.UpdateStoreDeleteInProgress(gop.Key, true);
							    //go.CommitData();
							    //go.CloseUpdateConnection();
						    }

						    //=============================================
						    // Check for any stores ready for deletion
						    //=============================================
						    DataTable storeTable = _storeData.StoreProfile_ReadForStoreDelete();
						    if (storeTable.Rows.Count == 0 && !errorFound)
						    {
							    LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
							    msg = MIDText.GetTextOnly(eMIDTextCode.msg_NoStoresMarkedForDeletion);
							    WriteEventLog("msg", EventLogEntryType.Error);
							    LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
							    // Switch to AnalysisOnly mode so processwon't continue
							    _analysisOnly = true;
							    errorFound = true;
						    }
						    //===================
						    // DO ANALYSIS
						    //===================
						    if (!_skipAnalysis && !errorFound)
						    {
							    LogInfo("Running Analysis");
							    _storeData.StoreProfile_DoStoreRemovalAnalysis(_rowPercentageThreshold, _minimumRowCount, _maximumRowCount);
							    _storeData.CommitData();
						    }
						    //==========================
						    // Check Analysis dates
						    //==========================
						    if (_skipAnalysis && !errorFound)
						    {
							    errorFound = CheckAnalysisDates();
						    }
						    if (!errorFound)
						    {
							    LogAnalysis();
						    }

						    //=====================================================
						    // If not in analysis Only mode, continue processing
						    //=====================================================
						    if (!_analysisOnly && !errorFound)
						    {
							    try
							    {
								    //============================================================
								    //Update Global Options Store Delete In progress to true;
								    //============================================================
								    go.OpenUpdateConnection();
								    go.UpdateStoreDeleteInProgress(gop.Key, true);
								    go.CommitData();
								    go.CloseUpdateConnection();
								    //=============================
								    // Set recovery mode to SIMPLE
								    //=============================
								    SetRecoveryModelSimple(_connectionString);

								    //==================
								    // Drop Triggers
								    //==================
								    _storeData.DisableTrigger("UPDATE_INTRANSIT_REV", "STORE_INTRANSIT");
								    _storeData.DisableTrigger("UPDATE_IMO_REV", "STORE_IMO");
								    LogInfo("Triggers Disabled");

								    //=====================
								    // Disable Constraints
								    //=====================
								    //try
								    //{
								    //    _systemData = new SystemData();
								    //    _systemData.OpenUpdateConnection();
								    //    //_systemData.DatabaseConstraints_DisableAll();
								    //    _systemData.CommitData();
								    //    _systemData.CloseUpdateConnection();
								    //    LogInfo("DB Constraints Disabled");
								    //}
								    //catch (Exception ex)
								    //{
								    //    LogInfo("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
								    //    LogInfo("Could NOT DISABLE constraints for STORES. Exception: " + ex.ToString());
								    //    LogInfo("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
								    //    throw;
								    //}

								    //=======================
								    // Truncate REV tables
								    //=======================
								    _storeData.TruncateTable("STORE_INTRANSIT_REV");
								    _storeData.TruncateTable("STORE_IMO_REV");
								    LogInfo("Truncated _REV Tables");
								    _storeData.CommitData();

								    //======================================
								    // Log table row counts prior to delete
								    //======================================
								    _dtPreDelete = LogDBTableRowCounts("PRE-DELETE TABLE ROW COUNTS");

								    //=================
								    // PROCESSING
								    //=================
								    if (!errorFound)
								    {
									    errorFound = DeleteStoresConcurrent(msgDelegate);
								    }
							    }
							    catch
							    {
								    errorFound = true;
								    throw;
							    }
							    finally
							    {
								    //======================================
								    // Log table row counts after delete
								    //======================================
								    if (!errorFound)
								    {
									    LogInfo(_numStoreDeleted.ToString() + " stores deleted.");
								    }
								    _dtPostDelete = GetDBTableRowCounts();
								    LogDBRowCountDeleted(_dtPreDelete, _dtPostDelete);
							

								    LogInfo("===============================");
								    LogInfo("FINAL CLEANUP");
								    LogInfo("===============================");

								    //=======================
								    // Clean up work tables
								    // *Now cleaned up in each process*
								    //=======================
								    //CleanUpWorkTables();
								    //LogInfo("Work Tables Removed");
								    //==================
								    // Enable Triggers
								    //==================
								    _storeData.EnableTrigger("UPDATE_INTRANSIT_REV", "STORE_INTRANSIT");
								    _storeData.EnableTrigger("UPDATE_IMO_REV", "STORE_IMO");
								    LogInfo("Enabled triggers");

								    //=====================
								    // Enable Constraints
								    //=====================
								    //_systemData = new SystemData();
								    //_systemData.OpenUpdateConnection();
								    ////_systemData.DatabaseConstraints_EnableAll();
								    //_systemData.CommitData();
								    //_systemData.CloseUpdateConnection();
								    //LogInfo("DB Constraints Enabled");

								    //==========================
								    // Close main DB Connection
								    //==========================
								    if (_storeData.ConnectionIsOpen)
								    {
									    _storeData.CommitData();
									    _storeData.CloseUpdateConnection();
								    }

								    //=======================
								    // Restore Recovery Mode
								    //=======================
								    RestoreRecoveryModel(_connectionString);

							    }
						    }
					    }
					    else
					    {
						    errorFound = true;
						    msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteServicesMustBeDown);
						    WriteEventLog(msg, EventLogEntryType.Error);
					    }
                    }
                    else 
                    {
                        //=====================================
                        //Begin of AnalysisOnly
                        //=====================================

                        //=====================================
                        // Be sure Server Sessions are DOWN
                        //=====================================
                        //if (AreServicesStopped())
                        //{
                            _workTableList = new List<string>();
                            _connectionString = MIDConfigurationManager.AppSettings["ConnectionString"];

                            //=========================
                            // Get main DB connection
                            //=========================
                            _storeData = new StoreData(_connectionString);
                            if (!_storeData.ConnectionIsOpen)
                            {
                                _storeData.OpenUpdateConnection();
                            }
                            LogInfo("DataBase: " + _storeData.GetDatabaseName());

                            //============================================
                            // Check for Store Delete already in progress
                            //============================================
                            gop.LoadOptions();
                            if (gop.IsStoreDeleteInProgress)
                            {
                                LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                //LogError("The system indicates that a Store Delete is already running. Only one Store Delete process may be run at a time.");
                                WriteEventLog("The system indicates that a Store Delete is already running. Only one Store Delete process may be run at a time.", EventLogEntryType.Error);
                                LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                // Switch to AnalysisOnly mode so processwon't continue
                                _analysisOnly = true;
                                errorFound = true;
                            }
                            else
                            {
                                //============================================================
                                //Update Global Options Store Delete In progress to true;
                                //============================================================
                                // Moved to further down to when deleting actually begins.
                                //go.OpenUpdateConnection();
                                //go.UpdateStoreDeleteInProgress(gop.Key, true);
                                //go.CommitData();
                                //go.CloseUpdateConnection();
                            }

                            //=============================================
                            // Check for any stores ready for deletion
                            //=============================================
                            DataTable storeTable = _storeData.StoreProfile_ReadForStoreDelete();
                            if (storeTable.Rows.Count == 0 && !errorFound)
                            {
                                LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                msg = MIDText.GetTextOnly(eMIDTextCode.msg_NoStoresMarkedForDeletion);
                                WriteEventLog("msg", EventLogEntryType.Error);
                                LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                // Switch to AnalysisOnly mode so processwon't continue
                                _analysisOnly = true;
                                errorFound = true;
                            }
                            //===================
                            // DO ANALYSIS
                            //===================
                            if (!_skipAnalysis && !errorFound)
                            {
                                LogInfo("Running Analysis");
                                _storeData.StoreProfile_DoStoreRemovalAnalysis(_rowPercentageThreshold, _minimumRowCount, _maximumRowCount);
                                _storeData.CommitData();
                            }
                            //==========================
                            // Check Analysis dates
                            //==========================
                            if (_skipAnalysis && !errorFound)
                            {
                                errorFound = CheckAnalysisDates();
                            }
                            if (!errorFound)
                            {
                                LogAnalysis();
                            }

                            ////=====================================================
                            //// If not in analysis Only mode, continue processing
                            ////=====================================================
                            //if (!_analysisOnly && !errorFound)
                            //{
                            //    try
                            //    {
                            //        //============================================================
                            //        //Update Global Options Store Delete In progress to true;
                            //        //============================================================
                            //        go.OpenUpdateConnection();
                            //        go.UpdateStoreDeleteInProgress(gop.Key, true);
                            //        go.CommitData();
                            //        go.CloseUpdateConnection();
                            //        //=============================
                            //        // Set recovery mode to SIMPLE
                            //        //=============================
                            //        SetRecoveryModelSimple(_connectionString);

                            //        //==================
                            //        // Drop Triggers
                            //        //==================
                            //        _storeData.DisableTrigger("UPDATE_INTRANSIT_REV", "STORE_INTRANSIT");
                            //        _storeData.DisableTrigger("UPDATE_IMO_REV", "STORE_IMO");
                            //        LogInfo("Triggers Disabled");

                            //        //=====================
                            //        // Disable Constraints
                            //        //=====================
                            //        //try
                            //        //{
                            //        //    _systemData = new SystemData();
                            //        //    _systemData.OpenUpdateConnection();
                            //        //    //_systemData.DatabaseConstraints_DisableAll();
                            //        //    _systemData.CommitData();
                            //        //    _systemData.CloseUpdateConnection();
                            //        //    LogInfo("DB Constraints Disabled");
                            //        //}
                            //        //catch (Exception ex)
                            //        //{
                            //        //    LogInfo("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                            //        //    LogInfo("Could NOT DISABLE constraints for STORES. Exception: " + ex.ToString());
                            //        //    LogInfo("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                            //        //    throw;
                            //        //}

                            //        //=======================
                            //        // Truncate REV tables
                            //        //=======================
                            //        _storeData.TruncateTable("STORE_INTRANSIT_REV");
                            //        _storeData.TruncateTable("STORE_IMO_REV");
                            //        LogInfo("Truncated _REV Tables");
                            //        _storeData.CommitData();

                            //        //======================================
                            //        // Log table row counts prior to delete
                            //        //======================================
                            //        _dtPreDelete = LogDBTableRowCounts("PRE-DELETE TABLE ROW COUNTS");

                            //        //=================
                            //        // PROCESSING
                            //        //=================
                            //        if (!errorFound)
                            //        {
                            //            errorFound = DeleteStoresConcurrent(msgDelegate);
                            //        }
                            //    }
                            //    catch
                            //    {
                            //        errorFound = true;
                            //        throw;
                            //    }
                            //    finally
                            //    {
                            //        //======================================
                            //        // Log table row counts after delete
                            //        //======================================
                            //        if (!errorFound)
                            //        {
                            //            LogInfo(_numStoreDeleted.ToString() + " stores deleted.");
                            //        }
                            //        _dtPostDelete = GetDBTableRowCounts();
                            //        LogDBRowCountDeleted(_dtPreDelete, _dtPostDelete);


                            //        LogInfo("===============================");
                            //        LogInfo("FINAL CLEANUP");
                            //        LogInfo("===============================");

                            //        //=======================
                            //        // Clean up work tables
                            //        // *Now cleaned up in each process*
                            //        //=======================
                            //        //CleanUpWorkTables();
                            //        //LogInfo("Work Tables Removed");
                            //        //==================
                            //        // Enable Triggers
                            //        //==================
                            //        _storeData.EnableTrigger("UPDATE_INTRANSIT_REV", "STORE_INTRANSIT");
                            //        _storeData.EnableTrigger("UPDATE_IMO_REV", "STORE_IMO");
                            //        LogInfo("Enabled triggers");

                            //        //=====================
                            //        // Enable Constraints
                            //        //=====================
                            //        //_systemData = new SystemData();
                            //        //_systemData.OpenUpdateConnection();
                            //        ////_systemData.DatabaseConstraints_EnableAll();
                            //        //_systemData.CommitData();
                            //        //_systemData.CloseUpdateConnection();
                            //        //LogInfo("DB Constraints Enabled");

                            //        //==========================
                            //        // Close main DB Connection
                            //        //==========================
                            //        if (_storeData.ConnectionIsOpen)
                            //        {
                            //            _storeData.CommitData();
                            //            _storeData.CloseUpdateConnection();
                            //        }

                            //        //=======================
                            //        // Restore Recovery Mode
                            //        //=======================
                            //        RestoreRecoveryModel(_connectionString);

                            //    }
                            //}
                        //}
                        //else
                        //{
                        //    errorFound = true;
                        //    msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteServicesMustBeDown);
                        //    WriteEventLog(msg, EventLogEntryType.Error);
                        //}

                    }
				}

				catch (Exception exception)
				{
					errorFound = true;
					message = "";
					while (exception != null)
					{
						message += " -- " + exception.ToString(); ;
						exception = exception.InnerException;
					}

					WriteEventLog(message, EventLogEntryType.Error);

				}
				finally
				{
					if (errorFound)
					{
						highestMessage = eMIDMessageLevel.Error;
					}
					else
					{
						highestMessage = eMIDMessageLevel.None;
						//============================================================
						//Update Global Options Store Delete In progress to false;
						//============================================================
						go.OpenUpdateConnection();
						go.UpdateStoreDeleteInProgress(gop.Key, false);
						go.CommitData();
						go.CloseUpdateConnection();
					}
				}

				if (_analysisOnly)
				{
					WriteEventLog("Store Delete Analysis has completed", EventLogEntryType.Information);
				}
				else
				{
				WriteEventLog("Store Delete has completed", EventLogEntryType.Information);
				}
				LogBufferFlush();
				return Convert.ToInt32(highestMessage);

			}


			private void GetConfigSettings(bool analysisOnly, bool skipAnalysis)
			{
				try
				{
					string strParm = MIDConfigurationManager.AppSettings["BatchSize"];
					if (strParm != null)
					{
						try
						{
							_batchSize = Convert.ToInt32(strParm);
						}
						catch
						{
						}
					}
					LogInfo("Batch Size: " + _batchSize);

					strParm = MIDConfigurationManager.AppSettings["CopyConcurrentProcesses"];
					if (strParm != null)
					{
						try
						{
							_copyConcurrentProcesses = Convert.ToInt32(strParm);
						}
						catch
						{
						}
					}
					LogInfo("Copy Concurrent Processes: " + _copyConcurrentProcesses);

					strParm = MIDConfigurationManager.AppSettings["DeleteConcurrentProcesses"];
					if (strParm != null)
					{
						try
						{
							_deleteConcurrentProcesses = Convert.ToInt32(strParm);
						}
						catch
						{
						}
					}
					LogInfo("Delete Concurrent Processes: " + _deleteConcurrentProcesses);

					//strParm = MIDConfigurationManager.AppSettings["SkipAnalysis"];
					//if (strParm != null)
					//{
					//    try
					//    {
					//        _skipAnalysis = Include.ConvertStringToBool(strParm);
					//    }
					//    catch
					//    {
					//    }
					//}
					_skipAnalysis = skipAnalysis;
					LogInfo("Skip Analysis: " + _skipAnalysis.ToString());

					//strParm = MIDConfigurationManager.AppSettings["AnalysisOnly"];
					//if (strParm != null)
					//{
					//    try
					//    {
					//        _analysisOnly = Include.ConvertStringToBool(strParm);
					//    }
					//    catch
					//    {
					//    }
					//}
					_analysisOnly = analysisOnly;
					LogInfo("  Analysis Only: " + _analysisOnly.ToString());

					strParm = MIDConfigurationManager.AppSettings["RowPercentageThreshold"];
					if (strParm != null)
					{
						try
						{
							_rowPercentageThreshold = Convert.ToDouble(strParm);
							//==================================================================================================
							// The percentage is entered as a whole number, but the stored proc likes it as a true percentage.
							// Example: the config says "25", we change that to .25 for the stored procedure.
							//==================================================================================================
							_rowPercentageThreshold = _rowPercentageThreshold / 100;
						}
						catch
						{
						}
					}
					LogInfo("Row Percentage Threshold: " + strParm + "%");

					strParm = MIDConfigurationManager.AppSettings["MinimumRowCount"];
					if (strParm != null)
					{
						try
						{
							_minimumRowCount = Convert.ToInt32(strParm);
						}
						catch
						{
						}
					}
					LogInfo("Minimum Row Count: " + _minimumRowCount.ToString());

					strParm = MIDConfigurationManager.AppSettings["MaximumRowCount"];
					if (strParm != null)
					{
						try
						{
							_maximumRowCount = Convert.ToInt32(strParm);
						}
						catch
						{
						}
					}
					LogInfo("Maximumm Row Count: " + _maximumRowCount.ToString());
					LogInfo(" ");
				}
				catch
				{
					throw;
				}
			}

            //BEGIN TT#4636-VStuart-Incorrectly checking if services are down-MID
            private bool AreServicesStopped()
			{
				try
				{
                    bool stopped = false;
                    _appControlServer = MIDConfigurationManager.AppSettings["ControlServiceNames"];
					_appStoreServer = MIDConfigurationManager.AppSettings["StoreServiceNames"];
					_appHierarchyServer = MIDConfigurationManager.AppSettings["MerchandiseServiceNames"];
                    //_appApplicationServer = MIDConfigurationManager.AppSettings["ApplicationServiceNames"];
					_appSchedulerServer = MIDConfigurationManager.AppSettings["SchedulerServiceNames"];

					_controlServerStopped = IsServiceStopped(_appControlServer);
					_storeServerStopped = IsServiceStopped(_appControlServer);
					_hierarchyServerStopped = IsServiceStopped(_appControlServer);
					_schedulerServerStopped = IsServiceStopped(_appControlServer);
                    //_applicationServerStopped = IsServiceStopped(_appControlServer);

                    //return (_controlServerStopped && _storeServerStopped && _hierarchyServerStopped && _schedulerServerStopped && _applicationServerStopped);
                    if (_controlServerStopped && _storeServerStopped && _hierarchyServerStopped && _schedulerServerStopped)
                    {
                        stopped = true;
                    }
                    return stopped;
                }
				catch
				{
					throw;
				}
			}
            //END TT#4636-VStuart-Incorrectly checking if services are down-MID


			private bool IsServiceStopped(string serviceName)
			{
				bool isStopped = false;
				System.ServiceProcess.ServiceController sc = new System.ServiceProcess.ServiceController(serviceName);

				try
				{
					if (sc.Status == ServiceControllerStatus.Stopped)
					{
						isStopped = true;
					}
				}
				catch (System.InvalidOperationException ex)
				{
					// Exception thrown when service doesn't exist...which is OK.
					isStopped = true;
				}
				catch
				{
					throw;
				}

				return isStopped;
			}

			private void CleanUpWorkTables()
			{
				if (!_storeData.ConnectionIsOpen)
				{
					_storeData.OpenUpdateConnection();
				}
				foreach (string workTableName in _workTableList)
				{
					_storeData.TruncateTable(workTableName);
					_storeData.DropTable(workTableName);
				}
				_storeData.CommitData();
			}

				
				

			private void LogAnalysis()
			{
				List<string> deleteList = new List<string>();
				List<string> copyList = new List<string>();
				List<string> bypassList = new List<string>();
				

				string results = string.Empty;
				LogInfo("===============================");
				LogInfo(" ANALYSIS RESULTS");
				LogInfo("===============================");

				//=====================
				// get ANALYSIS table
				//=====================
				DataTable analysisTable = _storeData.StoreProfile_ReadRemovalAnalysis();
				_maxTableNameLength = analysisTable.AsEnumerable().Select(row => row["TABLE_NAME"]).OfType<string>().Max(val => val.Length);
				_maxTableNameLength = _maxTableNameLength + 9;

				foreach (DataRow row in analysisTable.Rows)
				{
					string tableString = string.Empty;
					string tableName = row["TABLE_NAME"].ToString();
					int rowsToDelCount = int.Parse(row["ROWS_TO_DELETE_COUNT"].ToString());
					if (Include.ConvertStringToBool(row["STORE_SET_IND"].ToString()))
					{
						tableName = tableName + " (sets)";
					}

					if (rowsToDelCount == 0)
					{
						results = "Bypassed  ";
						bypassList.Add(tableName + "," + rowsToDelCount);
					} 
					else 
					{
						results = "Copy/Load ";
						if (row["DO_DELETE_PROCESS"] != DBNull.Value  && Include.ConvertStringToBool(row["DO_DELETE_PROCESS"].ToString()))
						{
							results = "Delete    ";
						}
						if (results == "Copy/Load ")
						{
							copyList.Add(tableName + "," + rowsToDelCount);
						}
						else
						{
							deleteList.Add(tableName + "," + rowsToDelCount);
						}
					}
				}

				foreach (string line in copyList)
				{
					string [] items = MIDstringTools.Split(line, ',', true);
					string tableName = items[0];
					string rows = items[1];

					String rec = new String(' ', 150);
					rec = rec.Insert(0, tableName);
					rec = rec.Insert(_maxTableNameLength, "Results: Copy/Load Number of Rows: " + rows);
					LogInfo(rec);
					
				}
				LogInfo("");
				foreach (string line in deleteList)
				{
					string[] items = MIDstringTools.Split(line, ',', true);
					string tableName = items[0];
					string rows = items[1];

					String rec = new String(' ', 150);
					rec = rec.Insert(0, tableName);
					rec = rec.Insert(_maxTableNameLength, "Results: Delete    Number of Rows: " + rows);
					LogInfo(rec);
				}
				LogInfo("");
				foreach (string line in bypassList)
				{
					string[] items = MIDstringTools.Split(line, ',', true);
					string tableName = items[0];
					string rows = items[1];

					String rec = new String(' ', 150);
					rec = rec.Insert(0, tableName);
					rec = rec.Insert(_maxTableNameLength, "Results: Bypass    Number of Rows: " + rows);
					LogInfo(rec);
				}
			}

			private bool CheckAnalysisDates()
			{
				bool errorFound = false;
				//=====================
				// get ANALYSIS table
				//=====================
				DataTable analysisTable = _storeData.StoreProfile_ReadRemovalAnalysis();
				DateTime sevenDaysAgo = DateTime.Now.AddDays(-7);
				foreach (DataRow row in analysisTable.Rows)
				{
					if (row["ANALYSIS_DATE"] != DBNull.Value)
					{
						DateTime AnalysisDate = Convert.ToDateTime(row["ANALYSIS_DATE"]);
						if (AnalysisDate < sevenDaysAgo)
						{
							errorFound = true;
							LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
							//LogError("The system indicates that a Store Delete is already running. Only one Store Delete process may be run at a time.");
							string msg = MIDText.GetTextOnly(eMIDTextCode.msg_StoreDeleteAnalysisOutdated);
							WriteEventLog(msg, EventLogEntryType.Error);
							LogError("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
							errorFound = true;
							break;
						}
					}
				}
				return errorFound; 
			}

			private bool DeleteStoresConcurrent(StoreDeleteCommon.SendMessageDelegate msgDelegate)
			{
				bool errorFound = false;
				DataTable analysisTable = null;
				DataTable storeTable = null;
				try
				{
					
					//=====================
					// get ANALYSIS table
					//=====================
					analysisTable = _storeData.StoreProfile_ReadRemovalAnalysis();

					//=========================
					// Build Stored Procedures
					//=========================
					_storedProcedure = new StringBuilder();
					List<string> removalList = new List<string>();
					List<string> copyList = new List<string>();
					List<string> removalSetList = new List<string>();
					List<string> copySetList = new List<string>();
					BuildStoredProcedures(analysisTable, removalList, copyList, removalSetList, copySetList);

					//============================
					// Log Stores to be deleted
					//============================
					storeTable = _storeData.StoreProfile_ReadForStoreDelete();
					LogStoresToBeDeleted(storeTable);

					double totTables = analysisTable.Rows.Count;
					MIDTimer totalTimer = new MIDTimer();
					totalTimer.Start();

					DataTable dtPreStore = GetDBTableRowCounts();

					//if (storeCount % 2 == 0)
					//{
					//    _statusBar.RenderConsoleProgress(Convert.ToInt32((storeCount / totStores) * 100), '\u2592', ConsoleColor.Green, Convert.ToInt32((storeCount / totStores) * 100).ToString() + "% Complete");
					//}
					LogInfo(" ");

					//=================================
					// Special Tables to delete first
					//=================================
					DeleteStoreFromTable("NODE_SIZE_CURVE_SIMILAR_STORE", _batchSize);
					DeleteStoreFromTable("SIMILAR_STORES", _batchSize);
					//DeleteStoreFromTable("STORE_GROUP_LEVEL_STATEMENT", _batchSize);  // moved to end of delete process
                    DeleteStoreFromTable("FILTER_CONDITION_LIST_VALUES", _batchSize);  //DeleteStoreFromTable("IN_USE_FILTER_XREF", _batchSize); //TT#1342-MD -jsobek -Store Filters - IN_USE_FILTER_XREF
					DeleteStoreFromTable("SYSTEM_OPTIONS", _batchSize);

					//=================================
					// Start Multi Processing
					//=================================
					ArrayList storeRemovalArray = new ArrayList();
					ArrayList storeCopyArray = new ArrayList();
					Stack storeRemovalStack = BuildStack(storeRemovalArray, removalList, msgDelegate);
					Stack storeCopyStack = BuildStack(storeCopyArray, copyList, msgDelegate);
					string ml = MIDText.GetTextOnly((int)eMIDMessageLevel.None);
					int totalRowsDeleted = 0;
					//======================
					// Do Store COPYs
					//======================
					totalRowsDeleted += ProcessConcurrently(storeCopyStack, _copyConcurrentProcesses, storeCopyArray);
					//======================
					// Do Store REMOVALs
					//======================
					totalRowsDeleted += ProcessConcurrently(storeRemovalStack, _deleteConcurrentProcesses, storeRemovalArray);

                    //LogInfo("Removing Store Sets");
                    ////====================================================================================== 
                    //// Delete Store Sets that dynamically point to specific stores which are being deleted
                    ////======================================================================================
                    //ArrayList setRemovalArray = new ArrayList();
                    //ArrayList setCopyArray = new ArrayList();
                    //Stack setRemovalStack = BuildStack(storeRemovalArray, removalSetList, msgDelegate);
                    //Stack setCopyStack = BuildStack(storeCopyArray, copySetList, msgDelegate);
                    ////======================
                    //// Do Set COPYs
                    ////======================
                    //totalRowsDeleted += ProcessConcurrently(setCopyStack, _copyConcurrentProcesses, setCopyArray);
                    ////======================
                    //// Do Set REMOVALs
                    ////======================
                    //totalRowsDeleted += ProcessConcurrently(setRemovalStack, _deleteConcurrentProcesses, setRemovalArray);
                    //LogInfo("Completed Store Sets");

					//====================================
					// Finally Delete from STORES tables
					//====================================
					if (!errorFound)
					{
						//=================================
						// Special Tables to delete LAST
						//=================================
						//DeleteStoreFromTable("STORE_GROUP_LEVEL_STATEMENT", _batchSize);
						DeleteStoreFromTable("STORES", _batchSize);

						//========================================================================
						// Clean up Store eligibility table due to deleting any similar stores
						//========================================================================
						LogInfo("Final Clean up Similar Store Eligibilty");
						_storeData.StoreProfile_CleanupSimilarStoreEligibility();

						_storeData.CommitData();
					}
					else
					{
						_storeData.Rollback();
					}

					totalTimer.Stop();
					LogStoreMessage("COMPLETED", "Elapsed Time", 0, totalTimer.ElaspedTime);

					//TimeSpan storeAvg = totalTimer.AverageTime(storeTable.Rows.Count);
					//LogStoreMessage("STORE_AVERAGE", " xxxxxxxxxxxxx", totalRowsDeleted, storeAvg);

					//Flushes log4net buffer to log
					LogBufferFlush();

					return errorFound;
				}
				catch
				{
					throw;
				}
				finally
				{
					
				}
			}

			private int ProcessConcurrently(Stack storeStack, int concurrentProcesses, ArrayList storeArray)
			{
				int totalRowsDeleted = 0;

				try
				{
					if (concurrentProcesses > 1)
					{
						ConcurrentProcessManager cpm = new ConcurrentProcessManager(null, storeStack, concurrentProcesses, 500);
						cpm.ProcessCommands();
					}
					else
					{
						while (storeStack.Count > 0)
						{
							ConcurrentProcess cp = (ConcurrentProcess)storeStack.Pop();
							cp.ExecuteProcess();
						}
					}

					//========================
					// Post Copy processing
					//========================
					foreach (StoreDeleteProcess sdp in storeArray)
					{
						if (sdp.RowsDeleted > 0)
						{
							totalRowsDeleted = sdp.RowsDeleted + totalRowsDeleted;
						}
						if (sdp.MessageLevel == eMIDMessageLevel.Error || sdp.MessageLevel == eMIDMessageLevel.Severe)
						{
							errorFound = true;
						}
					}
				}
				catch
				{
					throw;
				}

				return totalRowsDeleted;
			}

			
			private void LogInfo(string msg)
			{
				try
				{
					if (_log.IsInfoEnabled)
					{
						_log.Info(msg);
					}
				}
				catch
				{
					throw;
				}
			}

			/// <summary>
			/// Builds all of the Stored Procedures for all of the tables being processed by Store Delete.
			/// </summary>
			/// <param name="analysisTable"></param>
			/// <param name="removalList"></param>
			/// <param name="copyList"></param>
			private void BuildStoredProcedures(DataTable analysisTable, List<string> removalList, List<string> copyList, List<string> removalSetList, List<string> copySetList)
			{
				List<string> spList = new List<string>();
				string spName = string.Empty;

				try
				{
					foreach (DataRow row in analysisTable.Rows)
					{
						string tableName = row["TABLE_NAME"].ToString();
						int rowsToDelCount = int.Parse(row["ROWS_TO_DELETE_COUNT"].ToString());
						eStoreDeleteTableStatus tableStatus = eStoreDeleteTableStatus.NotStarted;
						if (row["COMPLETED"] != DBNull.Value)
						{
							if (Convert.ToBoolean(row["COMPLETED"]))
							{
								tableStatus = eStoreDeleteTableStatus.Completed;
							}
						}
						//==============================================================
						// Only build stored procedures for tables with rows to delete
						//==============================================================
						if (rowsToDelCount > 0 && tableStatus != eStoreDeleteTableStatus.Completed)
						{
							bool doDelete = false;
							if (row["DO_DELETE_PROCESS"] != DBNull.Value)
							{
								doDelete = Include.ConvertStringToBool(row["DO_DELETE_PROCESS"].ToString());
							}
							//===============================================================================================
							// We ALWAYS want to COPY the SIZE_CONSTRAINT_MINMAX table because we remove null records also
							//===============================================================================================
							if (tableName == "SIZE_CONSTRAINT_MINMAX")
							{
								if (doDelete)
								{
									LogInfo("SIZE_CONSTRAINT_MINMAX Set processing has been changed from 'Delete' to 'Copy/load'");
								}
								doDelete = false;
							}

							if (doDelete)
							{
								bool isStoreSet = false;
								isStoreSet = Include.ConvertStringToBool(row["STORE_SET_IND"].ToString());
								if (isStoreSet)
								{
									// This inits and creates _storedProcedure
									spName = BuildRemoveSetStoredProcedure(tableName, "SGL_RID");
									_storeData.StoreProfile_ExecCommand(_storedProcedure.ToString());
									removalSetList.Add(spName);
								}
								else
								{
									// This inits and creates _storedProcedure
									spName = BuildRemoveStoredProcedure(tableName, "ST_RID");
									_storeData.StoreProfile_ExecCommand(_storedProcedure.ToString());
									removalList.Add(spName);
								}
							}
							else
							{
								bool isStoreSet = false;
								isStoreSet = Include.ConvertStringToBool(row["STORE_SET_IND"].ToString());
								if (isStoreSet)
								{
									// This inits and creates _storedProcedure
									spName = BuildCopySetStoredProcedure(tableName, "SGL_RID");
									_storeData.StoreProfile_ExecCommand(_storedProcedure.ToString());
									copySetList.Add(spName);
									_workTableList.Add(tableName + "_COPY");
								}
								else
								{
									// This inits and creates _storedProcedure
									spName = BuildCopyStoredProcedure(tableName, "ST_RID");
									_storeData.StoreProfile_ExecCommand(_storedProcedure.ToString());
									copyList.Add(spName);
									_workTableList.Add(tableName + "_COPY");
								}
							}
						}
						else
						{
							//=========================================================================
							// Updates completed switch to completed for tables with no rows to delete
							//=========================================================================
							_storeData.UpdateStoreDeleteInProgress(eStoreDeleteTableStatus.Completed, tableName);
						}
					}
					_storeData.CommitData();
				}
				catch
				{
					throw;
				}
				
			}

			private string BuildRemoveStoredProcedure(string tableName, string column)
			{

				try
				{
					string spName = "SP_MID_STORE_REMOVAL_" + tableName;

					//=======================================
					// Checks and Drops Stored Procedure
					//=======================================
					BuildStoredProcedureDrop(spName);
					_storeData.StoreProfile_ExecCommand(_storedProcedure.ToString());

					_storedProcedure.Length = 0;
					_storedProcedure.Capacity = 0;
					//============================================
					// WriteLine() fills in _storedProcedure
					//============================================
					WriteLine("CREATE PROCEDURE " + spName);
					WriteLine("(");
					WriteLine("@RC INT OUTPUT");
					WriteLine(")");
					WriteLine("AS");
					WriteLine("BEGIN");
					WriteLine("declare @rowCount int");
					WriteLine("set @RC = 0");
					WriteLine("set @rowCount = 0");
					WriteLine("while (1 = 1)");
					WriteLine("BEGIN");
					WriteLine("BEGIN TRANSACTION");
					WriteLine("DELETE TOP (" + _batchSize.ToString() + ") from dbo." + tableName + " where ST_RID in (SELECT ST_RID from STORES where STORE_DELETE_IND = '1')");
					WriteLine("set @rowCount = @@ROWCOUNT");
					WriteLine("COMMIT");
					WriteLine("if @rowCount = 0");
					WriteLine("break");
					WriteLine("else");
					WriteLine("BEGIN");
					WriteLine("set @RC = @RC + @rowCount");
					WriteLine("continue");
					WriteLine("END");
					WriteLine("END");
					WriteLine("return @RC");
					WriteLine("END");

					return spName;
				}
				catch 
				{
					throw;
				}
			}

			private string BuildCopyStoredProcedure(string tableName, string column)
			{
				try
				{
					string spName = "SP_MID_STORE_COPYLOAD_" + tableName;
					string newTableName = tableName + "_COPY";

					//=======================================
					// Checks and Drops Stored Procedure
					//=======================================
					BuildStoredProcedureDrop(spName);
					_storeData.StoreProfile_ExecCommand(_storedProcedure.ToString());

					//=======================================
					// Checks and Drops temp (_COPY) table
					//=======================================
					BuildTempTableDrop(newTableName);
					_storeData.StoreProfile_ExecCommand(_storedProcedure.ToString());

					_storedProcedure.Length = 0;
					_storedProcedure.Capacity = 0;
					//============================================
					// WriteLine() fills in _storedProcedure
					//============================================
					WriteLine("CREATE PROCEDURE " + spName);
					WriteLine("(");
					WriteLine("@RC INT OUTPUT");
					WriteLine(")");
					WriteLine("AS");
					WriteLine("BEGIN");

					WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + newTableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)");
					WriteLine("BEGIN");
					WriteLine("TRUNCATE TABLE dbo." + newTableName);
					WriteLine("DROP TABLE dbo." + newTableName);
					WriteLine("END");

					WriteLine("set @RC = 0");
					WriteLine("BEGIN TRANSACTION");
					WriteLine("select * into dbo." + newTableName + " from dbo." + tableName + " where ST_RID in (SELECT ST_RID from STORES where STORE_DELETE_IND <> '1')");
					WriteLine("set @RC = @@ROWCOUNT");
					WriteLine("COMMIT");

					WriteLine("BEGIN TRANSACTION");
					WriteLine("TRUNCATE TABLE " + tableName);
					WriteLine("INSERT INTO dbo." + tableName + " SELECT * FROM dbo." + newTableName);

					WriteLine("set @RC = @@ROWCOUNT");
					WriteLine("COMMIT");

					WriteLine("return @RC");
					WriteLine("END");

					return spName;
				}
				catch 
				{
					throw;
				}
			}

			private string BuildRemoveSetStoredProcedure(string tableName, string column)
			{

				try
				{
					string spName = "SP_MID_STORE_SET_REMOVAL_" + tableName;

					//=======================================
					// Checks and Drops Stored Procedure
					//=======================================
					BuildStoredProcedureDrop(spName);
					_storeData.StoreProfile_ExecCommand(_storedProcedure.ToString());

					_storedProcedure.Length = 0;
					_storedProcedure.Capacity = 0;
					//============================================
					// WriteLine() fills in _storedProcedure
					//============================================
					WriteLine("CREATE PROCEDURE " + spName);
					WriteLine("(");
					WriteLine("@RC INT OUTPUT");
					WriteLine(")");
					WriteLine("AS");
					WriteLine("BEGIN");
					WriteLine("declare @rowCount int");
					WriteLine("declare @sgRid int");
					WriteLine("set @RC = 0");
					WriteLine("set @rowCount = 0");
					WriteLine("set @sgRid = 0");
					WriteLine("SELECT @sgRid = SG_RID FROM STORE_DYNAMIC_GROUP_DESC where SDGD_CHAR_ID = 900000");
					WriteLine("if (@sgRid > 0)");
					WriteLine("BEGIN");
					WriteLine("while (1 = 1)");
					WriteLine("BEGIN");
					WriteLine("BEGIN TRANSACTION");

                    //WriteLine("DELETE TOP (" + _batchSize.ToString() + ") from dbo." + tableName + " select *  from dbo.SIZE_CONSTRAINT_GRPLVL scsgl");
                    //WriteLine("where scsgl.SGL_RID in (SELECT SGL_RID from STORE_GROUP_LEVEL sgl");
                    WriteLine("DELETE TOP (" + _batchSize.ToString() + ") from dbo." + tableName);
					WriteLine("where SGL_RID in (SELECT SGL_RID from STORE_GROUP_LEVEL sgl");
					WriteLine("inner join STORES s on sgl.SGL_ID = s.ST_ID");
					WriteLine("inner join STORE_DYNAMIC_GROUP_DESC sdgd on sdgd.SG_RID = sgl.SG_RID");
					WriteLine("where sdgd.SDGD_CHAR_ID = 900000 and STORE_DELETE_IND = 1)");

					WriteLine("set @rowCount = @@ROWCOUNT");
					WriteLine("COMMIT");
					WriteLine("if @rowCount = 0");
					WriteLine("break");
					WriteLine("else");
					WriteLine("BEGIN");
					WriteLine("set @RC = @RC + @rowCount");
					WriteLine("continue");
					WriteLine("END");
					WriteLine("END");
					WriteLine("END");
					WriteLine("return @RC");
					WriteLine("END");

					return spName;
				}
				catch 
				{
					throw;
				}
			}

			private string BuildCopySetStoredProcedure(string tableName, string column)
			{
				try
				{
					string spName = "SP_MID_STORE_SET_COPYLOAD_" + tableName;
					string newTableName = tableName + "_COPY";

					//=======================================
					// Checks and Drops Stored Procedure
					//=======================================
					BuildStoredProcedureDrop(spName);
					_storeData.StoreProfile_ExecCommand(_storedProcedure.ToString());

					//=======================================
					// Checks and Drops temp (_COPY) table
					//=======================================
					BuildTempTableDrop(newTableName);
					_storeData.StoreProfile_ExecCommand(_storedProcedure.ToString());

					_storedProcedure.Length = 0;
					_storedProcedure.Capacity = 0;
					//============================================
					// WriteLine() fills in _storedProcedure
					//============================================
					WriteLine("CREATE PROCEDURE " + spName);
					WriteLine("(");
					WriteLine("@RC INT OUTPUT");
					WriteLine(")");
					WriteLine("AS");
					WriteLine("BEGIN");

					WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + newTableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)");
					WriteLine("BEGIN");
					WriteLine("TRUNCATE TABLE dbo." + newTableName);
					WriteLine("DROP TABLE dbo." + newTableName);
					WriteLine("END");

					WriteLine("declare @sgRid int");
					WriteLine("set @sgRid = 0");
					WriteLine("SELECT @sgRid = SG_RID FROM STORE_DYNAMIC_GROUP_DESC where SDGD_CHAR_ID = 900000");
					WriteLine("if (@sgRid > 0)");
					WriteLine("BEGIN");

					WriteLine("set @RC = 0");
					WriteLine("BEGIN TRANSACTION");

					WriteLine("select * into dbo." + newTableName + " from dbo." + tableName + " sglx where sglx.SGL_RID not in (SELECT SGL_RID from STORE_GROUP_LEVEL sgl ");
					WriteLine("inner join STORES s on sgl.SGL_ID = s.ST_ID");
					WriteLine("inner join STORE_DYNAMIC_GROUP_DESC sdgd on sdgd.SG_RID = sgl.SG_RID");
					WriteLine("where sdgd.SDGD_CHAR_ID = 900000 and STORE_DELETE_IND = 1)");

					if (tableName == "SIZE_CONSTRAINT_MINMAX")
					{
                        //WriteLine("and (SIZE_MIN is not NULL or SIZE_MAX is not NULL or SIZE_MULT is not NULL)");
                        WriteLine("or (SIZE_MIN is NULL and SIZE_MAX is NULL and SIZE_MULT is NULL)");
					}
					
					WriteLine("set @RC = @@ROWCOUNT");
					WriteLine("COMMIT");

					WriteLine("BEGIN TRANSACTION");
					WriteLine("TRUNCATE TABLE " + tableName);
					WriteLine("INSERT INTO dbo." + tableName + " SELECT * FROM dbo." + newTableName);

					WriteLine("set @RC = @@ROWCOUNT");
					WriteLine("COMMIT");
					WriteLine("END");

					WriteLine("return @RC");
					WriteLine("END");

					return spName;
				}
				catch 
				{
					throw;
				}
			}

			private void BuildStoredProcedureDrop(string spName)
			{

				try
				{
					_storedProcedure.Length = 0;
					_storedProcedure.Capacity = 0;
					//============================================
					// WriteLine() fills in _storedProcedure
					//============================================
					WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'" + spName + "') and OBJECTPROPERTY(id, N'IsProcedure') = 1)");
					WriteLine("BEGIN");
					WriteLine("drop procedure " + spName);
					WriteLine("END");
					
				}
				catch 
				{
					throw;
				}
			}

			private void BuildTempTableDrop(string newTableName)
			{
				try
				{
					_storedProcedure.Length = 0;
					_storedProcedure.Capacity = 0;
					//============================================
					// WriteLine() fills in _storedProcedure
					//============================================
					WriteLine("if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[" + newTableName + "]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)");
					WriteLine("BEGIN");
					WriteLine("TRUNCATE TABLE dbo." + newTableName);
					WriteLine("DROP TABLE dbo." + newTableName);
					WriteLine("END");
				}
				catch 
				{
					throw;
				}
			}

			public void WriteLine(string aText)
			{
				try
				{
					_storedProcedure.Append(aText);
					_storedProcedure.Append(System.Environment.NewLine);
				}
				catch 
				{
					throw;
				}
			}
    
			private int DeleteStoreFromTable(string tableName, int batchSize)
			{
				try
				{
					MIDTimer aTimer = new MIDTimer();
					aTimer.Start();
					if (!_storeData.ConnectionIsOpen)
					{
						_storeData.OpenUpdateConnection();
					}
					int rowsDeleted = _storeData.StoreProfile_Delete(tableName, batchSize);
					aTimer.Stop();

					_storeData.CommitData();

					if (rowsDeleted > 0)
					{
						String rec = new String(' ', 150);
						rec = rec.Insert(0, tableName);
						rec = rec.Insert(_maxTableNameLength, " Rows_Deleted: " + rowsDeleted);
						rec = rec.Insert(_maxTableNameLength + 25, aTimer.ElaspedTimeString);
						LogInfo(rec);
					}

					return rowsDeleted;
				}
				catch
				{
					throw;
				}
			}

			private void LogStoreMessage(string msg1, string msg2, int count, TimeSpan time)
			{
				try
				{
						String rec = new String(' ', 150);
						rec = rec.Insert(0, msg1);
						rec = rec.Insert(_maxTableNameLength, msg2 + " " + count.ToString());
						rec = rec.Insert(_maxTableNameLength + 25, time.ToString());
						LogInfo(rec);
				}
				catch
				{
					throw;
				}
			}


			private Stack BuildStack(ArrayList stackList, List<string> procList, StoreDeleteCommon.SendMessageDelegate msgDelegate)
			{
				Stack aStack = new Stack();

				foreach (string proc in procList)
				{
					StoreDeleteProcess aProcess = new StoreDeleteProcess(_connectionString, _SAB, proc, _storeData, _log, _batchSize, _maxTableNameLength, _maxStoreIdLength, msgDelegate);
					aStack.Push(aProcess);
					stackList.Add(aProcess);
				}

				return aStack;
			}

			private void WriteEventLog(string msg, EventLogEntryType entryType)
			{
				EventLog.WriteEntry(eventLogID, msg, entryType);
				if (entryType == EventLogEntryType.Error)
				{
					LogError(msg);
				}
				else
				{
					LogInfo(msg);
				}
			}

			private void LogStoresToBeDeleted(DataTable dt)
			{
				LogInfo(" ");
				StringBuilder storeList = new StringBuilder();
				storeList.Append("Stores to be deleted: ");
				foreach (DataRow aRow in dt.Rows)
				{
					string stRid = aRow["ST_RID"].ToString();
					string stId = aRow["ST_ID"].ToString();
					storeList.Append(stId + "(" + stRid + ")  ");
				}
				LogInfo(storeList.ToString());

				_maxStoreIdLength = dt.AsEnumerable().Select(row => row["ST_ID"]).OfType<string>().Max(val => val.Length);
				_maxStoreIdLength = _maxStoreIdLength + 2;
				_numStoreDeleted = dt.Rows.Count;
			}

			private DataTable LogDBTableRowCounts(string title)
			{
				
				SystemData systemData = new SystemData();
				try
				{
					DataTable dt = GetDBTableRowCounts();

					LogInfo("===============================");
					LogInfo("  " + title);
					LogInfo("===============================");

					_maxTableNameLength = dt.AsEnumerable().Select(row => row["Table"]).OfType<string>().Max(val => val.Length);
					_maxTableNameLength = _maxTableNameLength + 2;

					foreach (DataRow aRow in dt.Rows)
					{
						String rec = new String(' ', 100);
						string table = aRow["Table"].ToString();
						string rowCount = aRow["RowCount"].ToString();
						rec = rec.Insert(0, table);
						rec = rec.Insert(_maxTableNameLength, rowCount);
						LogInfo(rec);
					}
					return dt;
				}
				catch
				{
					throw;
				}
				finally
				{
					if (systemData.ConnectionIsOpen)
					{
						systemData.CloseUpdateConnection();
					}
				}
			}

			private DataTable GetDBTableRowCounts()
			{

				SystemData systemData = new SystemData();
				try
				{
					DataTable dt = systemData.GetDBTableRowCounts();
					return dt;
				}
				catch
				{
					throw;
				}
				finally
				{
					if (systemData.ConnectionIsOpen)
					{
						systemData.CloseUpdateConnection();
					}
				}
			}

			/// <summary>
			/// Determines the number of rows removed from each table and logs that info
			/// </summary>
			/// <param name="title"></param>
			/// <returns></returns>
			private void LogDBRowCountDeleted(DataTable preData, DataTable postdata)
			{
				decimal totPreRows = 0;
				decimal totPostRows = 0;
				decimal totRowsRemoved = 0;
				LogInfo("=================================");
				LogInfo("  ROWS REMOVED FROM EACH TABLE");
				LogInfo("=================================");
				try
				{
					int MaxColLen = preData.AsEnumerable().Select(row => row["Table"]).OfType<string>().Max(val => val.Length);
					MaxColLen = MaxColLen + 2;

					foreach (DataRow preRow in _dtPreDelete.Rows)
					{
						String rec = new String(' ', 100);
						string table = preRow["Table"].ToString();
						DataRow[] postRows = postdata.Select("Table = '" + table + "'");
						if (postRows.Length > 0)
						{
							DataRow postRow = postRows[0];
							int preRowCount = int.Parse(preRow["RowCount"].ToString());
							int postRowCount = int.Parse(postRow["RowCount"].ToString());
							totPreRows += preRowCount;
							totPostRows += postRowCount;
							int rowsDeleted = postRowCount - preRowCount;

							if (rowsDeleted < 0)
							{
								rec = rec.Insert(0, table);
								rec = rec.Insert(MaxColLen, preRowCount.ToString() + " - ");
								rec = rec.Insert(MaxColLen + 13, postRowCount.ToString() + " = " );
								rec = rec.Insert(MaxColLen + 26, rowsDeleted.ToString());
								LogInfo(rec);
							}
						}
					}

					totRowsRemoved = totPreRows - totPostRows;
					LogInfo("Total rows before store delete: " + totPreRows + ". Total rows after store delete: " + totPostRows + ". Total rows deleted: " + totRowsRemoved + ".");
				}
				catch
				{
					throw;
				}
			}

			private void LogError(string msg)
			{
				try
				{
					_log.Error(msg);
				}
				catch
				{
					throw;
				}
			}

			public void LogDebug(string msg)
			{
				try
				{
					if (_log.IsDebugEnabled)
					{
						_log.Debug(msg);
					}
				}
				catch
				{
					throw;
				}
			}

			public void LogBufferFlush()
			{
				log4net.Repository.ILoggerRepository rep = LogManager.GetRepository();
				foreach (log4net.Appender.IAppender appender in rep.GetAppenders())
				{
					var buffered = appender as log4net.Appender.BufferingAppenderSkeleton;
					if (buffered != null)
					{
						buffered.Flush();
					}
				}
			}


			public void SetRecoveryModelSimple(string aConnString)
			{
				DataTable dt;
				SqlDataAdapter sda;
				SqlCommand sqlCommand = null;
				bool connectionOpen = false;
				try
				{
					try
					{
						string connectionString = aConnString;

						sqlCommand = new SqlCommand();
						sqlCommand.Connection = new SqlConnection(connectionString);
						sqlCommand.Connection.Open();
						connectionOpen = true;
					}
					catch (Exception ex)
					{
						string message = ex.ToString();
						LogError("FATAL DB Error: Error encountered during open of database");
					
						throw;
					}

					sqlCommand.CommandType = CommandType.Text;
					sqlCommand.CommandText = "SELECT recovery_model_desc FROM sys.databases WHERE name = '" + sqlCommand.Connection.Database + "'";
					dt = MIDEnvironment.CreateDataTable("model");
					sda = new SqlDataAdapter(sqlCommand);
					sda.Fill(dt);

					if (dt.Rows.Count > 0)
					{
						_recoveryModel = Convert.ToString(dt.Rows[0]["recovery_model_desc"]);
						SetRecoveryModelSimple(sqlCommand, sqlCommand.Connection.Database);
					}
				}
				catch (Exception ex)
				{
					LogError("UNEXPECTED EXCEPTION: " + ex.ToString());
					sqlCommand.Transaction.Rollback();
				}
				finally
				{
					if (connectionOpen)
					{
						sqlCommand.Connection.Close();
					}
				}
			}

			private void SetRecoveryModelSimple(SqlCommand aSqlCommand, string aDatabase)
			{
				try
				{
					aSqlCommand.CommandText = "ALTER DATABASE " + aDatabase + " SET RECOVERY SIMPLE";
					ExecuteCommand(aSqlCommand);
					LogInfo("DB recovery model set to SIMPLE");
				}
				catch (Exception ex)
				{
					LogError("UNEXPECTED EXCEPTION: " + ex.ToString());
				}
			}

			public void RestoreRecoveryModel(string aConnString)
			{
				SqlCommand sqlCommand = null;
				bool connectionOpen = false;
				try
				{
					try
					{
						string connectionString = aConnString;

						sqlCommand = new SqlCommand();
						sqlCommand.Connection = new SqlConnection(connectionString);
						sqlCommand.Connection.Open();
						connectionOpen = true;
					}
					catch (Exception ex)
					{
						string message = ex.ToString();
						LogError("FATAL DB Error: Error encountered during open of database");
						throw;
					}

					if (!string.IsNullOrEmpty(_recoveryModel))
					{
						RestoreRecoveryModel(sqlCommand, sqlCommand.Connection.Database);
					}
					LogInfo("DB recovery model set back to " + _recoveryModel);
				}
				catch (Exception ex)
				{
					LogError("UNEXPECTED EXCEPTION: " + ex.ToString());
				}
				finally
				{
					if (connectionOpen)
					{
						sqlCommand.Connection.Close();
					}
				}
			}

			private void RestoreRecoveryModel(SqlCommand aSqlCommand, string aDatabase)
			{
				try
				{
					aSqlCommand.CommandText = "ALTER DATABASE " + aDatabase + " SET RECOVERY " + _recoveryModel;
					ExecuteCommand(aSqlCommand);
				}
				catch (Exception ex)
				{
					LogError("UNEXPECTED EXCEPTION: " + ex.ToString());
				}
			}

			private bool ExecuteCommand(SqlCommand aSqlCommand)
			{
				bool successful = true;
				try
				{
					try
					{
						aSqlCommand.ExecuteNonQuery();
					}
					catch (Exception ex)
					{
						LogError("UNEXPECTED EXCEPTION: " + ex.ToString());
						successful = false;
					}
				}
				catch
				{
					successful = false;
				}
				return successful;
			}
		}
	}

	public class StoreDeleteProcess : ConcurrentProcess
	{
		//=======
		// FIELDS
		//=======
		private StoreData _storeData = null;
		private string _tableName = null;
		private string _tableNameCopy = null;
		private string _procName = null;
		private bool _isIndexed = false;
		private SessionAddressBlock _SAB;
		private int _batchSize = 1000;
		private int _storeRid;
		private string _storeId;
		private string _connectionString;
		private StoreData _pStoreData;
		private int _maxTableNameLength;
		private int _maxStoreIdLength;

		private MIDTimer _timer;
		private int _rowsDeleted = 0;
		private eMIDMessageLevel _messageLevel = eMIDMessageLevel.None;
		private log4net.ILog _log;
		private SqlCommand _cmd;
		private SqlConnection _databaseCommandConnection;
		private bool _isCopyLoad = false;
		private StoreDeleteCommon.SendMessageDelegate _msgDelegate;

		//=============
		// CONSTRUCTORS
		//=============


		public StoreDeleteProcess(string connectionString, SessionAddressBlock aSAB, string proc,  StoreData storeData, log4net.ILog log, int batchSize, int maxTableNameLength, int maxStoreIdLength, StoreDeleteCommon.SendMessageDelegate msgDelegate)
			: base(null)
		{
			try
			{
				_SAB = aSAB;
				_procName = proc;
				_storeData = storeData;
				_log = log;
				_batchSize = batchSize;
				_timer = new MIDTimer();
				_connectionString = connectionString;
				_maxTableNameLength = maxTableNameLength;
				_maxStoreIdLength = maxStoreIdLength;
				_storeId = "";
				_msgDelegate = msgDelegate;
				_tableName = proc.Replace("SP_MID_STORE_REMOVAL_", "");
				_tableName = _tableName.Replace("SP_MID_STORE_COPYLOAD_", "");
				_tableName = _tableName.Replace("SP_MID_STORE_SET_REMOVAL_", "");
				_tableName = _tableName.Replace("SP_MID_STORE_SET_COPYLOAD_", "");
				_tableNameCopy = _tableName + "_COPY";

				if (proc.Contains("COPYLOAD"))
				{
					_isCopyLoad = true;
				}
			}
			catch (Exception exc)
			{
				ExitMessageLevel = eMIDMessageLevel.Severe;
				LogError("Exception encountered in creating stack for procedure: " + proc + " " + exc.ToString());
				throw;
			}
		}

		//public StoreDeleteProcess(string connectionString, SessionAddressBlock aSAB, string tableName, int storeRid, string storeId, bool isIndexed, StoreData storeData, log4net.ILog log, int batchSize, int maxTableNameLength, int maxStoreIdLength)
		//    : base(null)
		//{
		//    try
		//    {
		//        _SAB = aSAB;
		//        _tableName = tableName;
		//        _isIndexed = isIndexed;
		//        _storeData = storeData;
		//        _log = log;
		//        _batchSize = batchSize;
		//        _storeId = storeId;
		//        _storeRid = storeRid;
		//        _timer = new MIDTimer();
		//        _connectionString = connectionString;
		//        _maxTableNameLength = maxTableNameLength;
		//        _maxStoreIdLength = maxStoreIdLength;
		//    }
		//    catch (Exception exc)
		//    {
		//        ExitMessageLevel = eMIDMessageLevel.Severe;
		//        LogError("Exception encountered in Constructor for Table: " + tableName);
		//        throw;
		//    }
		//}

		//===========
		// PROPERTIES
		//===========

		public MIDTimer Timer
		{
			get
			{
				return _timer;
			}
		}

		public int RowsDeleted
		{
			get { return _rowsDeleted; }
			//set { _rowsDeleted = value; }

		}

		public string ProcedureName
		{
			get { return _procName; }
			//set { _rowsDeleted = value; }

		}

		public eMIDMessageLevel MessageLevel
		{
			get { return _messageLevel; }
			//set { _rowsDeleted = _messageLevel; }

		}
		
		//========
		// METHODS
		//========

		override public void ExecuteProcess()
		{
			try
			{
				string msg = string.Empty;
				if (_isCopyLoad)
				{
					msg = _tableName + " Copy/Load Started ";
					LogInfo(msg);
					//_msgDelegate(DateTime.Now.ToString() + " " +  msg);
					//MyListBox.PutMsgInListBox(DateTime.Now.ToString() + " " + msg);
				}
				else
				{
					msg = _tableName + " Delete Started ";
					LogInfo(msg);
					//_msgDelegate(DateTime.Now.ToString() + " " + msg);
					//MyListBox.PutMsgInListBox(DateTime.Now.ToString() + " " + msg);
				}

				IsRunning = true;
				_timer.Start();

				_pStoreData = new StoreData(_connectionString);
				if (!_pStoreData.ConnectionIsOpen)
				{
					_pStoreData.OpenUpdateConnection();
				}

				_pStoreData.UpdateStoreDeleteInProgress(eStoreDeleteTableStatus.InProgress, _tableName);
				_pStoreData.CommitData();

				_rowsDeleted = _pStoreData.StoreProfile_ExecProc(_procName);
				_pStoreData.UpdateStoreDeleteInProgress(eStoreDeleteTableStatus.Completed, _tableName);
				_pStoreData.CommitData();
			}
			catch (ThreadAbortException)
			{
				try
				{
					LogError("Cancelled by User. Was Processing table: " + _tableName);
					_messageLevel = eMIDMessageLevel.Severe;
				}
				catch (InvalidOperationException)
				{
					LogError("Invalid Operation Exception thrown. Was Processing table: " + _tableName);
					_messageLevel = eMIDMessageLevel.Severe;
				}
			}
			catch (Exception exc)
			{
				LogError("Exception encountered in ExecuteProcess. Was Processing table: " + _tableName + " " + exc.ToString());
				_messageLevel = eMIDMessageLevel.Severe;
			}
			finally
			{
				try
				{
					// Remove work table if this was a copy/load
					if (_isCopyLoad)
					{
						// Truncate
						if (!_pStoreData.ConnectionIsOpen)
						{
							_pStoreData.OpenUpdateConnection();
						}
						_pStoreData.TruncateTable(_tableNameCopy);
						//_pStoreData.CommitData();

						// Drop _COPY table
						if (!_pStoreData.ConnectionIsOpen)
						{
							_pStoreData.OpenUpdateConnection();
						}
						_pStoreData.DropTable(_tableNameCopy);
						//_pStoreData.CommitData();
					}
					if (_pStoreData.ConnectionIsOpen)
					{
						_pStoreData.CommitData();
						_pStoreData.CloseUpdateConnection();
					}

					IsRunning = false;
					CompletionDateTime = DateTime.Now;
					ExitMessageLevel = _messageLevel;
					_timer.Stop();

					if (_rowsDeleted > 0)
					{
						String rec = new String(' ', 150);
						rec = rec.Insert(0, _tableName);
						if (_isCopyLoad)
						{
							rec = rec.Insert(_maxTableNameLength, " Rows_Copied:  " + _rowsDeleted);
						}
						else
						{
							rec = rec.Insert(_maxTableNameLength, " Rows_Deleted: " + _rowsDeleted);

						}
						rec = rec.Insert(_maxTableNameLength + 25, _timer.ElaspedTimeString);
						//_msgDelegate(DateTime.Now.ToString() + " " + rec);
						//MyListBox.PutMsgInListBox(DateTime.Now.ToString() + " " + rec);
						LogInfo(rec);
						LogBufferFlush();
					}
				}
				catch (Exception exc)
				{
					LogError("Exception encountered in ExecuteProcess.Finally Block. Was Processing table: " + _tableName + " " + exc.ToString());
					_messageLevel = eMIDMessageLevel.Severe;
					throw;
				}
				finally
				{

				}
			}
		}
		private void LogInfo(string msg)
		{
			try
			{
				if (_log.IsInfoEnabled)
				{
					_log.Info(msg);
				}
			}
			catch
			{
				throw;
			}
		}

		private void LogError(string msg)
		{
			try
			{
				_log.Error(msg);
			}
			catch
			{
				throw;
			}
		}

		public void LogDebug(string msg)
		{
			try
			{
				if (_log.IsDebugEnabled)
				{
					_log.Debug(msg);
				}
			}
			catch
			{
				throw;
			}
		}

		public void LogBufferFlush()
		{
			log4net.Repository.ILoggerRepository rep = LogManager.GetRepository();
			foreach (log4net.Appender.IAppender appender in rep.GetAppenders())
			{
				var buffered = appender as log4net.Appender.BufferingAppenderSkeleton;
				if (buffered != null)
				{
					buffered.Flush();
				}
			}
		}


		/// <summary>
		/// get DB connection without using MID datalayer
		/// </summary>
		/// <returns></returns>
		private bool DatabaseConnectionOpen()
		{
			try
			{
				_databaseCommandConnection = new SqlConnection(_connectionString);
				_databaseCommandConnection.Open();

				_cmd = new SqlCommand();
				_cmd.Connection = _databaseCommandConnection;
				_cmd.CommandTimeout = 0;
			}
			catch (Exception exception)
			{
				LogError("Error:" + exception.Message);
				return false;
			}

			return true;
		}

		private bool DatabaseConnectionEnsureOpen()
		{

			try
			{
				if (_databaseCommandConnection.State != ConnectionState.Open)
				{
					_databaseCommandConnection.Open();
				}

			}
			catch (Exception exception)
			{
				LogError("Error:" + exception.Message);
				return false;
			}

			return true;
		}

		private void DatabaseConnectionClose()
		{
			try
			{
				//Close the cmd connection string
				_databaseCommandConnection.Close();
			}
			catch
			{
			}
		}

	}

	public static class StoreDeleteCommon
	{
		public delegate void SendMessageDelegate(string msg);
	}

	public static class MyListBox
	{
		private static System.Windows.Forms.ListBox myListBox = null;

		public static void SetListBox(System.Windows.Forms.ListBox aListBox)
		{
			myListBox = aListBox;
		}

		public static void PutMsgInListBox(string msg)
		{
			try
			{
				if (myListBox.InvokeRequired)
				{
					myListBox.Invoke((System.Windows.Forms.MethodInvoker)delegate
					{
						myListBox.Items.Add(msg);
						//myListBox.SelectedIndex = myListBox.Items.Count - 1;
					});
				}
				else
				{
					myListBox.Items.Add(msg);
					//myListBox.SelectedIndex = myListBox.Items.Count - 1;
				}
			}
			catch (Exception exe)
			{
				throw;
			}
		}
	}
}
