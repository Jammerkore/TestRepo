using System;
using System.IO;
using System.Configuration;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Channels;
using System.Globalization;
using System.Data;
using System.Linq;
using System.Text;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

using log4net;
using log4net.Config;

[assembly: log4net.Config.XmlConfigurator(Watch = true)]

namespace MIDRetail.HeaderAllocationLoad
{
	class HeaderAllocationLoad
	{
		[STAThread]
		static int Main(string[] args)
		{
			//log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
			log4net.ILog log = log4net.LogManager.GetLogger("Activity");
			log4net.ILog errorlog = log4net.LogManager.GetLogger("Errors");

		    HeaderAllocationLoadWorker poAllocation = new HeaderAllocationLoadWorker(log, errorlog);
		    return poAllocation.Process(args);
		}

		public class HeaderAllocationLoadWorker
		{
			string sourceModule = "HeaderAllocationLoad.cs";
			string eventLogID = "HeaderAllocationLoad";
			SessionAddressBlock _SAB;
			SessionSponsor _sponsor;
			IMessageCallback _messageCallback;
			log4net.ILog _log;
			log4net.ILog _errorLog;
			bool _LOGGING = false;
			ScheduleData _scheduleData;
			int concurrentProcesses = 5;
			eMIDMessageLevel highestMessage;
			DataRow taskListRow;
			int userRid;
			string tasklistName;
			string msg;
			string message = null;
			bool errorFound = false;
			DataTable dtTask;

			System.Runtime.Remoting.Channels.IChannel channel;
			int _taskListRid = Include.NoRID;
			int _taskSeq = 0;
			int _processId = Include.NoRID;
			string _accumAllDays = string.Empty;

			string _packTableName = string.Empty;
			string _bulkTableName = string.Empty;
			//HeaderEnqueue _headerEnqueue;  // TT#1185 - Verify ENQ before Update
			//bool _hdrEnqueued = false;     // TT#1185 - Verify ENQ before Update

			int _headersProcessed = 0;
			int _headersSuccessful = 0;
			int _headersFailed = 0;

			public HeaderAllocationLoadWorker(log4net.ILog log, log4net.ILog errorLog)
			{
				_log = log;
				_errorLog = errorLog;
			}

			public int Process(string[] args)
			{

				try
				{
					_messageCallback = new BatchMessageCallback();
					_sponsor = new SessionSponsor();
					_SAB = new SessionAddressBlock(_messageCallback, _sponsor);

					if (!EventLog.SourceExists(eventLogID))
					{
						EventLog.CreateEventSource(eventLogID, null);
					}

					// Register callback channel

					try
					{
						channel = _SAB.OpenCallbackChannel();
					}
					catch (Exception exception)
					{
						errorFound = true;
						EventLog.WriteEntry(eventLogID, "Error opening port #0 - " + exception.Message, EventLogEntryType.Error);
						return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
					}

					// Create Sessions

					try
					{
						_SAB.CreateSessions((int)eServerType.Client | (int)eServerType.Hierarchy | (int)eServerType.Store | (int)eServerType.Application);
					}
					catch (Exception exception)
					{
						errorFound = true;
						Exception innerE = exception;
						while (innerE.InnerException != null)
						{
							innerE = innerE.InnerException;
						}
						EventLog.WriteEntry(eventLogID, "Error creating sessions - " + innerE.Message, EventLogEntryType.Error);
						return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
					}

					//====================
					// Process Arguments
					//====================
					errorFound = ProcessArgs(args);
					if (errorFound)
					{
						return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
					}

					_scheduleData = new ScheduleData();

					userRid = Include.UndefinedUserRID;
					tasklistName = string.Empty;

					//=========================================
					// DO this if running a specific task list
					//=========================================
					if (_taskListRid != Include.NoRID)
					{
						taskListRow = _scheduleData.TaskList_Read(_taskListRid);
						if (taskListRow != null)
						{
							userRid = Convert.ToInt32(taskListRow["USER_RID"], CultureInfo.CurrentUICulture);
							tasklistName = taskListRow["TASKLIST_NAME"].ToString();
						}
						else
						{
							EventLog.WriteEntry(eventLogID, "Invalid tasklist RID:" + _taskListRid.ToString(), EventLogEntryType.Error);
							System.Console.Write("Invalid tasklist RID:" + _taskListRid.ToString());
							errorFound = true;
							return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
						}
					}

                    // Begin TT#2249 - JSmith - Wrong name for process
                    //eSecurityAuthenticate authentication = _SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
                    //MIDConfigurationManager.AppSettings["Password"], eProcesses.SizeDayToWeekSummary);
                    eSecurityAuthenticate authentication = _SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
                    MIDConfigurationManager.AppSettings["Password"], eProcesses.headerAllocationLoad);
                    // End TT#2249

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
						errorFound = true;
						EventLog.WriteEntry(eventLogID, "Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"], EventLogEntryType.Error);
						System.Console.Write("Unable to log in with user:" + MIDConfigurationManager.AppSettings["User"] + " password:" + MIDConfigurationManager.AppSettings["Password"]);
						//					return Convert.ToInt32(eReturnCode.fatal,CultureInfo.CurrentUICulture);
						return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
					}

					if (_processId != Include.NoRID)
					{
						_SAB.ClientServerSession.Initialize(_processId);
					}
					else
					{
						_SAB.ClientServerSession.Initialize();
					}

                    //_SAB.HierarchyServerSession.Initialize();  // TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
					_SAB.ApplicationServerSession.Initialize();
					//if (_SAB.SchedulerServerSession != null)
					//{
					//    _SAB.SchedulerServerSession.Initialize();
					//}
					//else
					//{
					//    _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Warning, "Scheduler Service not found -- Schedules will not be purged", sourceModule);
					//}
					_SAB.StoreServerSession.Initialize();
                    // Begin TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.
                    // StoreServerSession must be initialized before HierarchyServerSession 
                    _SAB.HierarchyServerSession.Initialize();
                    // End TT#1905 - JSmith - Versioning_Test Interfaced after interface in a new store to a dynamic set when process the alloc override receve severe error.


					//============
					// Logging
					//============
					//string logging = MIDConfigurationManager.AppSettings["SizeDayToWeekSummaryLog"];
					//_LOGGING = Include.ConvertStringToBool(logging);
					//string logFilePath = MIDConfigurationManager.AppSettings["SizeDayToWeekSummaryLogFilePath"];
					//if (logFilePath == null)
					//    logFilePath = ".";
					//string time = System.DateTime.Now.ToString("HHmmssfff").ToString();
					//string date = System.DateTime.Now.ToString("yyyyMMdd").ToString();

					//foreach (string arg in args)
					//{
					//    LogMessage("Argument: " + arg);
					//}
					//LogMessage("Concurrent Processes: " + concurrentProcesses);


					//============
					// Logging
					//============
					string msg1 = "Task List Parameters. Task List Rid: " + _taskListRid + " Task Seq: " + _taskSeq + " Task List Name: " + tasklistName;
					//_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg1, sourceModule, true);
					//LogMessage(msg1);

					if (_taskListRid != Include.NoRID)
					{
						msg = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_TaskListBegin);
						msg = msg.Replace("{0}", tasklistName);
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, sourceModule, true);
					}

					//=================
					// PROCESSING
					//=================
					if (!errorFound)
					{
						_bulkTableName = MIDConfigurationManager.AppSettings["CustomBulkTableName"];
						_packTableName = MIDConfigurationManager.AppSettings["CustomPackTableName"];
						// No Task List, so current week will be processed.
						if (_taskListRid == Include.NoRID)
						{
							errorFound = ProcessPOAllocation();
							DisplayCounts();
						}
						else
						{
							// Run Specific task list.
							dtTask = _scheduleData.TaskSizeDayToWeekSummary_ReadByTaskList(_taskListRid, _taskSeq);

							foreach (DataRow aRow in dtTask.Rows)
							{
								int cdrRid = Convert.ToInt32(aRow["CDR_RID"], CultureInfo.CurrentUICulture);
								int nodeRid = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);
								errorFound = ProcessPOAllocation();
							}
							DisplayCounts();
						}
					}
				}

				catch (Exception exception)
				{
					errorFound = true;
					message = "";
					while (exception != null)
					{
						message += " -- " + exception.Message;
						exception = exception.InnerException;
					}
					_SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, message, sourceModule);
					_errorLog.Error(message);
					_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
				}
				finally
				{
					if (!errorFound)
					{
						if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
						{
							_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", _SAB.GetHighestAuditMessageLevel());
						}
					}
					else
					{
						if (_SAB.ClientServerSession != null && _SAB.ClientServerSession.Audit != null)
						{
							_SAB.ClientServerSession.Audit.UpdateHeader(eProcessCompletionStatus.Failed, eMIDTextCode.sum_Failed, "", _SAB.GetHighestAuditMessageLevel());
						}
					}

					highestMessage = _SAB.CloseSessions();

		
				}

				return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
			}

			private void DisplayCounts()
			{
				LogInfo("PROCESS COMPLETED");
				LogInfo("Headers processed:              " + _headersProcessed);
				LogInfo("Headers successfully allocated: " + _headersSuccessful);
				LogInfo("Headers with errors:			 " + _headersFailed);
			}

			public bool ProcessArgs(string[] args)
			{
				bool errorFound = false;
				//_taskListRid = Include.NoRID;
				//_taskSeq = 0;
				//_processId = 0;

				try
				{
				//    if (args.Length > 0)
				//    {
				//        //================
				//        // From Task List
				//        //================
				//        if (args[0] == Include.SchedulerID)
				//        {
				//            _taskListRid = Convert.ToInt32(args[1]);
				//            _taskSeq = Convert.ToInt32(args[2]);
				//            _processId = Convert.ToInt32(args[3]);
				//        }
				//        else
				//        //====================
				//        // From command line
				//        //====================
				//        {
				//            _overrideDate = args[0].ToString();
				//            if (args.Length > 1)
				//            {
				//                _overrideNodeId = args[1].ToString();
				//            }

				//            errorFound = SetOverrideDateRange(_overrideDate);
				//        }
				//    }

					return errorFound;
				}
				catch
				{
					EventLog.WriteEntry(eventLogID, "Unable to process SizeDayToWeekSummary arguments. " + args.ToString(), EventLogEntryType.Error);
					System.Console.Write("Unable to process SizeDayToWeekSummary arguments. " + args.ToString());
					return true;
				}
				finally
				{

				}
			}

			private bool ProcessPOAllocation()
			{
				try
				{
					bool errorFound = false;
					HeaderAllocationLoadData poAllocData = new HeaderAllocationLoadData();
					DataTable dtHeaders = poAllocData.DistinctHeader_Read(_bulkTableName, _packTableName);
					LogInfo(dtHeaders.Rows.Count.ToString() + " headers will be processed.");

					//======================
					// Process Each Header
					//======================
					foreach (DataRow hdrRow in dtHeaders.Rows)
					{
						_headersProcessed++;
						string headerId = hdrRow["HEADER_ID"].ToString();
						DataTable dtBulk = poAllocData.BulkAlloc_Read(_bulkTableName, headerId);
						DataTable dtPack = poAllocData.PackAlloc_Read(_packTableName, headerId);
						DataRow[] bulkRows = new DataRow[dtBulk.Rows.Count];
						dtBulk.Rows.CopyTo(bulkRows, 0);
						DataRow[] packRows = new DataRow[dtPack.Rows.Count];
						dtPack.Rows.CopyTo(packRows, 0);
						//DataRow[] bulkRows = dtBulk.Select("HEADER_ID = '" + headerId + "'");
						//DataRow[] packRows = dtPack.Select("HEADER_ID = '" + headerId + "'");
						errorFound = AllocateHeader(headerId, bulkRows, packRows);
						if (errorFound)
						{
							_headersFailed++;
						}
						else
						{
							_headersSuccessful++;
						}
					}

					return errorFound;
				}
				catch
				{
					throw;
				}
			}



			private bool AllocateHeader(string headerId, DataRow[] bulkRows, DataRow[] packRows)
            {
                //ApplicationSessionTransaction aTransaction = new ApplicationSessionTransaction(_SAB);  // TT#1185 - Verify ENQ before Update
                ApplicationSessionTransaction aTransaction = _SAB.ApplicationServerSession.CreateTransaction(); // TT#1185 - Verify ENQ before Update
                string enqMessage; // TT#1185 - Verify ENQ before Update
				try
				{
					bool errorFound = false;

                    //ApplicationSessionTransaction aTransaction = new ApplicationSessionTransaction(_SAB); // TT#1185 - Verify ENQ before Update 
					AllocationProfile ap = new AllocationProfile(aTransaction, headerId, Include.NoRID, _SAB.ApplicationServerSession);
					if (ap.Key == Include.NoRID)
					{
						LogError("Header: " + headerId + " *ERROR* HeaderId not found in Allocation.");
						errorFound = true;
						return errorFound;
					}
                    // begin TT#1185 - Verify ENQ before Update
                    // Begin TT#1966-MD - JSmith - DC Fulfillment
                    if (ap.IsMasterHeader
                        || ap.IsSubordinateHeader)
                    {
                        LogError(MIDText.GetText(eMIDTextCode.msg_al_ActionNotValidForMasterOrSubordinate, headerId));
                        
                        errorFound = true;
                        return errorFound;
                    }
                    // End TT#1966-MD - JSmith - DC Fulfillment
                    List<int> hdrRidList = new List<int>();
                    hdrRidList.Add(ap.Key);
                    if (!aTransaction.EnqueueHeaders(hdrRidList, out enqMessage))
                    {
                        LogError(enqMessage); 
                        errorFound = true;
                        return errorFound;
                    }
                    ap.ReReadHeader();
                    AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
                    apl.Add(ap);
                    aTransaction.SetMasterProfileList(apl);
                    //AllocationProfileList apl = new AllocationProfileList(eProfileType.Allocation);
                    //apl.Add(ap);
                    //aTransaction.SetMasterProfileList(apl);
                    //EnqueueHeader(ap, aTransaction);
                    // end TT#1185 - Verify ENQ before Update

					//=============================================================
                    //// If Header is Released, RESET it so we can reallocate it. // TT#754 - AGallagher - Allocation - When a Reset is  processed - the Cancel Allocation Action should also be processed
                    // If Header is Released, RESET and CANCEL ALLOCATION it so we can reallocate it.  // TT#754 - AGallagher - Allocation - When a Reset is  processed - the Cancel Allocation Action should also be processed
					//=============================================================
                    // BEGIN TT#754 - AGallagher - Allocation - When a Reset is  processed - the Cancel Allocation Action should also be processed
                    ApplicationBaseAction aMethod = aTransaction.CreateNewMethodAction(eMethodType.Reset);
                    GeneralComponent aComponent = new GeneralComponent(eGeneralComponentType.Total); 
					bool aReviewFlag = false;
                    bool aUseSystemTolerancePercent = false;
                    double aTolerancePercent = Include.DefaultBalanceTolerancePercent;
                    int aStoreFilter = Include.AllStoreFilterRID;
                    int aWorkFlowStepKey = -1;
                    // END TT#754 - AGallagher - Allocation - When a Reset is  processed - the Cancel Allocation Action should also be processed

                    if (ap.GetHeaderAllocationStatus(true) == eHeaderAllocationStatus.Released)
					{
						//bool success = ap.ResetAction();
						//if (!success)
                        AllocationWorkFlowStep aAllocationWorkFlowStep
                                = new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);

                        aTransaction.DoAllocationAction(aAllocationWorkFlowStep);
                        eAllocationActionStatus actionStatus = aTransaction.AllocationActionAllHeaderStatus;


                        if (actionStatus != eAllocationActionStatus.ActionCompletedSuccessfully)
						{
							LogError("Header: " + headerId + " *ERROR* could not be RESET to re-allocate.");
							errorFound = true;
							return errorFound;
						}
                        // BEGIN TT#754 - AGallagher - Allocation - When a Reset is  processed - the Cancel Allocation Action should also be processed
                        if (actionStatus == eAllocationActionStatus.ActionCompletedSuccessfully)
                        {
                            aMethod = aTransaction.CreateNewMethodAction(eMethodType.BackoutAllocation);
                      
                            aAllocationWorkFlowStep
                                = new AllocationWorkFlowStep(aMethod, aComponent, aReviewFlag, aUseSystemTolerancePercent, aTolerancePercent, aStoreFilter, aWorkFlowStepKey);
                            aTransaction.DoAllocationAction(aAllocationWorkFlowStep);
                            actionStatus = aTransaction.AllocationActionAllHeaderStatus;


                            if (actionStatus != eAllocationActionStatus.ActionCompletedSuccessfully)
                            {
                                LogError("Header: " + headerId + " *ERROR* could not be Cancelled to re-allocate.");
                                errorFound = true;
                                return errorFound;
                            }
                        }
                        // END TT#754 - AGallagher - Allocation - When a Reset is  processed - the Cancel Allocation Action should also be processed
					}

					//====================
					// Allocate BULK
					//====================
					if (bulkRows.Length > 0)
					{
						int[] colorCodeRids = ap.GetBulkColorCodeRIDs();

						if (colorCodeRids.Length > 0)
						{
							int colorCodeRid = colorCodeRids[0];
							HdrColorBin hcb = ap.GetHdrColorBin(colorCodeRid);
							HdrSizeBin hsb = null;
							foreach (DataRow bulkRow in bulkRows)
							{
								string sizeCodeId = bulkRow["SIZE_CODE"].ToString();
								string storeId = bulkRow["STORE_ID"].ToString();
								int storeRid = Include.NoRID;
								if (bulkRow["ST_RID"] != null && bulkRow["ST_RID"].ToString() != string.Empty)
								{
									storeRid = int.Parse(bulkRow["ST_RID"].ToString());
								}
								else
								{
									LogError("Header: " + headerId + " *ERROR* store ID: " + storeId + " could not be found.");
									errorFound = true;
									continue;
								}
								int sizeCodeRid = Include.NoRID;
								if (bulkRow["SIZE_CODE_RID"] != null && bulkRow["SIZE_CODE_RID"].ToString() != string.Empty)
								{
									sizeCodeRid = int.Parse(bulkRow["SIZE_CODE_RID"].ToString());
								}
								else
								{
									LogError("Header: " + headerId + " *ERROR* size code ID: " + sizeCodeId + " could not be found.");
									errorFound = true;
									continue;
								}
								int qty = int.Parse(bulkRow["QTY"].ToString());

								try
								{
									hsb = hcb.GetSizeBin(sizeCodeRid);
								}
								catch (MIDException ex)
								{
									LogError("Header: " + headerId + " *ERROR* could not find size: " + sizeCodeId + " on header. " + ex.ToString());
									errorFound = true;
                                    continue;  // TT#751 - AGallagher - Allocation - Object reference not set to an instance of an object - when Pack does not exist on the header.
								}

								ap.SetStoreQtyAllocated(hsb, ap.StoreIndex(storeRid), qty, eDistributeChange.ToAll, false);
							}
						}
						else
						{
							LogError("Header: " + headerId + " *ERROR* rows were found on " + _bulkTableName + " table, but no bulk colors were found on header in Allocation.");
							errorFound = true;
						}
					}

					//====================
					// Allocate PACKS
					//====================
					if (packRows.Length > 0)
					{
						foreach (DataRow packRow in packRows)
						{
							int storeRid = -1;
							string storeId = packRow["STORE_ID"].ToString();
							if (packRow["ST_RID"] != null && packRow["ST_RID"].ToString() != string.Empty)
							{
								storeRid = int.Parse(packRow["ST_RID"].ToString());
							}
							else
							{
								LogError("Header: " + headerId + " *ERROR* store ID: " + storeId + " could not be found.");
								errorFound = true;
								continue;
							}
							string packId = packRow["PACK_ID"].ToString();
							int noOfPacks = int.Parse(packRow["NO_OF_PACKS"].ToString());
							PackHdr packHdr = null;
							try
							{
								packHdr = ap.GetPackHdr(packId);
							}
							catch (MIDException ex)
							{
								LogError("Header: " + headerId + " *ERROR* could not find pack: " + packId + " on header. " + ex.ToString());
								errorFound = true;
                                continue;  // TT#751 - AGallagher - Allocation - Object reference not set to an instance of an object - when Pack does not exist on the header.
							}
							
							ap.SetStoreQtyAllocated(packHdr, ap.StoreIndex(storeRid), noOfPacks, eDistributeChange.ToAll, false);
						}
					}

					//=====================
					// Update Header
					//=====================
					try
					{
                        // BEGIN TT#753 - AGallagher - Allocation - Header encountered an error - although was released.   Should not have released with an error
                        if (errorFound == true)
                        {
                            LogError("Header: " + headerId + " *ERROR* Review above errors! Could not be set to Released.");
                            errorFound = true;
                        }
                        else
                        // END TT#753 - AGallagher - Allocation - Header encountered an error - although was released.   Should not have released with an error
						if (ap.GetHeaderAllocationStatus(true) == eHeaderAllocationStatus.AllInBalance) 
                        {
							//================
							// Release it!
							//================
							ap.Action(eAllocationMethodType.Release, new GeneralComponent(eGeneralComponentType.Total), double.MaxValue, Include.AllStoreFilterRID, true);
						}
						else
						{
                            LogError("Header: " + headerId + " *ERROR* is allocated OUT OF BALANCE! Could not be set to Released.");
							errorFound = true;
						}

						if (ap.WriteHeader())
						{
							LogDebug("Header: " + headerId + " successfully updated.");
						}
						else
						{
							LogError("Header: " + headerId + " *ERROR* could not be updated!");
							errorFound = true;
						}
					}
					catch (Exception Ex)
					{
						LogError("Header: " + headerId + " *ERROR* processing exception! " + Ex.ToString());
						errorFound = true;
					}
					return errorFound;

				}
				catch
				{
					throw;
				}
				finally
				{
                    // begin TT#1185 - Verify ENQ before Update
                    aTransaction.DequeueHeaders();
                    //if (_hdrEnqueued)
                    //{
                    //    _headerEnqueue.DequeueHeaders();

                    //    _hdrEnqueued = false;
                    //}
                    // end TT#1185 - Verify ENQ before Update
				}
			}

            // begin TT#1185 - Verify ENQ before Update

            //private void EnqueueHeader(AllocationProfile ap, ApplicationSessionTransaction trans)
            //{
            //    // ==============
            //    // Enqueue Header
            //    // ==============
            //    AllocationHeaderProfile ahp = new AllocationHeaderProfile(ap.HeaderID, ap.Key);

            //    AllocationHeaderProfileList ahpList = new AllocationHeaderProfileList(eProfileType.AllocationHeader);

            //    try
            //    {
            //        ahpList.Clear();

            //        ahpList.Add(ahp);

            //        _headerEnqueue = new HeaderEnqueue(trans, ahpList);

            //        _headerEnqueue.EnqueueHeaders();

            //        ap.ReReadHeader();

            //        _hdrEnqueued = true;
            //    }

            //    catch (HeaderConflictException)
            //    {
            //        SecurityAdmin secAdmin = new SecurityAdmin();

            //        string msgText = "Allocation Header " + ahp.HeaderID + " is currently in use by User(s): ";
            //        foreach (HeaderConflict hc in _headerEnqueue.HeaderConflictList)
            //        {
            //            msgText += System.Environment.NewLine + secAdmin.GetUserName(hc.UserRID);
            //        }
            //        msgText += System.Environment.NewLine;
            //        LogError(msgText);
            //    }

            //}

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
						//_log.Error(msg);
						_errorLog.Error(msg);
				}
				catch
				{
					throw;
				}
			}

			private void LogDebug(string msg)
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
		}
	}
}
