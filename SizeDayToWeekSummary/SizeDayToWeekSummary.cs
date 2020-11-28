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
using System.Threading;
using System.Linq;
using System.Text;

using MIDRetail.Business;
using MIDRetail.Business.Allocation;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;

namespace MIDRetail.SizeDayToWeekSummary
{
	class SizeDayToWeekSummary
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static int Main(string[] args)
		{
			SizeDayToWeekSummaryWorker sizeSummary = new SizeDayToWeekSummaryWorker();
			return sizeSummary.Process(args);
		}

		public class SizeDayToWeekSummaryWorker
		{
			string sourceModule = "SizeDayToWeekSummary.cs";
			string eventLogID = "SizeDayToWeekSummary";
			SessionAddressBlock _SAB;
			SessionSponsor _sponsor;
			IMessageCallback _messageCallback;
			MIDLog _log = null;
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

			int _drpRid = Include.UndefinedCalendarDateRange;
			int _nodeRid = Include.NoRID;
			DateRangeProfile _overrideDateRange = null;
			string _overrideNodeId = string.Empty;
			string _overrideDate = string.Empty;
			string _styleId = string.Empty;
			string _colorCodeId = string.Empty;
			private HierarchyLevelProfile _styleLevelProf = null;
			private HierarchyLevelProfile _colorLevelProf = null;

			//ArrayList _summaryProcessList;

			private int _styleCnt = 0;
			private int _colorCnt = 0;
			private int _sizeCnt = 0;
			private int _totReads = 0;
			private int _totUpdates = 0;
			private int _valCnt = 0;

			private Dictionary<int, int>	_storeRidDictionary;
			private Dictionary<int, string> _storeIdDictionary;



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

					eSecurityAuthenticate authentication = _SAB.ClientServerSession.UserLogin(MIDConfigurationManager.AppSettings["User"],
                    MIDConfigurationManager.AppSettings["Password"], eProcesses.SizeDayToWeekSummary);

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


					// Concurrent Processes. (Default = 5)
                    // Begin TT#1054 - JSmith - Relieve Intransit not working.
                    //string strParm = ConfigurationSettings.AppSettings["ConcurrentProcesses"];
                    string strParm = MIDConfigurationManager.AppSettings["ConcurrentProcesses"];
                    // End TT#1054
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

					//============
					// Logging
					//============
					string logging = MIDConfigurationManager.AppSettings["SizeDayToWeekSummaryLog"];
					_LOGGING = Include.ConvertStringToBool(logging);
					string logFilePath = MIDConfigurationManager.AppSettings["SizeDayToWeekSummaryLogFilePath"];
					if (logFilePath == null)
						logFilePath = ".";
					string time = System.DateTime.Now.ToString("HHmmssfff").ToString();
					string date = System.DateTime.Now.ToString("yyyyMMdd").ToString();
					if (_LOGGING)
					{
                    //Begin  TT#339 - MD - Modify Forecast audit message - RBeck
                        //_log = new MIDLog(eventLogID + date + "-", logFilePath, _SAB.ClientServerSession.UserRID, eventLogID, int.Parse(time));
                        _log = new MIDLog(eventLogID + date + "-", logFilePath, _SAB.ClientServerSession.UserRID, eventLogID, "");
                    //End    TT#339 - MD - Modify Forecast audit message - RBeck
                    }

					foreach (string arg in args)
					{
						LogMessage("Argument: " + arg);
					}
					//LogMessage("Concurrent Processes: " + concurrentProcesses);

					//=============================================
					// Get the RID for the OVERRIDE DATE entered
					//=============================================
					try
					{
						if (_overrideDateRange != null)
						{
							_drpRid = _SAB.ApplicationServerSession.Calendar.AddDateRange(_overrideDateRange);
						}
					}
					catch
					{
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Invalid Override date: " + _overrideDate, sourceModule);
						return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
					}

					//=============================================
					// Get the RID for the OVERRIDE NODE entered
					//=============================================
					try
					{
						if (_overrideNodeId != string.Empty)
						{
							_nodeRid = _SAB.HierarchyServerSession.GetNodeRID(_overrideNodeId);
							if (_nodeRid == Include.NoRID)
							{
								_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Could not find Override Merchandise Node: " + _overrideNodeId, sourceModule);
								return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
							}
						}
					}
					catch
					{
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, "Invalid Override Merchandise Node: " + _overrideNodeId, sourceModule);
						return Convert.ToInt32(eMIDMessageLevel.Severe, CultureInfo.CurrentUICulture);
					}

					//============
					// Logging
					//============
					string msg1 = "Task List Parameters. Task List Rid: " + _taskListRid + " Task Seq: " + _taskSeq + " Task List Name: " + tasklistName;
					//_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg1, sourceModule, true);
					LogMessage(msg1);
					// Begin TT#763 - stodd - default should be current - 1
					string msg2 = string.Empty;
					if (_drpRid == Include.UndefinedCalendarDateRange && _nodeRid == Include.NoRID)
					{	
						// No overrides
					}
					else
					{
						msg2 = "Override Parameters. Override Date: " + _overrideDate + " (" + _drpRid + ")" + " Override Node ID: " + _overrideNodeId + " (" + _nodeRid + ")";
						LogMessage(msg2);
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg2, sourceModule, true);
					}
					// End TT#763 - stodd - default should be current - 1

					if (_taskListRid != Include.NoRID)
					{
                        // Begin TT#3423 -JSmith - Ran the Day to Week Summary in the Post Conversion and receive an Error Invalid Object Name
                        //msg = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_TaskListBegin);
                        msg = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_TaskListBegin, false);
                        // End TT#3423 -JSmith - Ran the Day to Week Summary in the Post Conversion and receive an Error Invalid Object Name
						msg = msg.Replace("{0}", tasklistName);
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, sourceModule, true);
					}

					//=================
					// PROCESSING
					//=================
					if (!errorFound)
					{
						// No Task List, so current week will be processed.
						if (_taskListRid == Include.NoRID)
						{
							errorFound = ProcessSizeDayToWeekSummary(concurrentProcesses, _drpRid, _nodeRid);
						}
						else
						{
							// Run Specific task list.
							dtTask = _scheduleData.TaskSizeDayToWeekSummary_ReadByTaskList(_taskListRid, _taskSeq);

							foreach (DataRow aRow in dtTask.Rows)
							{
								int cdrRid = Convert.ToInt32(aRow["CDR_RID"], CultureInfo.CurrentUICulture);
								int nodeRid = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);
								errorFound = ProcessSizeDayToWeekSummary(concurrentProcesses, cdrRid, nodeRid);
							}
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

					if (_LOGGING)
					{
						_log.CloseLogFile();
					}
				}

				return Convert.ToInt32(highestMessage, CultureInfo.CurrentUICulture);
			}

			public bool ProcessArgs(string[] args)
			{
				bool errorFound = false;
				_taskListRid = Include.NoRID;
				_taskSeq = 0;
				_processId = 0;

				try
				{
					if (args.Length > 0)
					{
						//================
						// From Task List
						//================
						if (args[0] == Include.SchedulerID)
						{
							_taskListRid = Convert.ToInt32(args[1]);
							_taskSeq = Convert.ToInt32(args[2]);
							_processId = Convert.ToInt32(args[3]);
						}
						else
						//====================
						// From command line
						//====================
						{
							_overrideDate = args[0].ToString();
							if (args.Length > 1)
							{
								_overrideNodeId = args[1].ToString();
							}

							errorFound = SetOverrideDateRange(_overrideDate);
						}
					}
					
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

			private bool SetOverrideDateRange(string oDate)
			{
				bool errorFound = false;
				try
				{
					_overrideDateRange = new DateRangeProfile(Include.NoRID);

					//=======================================
					// Date format "-1,-5" for dynamic weeks
					//=======================================
					if (oDate.Contains(","))
					{
						try
						{
							string[] sep = new string[] { "," };
							string[] fromTo = oDate.Split(sep, StringSplitOptions.None);
							int from = int.Parse(fromTo[0]);
							int to = int.Parse(fromTo[1]);
							_overrideDateRange.StartDateKey = from;
							_overrideDateRange.EndDateKey = to;
							_overrideDateRange.DateRangeType = eCalendarRangeType.Dynamic;
							_overrideDateRange.SelectedDateType = eCalendarDateType.Week;
							_overrideDateRange.RelativeTo = eDateRangeRelativeTo.Current;
						}
						catch
						{
							errorFound = InvalidDateParm(oDate);
						}
					}
					//================================================
					// Date format "YYYYWW-YYYYWW" for static weeks
					//================================================
					else if (oDate.Length == 13)
					{
						try
						{
							string[] sep = new string[] { "-" };
							string[] fromTo = oDate.Split(sep, StringSplitOptions.None);
							int from = int.Parse(fromTo[0]);
							int to = int.Parse(fromTo[1]);
							_overrideDateRange.StartDateKey = from;
							_overrideDateRange.EndDateKey = to;
							_overrideDateRange.DateRangeType = eCalendarRangeType.Static;
							_overrideDateRange.SelectedDateType = eCalendarDateType.Week;
							_overrideDateRange.RelativeTo = eDateRangeRelativeTo.None;
						}
						catch
						{
							errorFound = InvalidDateParm(oDate);
						}
					}
					//================================================
					// Date format "YYYYWW" for a static week
					//================================================
					else if (oDate.Length == 6)
					{
						try
						{
							int fromTo = int.Parse(oDate);
							_overrideDateRange.StartDateKey = fromTo;
							_overrideDateRange.EndDateKey = fromTo;
							_overrideDateRange.DateRangeType = eCalendarRangeType.Static;
							_overrideDateRange.SelectedDateType = eCalendarDateType.Week;
							_overrideDateRange.RelativeTo = eDateRangeRelativeTo.None;

						}
						catch
						{
							errorFound = InvalidDateParm(oDate);
						}
					}
					else if (oDate.Length == 0)
					{
						_overrideDateRange.Key = Include.UndefinedCalendarDateRange; 
					}
					//============================================
					// Date format "-1" for a single dynamic week
					//============================================
					else
					{
						try
						{
							int fromTo = int.Parse(oDate);
							_overrideDateRange.StartDateKey = fromTo;
							_overrideDateRange.EndDateKey = fromTo;
							_overrideDateRange.DateRangeType = eCalendarRangeType.Dynamic;
							_overrideDateRange.SelectedDateType = eCalendarDateType.Week;
							_overrideDateRange.RelativeTo = eDateRangeRelativeTo.Current;

						}
						catch
						{
							errorFound = InvalidDateParm(oDate);
						}
					}

					return errorFound;
				}
				catch
				{
					throw;
				}
			}

			private bool InvalidDateParm(string aDate)
			{
				EventLog.WriteEntry(eventLogID, "Invalid Date Parm: " + aDate + ". Valid formating is \"YYYYWW-YYYYWW\", \"YYYYWW\", \"-1,-5\", or \"-1\" ", EventLogEntryType.Error);
				System.Console.Write("Invalid Date Parm: " + aDate + ". Valid formating is \"YYYYWW-YYYYWW\", \"YYYYWW\", \"-1,-5\", or \"-1\" ");

				return true;
			}

			/// <summary>
			/// Main processing method for the Size Day to Week Summary API
			/// </summary>
			/// <param name="concurrentProcesses"></param>
			/// <param name="drpRid"></param>
			/// <param name="overrideNodeRid"></param>
			/// <returns></returns>
			public bool ProcessSizeDayToWeekSummary(int concurrentProcesses,int drpRid, int overrideNodeRid)
			{
				//int styleRid = -1;
				//string varName = string.Empty;
				//Stack summaryStack = new Stack();
				MIDTimer processTimer = new MIDTimer();
				MerchandiseHierarchyData nodeData = new MerchandiseHierarchyData();
				bool errorFound = false;

				try
				{
					processTimer.Start();
					//MIDTimer oosTimer = new MIDTimer();
					//MIDTimer sellThruTimer = new MIDTimer();

					//===================================================
					// Get Out of Stock and Sell Thru Limit color nodes
					//===================================================
                    //oosTimer.Start();
                    //DataTable dtOOSColorNodes = _SAB.HierarchyServerSession.GetSizeOutOfStockColorNodes();
                    //oosTimer.Stop("GetSizeOutOfStockColorNodes()");
                    //string msg = "Number of Color Nodes with Out Of Stock Values: " + dtOOSColorNodes.Rows.Count + "   " + oosTimer.ElaspedTimeString;
                    //_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString(), true);
                    //LogMessage(msg);

                    //oosTimer.Start();
                    //DataTable dtSellThruColorNodes = _SAB.HierarchyServerSession.GetSizeSellThruLimitColorNodes();
                    //oosTimer.Stop("GetSizeSellThruLimitColorNodes()");
                    //msg = "Number of Color Nodes with Sell Thru Limits: " + dtSellThruColorNodes.Rows.Count + "   " + oosTimer.ElaspedTimeString;
                    //_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, this.ToString(), true);
                    //LogMessage(msg);

					//=======================================================================================
					// Get Main Hierarchy information 
					//=======================================================================================
                    //HierarchyProfile mainHier = _SAB.HierarchyServerSession.GetMainHierarchyData();
                    //BuildLevelProfiles(mainHier);

					//=========
					// Stores
					//=========
                    //int[] storeRIDs = new int[MIDStorageTypeInfo.GetStoreMaxRID(0)];
                    //ProfileList storeList = _SAB.StoreServerSession.GetAllStoresList();
                    //for (int i = 0; i < storeRIDs.Length; i++)
                    //{
                    //    storeRIDs[i] = i + 1;
                    //}
                    //LogMessage("Number of Stores: " + storeRIDs.Length);
                    //BuildStoreDictionaries(storeRIDs, storeList);

					//==========================
					// gather up weeklist from 
					//==========================
					ProfileList weekList = null;
					if (drpRid == Include.UndefinedCalendarDateRange)
					{
						weekList = new ProfileList(eProfileType.Week);
						WeekProfile currWeek = _SAB.ApplicationServerSession.Calendar.CurrentWeek;
						// Begin TT#763 - stodd - default should be current - 1
						WeekProfile currWeekMinusOne = _SAB.ApplicationServerSession.Calendar.Add(currWeek, -1);
						weekList.Add(currWeekMinusOne);
						// End TT#763 - stodd - default should be current - 1
					}
					else
					{
						DateRangeProfile drp = _SAB.ApplicationServerSession.Calendar.GetDateRange(drpRid);
						weekList = _SAB.ApplicationServerSession.Calendar.GetDateRangeWeeks(drp, null);
					}
					if (weekList.Count == 0)
					{
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, "No of weeks to process is Zero.", this.ToString());
					}
					// Begin TT#763 - stodd - default should be current - 1
					else
					{
						string msgDate = string.Empty;
						if (weekList.Count == 1)
						{
							msgDate = ((WeekProfile)weekList[0]).Text();
						}
						else
						{
							string startWeek = ((WeekProfile)weekList[0]).Text();
							string endWeek = ((WeekProfile)weekList[weekList.Count - 1]).Text();
							msgDate = startWeek + " - " + endWeek;
						}
						object [] parms = new object[1] {msgDate};
                        // Begin TT#3423 -JSmith - Ran the Day to Week Summary in the Post Conversion and receive an Error Invalid Object Name
                        //string msg1 = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_WeeksToProcess, parms);
                        string msg1 = _SAB.ApplicationServerSession.Audit.GetText(eMIDTextCode.msg_al_WeeksToProcess, false, parms);
                        // End TT#3423 -JSmith - Ran the Day to Week Summary in the Post Conversion and receive an Error Invalid Object Name
						//string msg = "The following weeks will be processed: " + startWeek + " - " + endWeek;
						_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg1, sourceModule, true);
					}
					// End TT#763 - stodd - default should be current - 1

					LogMessage("Number of Weeks: " + weekList.Count);
					foreach (WeekProfile wp in weekList.ArrayList)
					{
						LogMessage("  " + wp.Text());
					}

                    // Begin TT#3423 -JSmith - Ran the Day to Week Summary in the Post Conversion and receive an Error Invalid Object Name
                    ////=====================================================
                    //// Builds sql time id list that is used to get styles
                    ////=====================================================
                    //List<int> sqlTimeList = new List<int>();
                    //foreach (WeekProfile weekProf in weekList)
                    //{
                    //    foreach (DayProfile dayProf in weekProf.Days.ArrayList)
                    //    {
                    //        SQL_TimeID sqlTime = new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, dayProf.Key);
                    //        sqlTimeList.Add(sqlTime.SqlTimeID);
                    //    }
                    //}
                    // Begin TT#3423 -JSmith - Ran the Day to Week Summary in the Post Conversion and receive an Error Invalid Object Name
					//==============
					// Get Styles
					//==============
					//DataTable dtAllStyles = BuildAllStylesTable();
					//int totalStyles = 0;
                    //if (overrideNodeRid == Include.NoRID)
                    //{
                    //    //StoreVariableHistoryBin dlStoreVarHist = new StoreVariableHistoryBin();
                    //    //dtAllStyles = dlStoreVarHist.GetStylesForTimeRange(sqlTimeList);
                    //    //totalStyles = nodeData.GetAllStylesCount();
                    //}
                    //else
                    //{

                    //    HierarchyNodeProfile hnp = _SAB.HierarchyServerSession.GetNodeData(overrideNodeRid);
                    //    if (hnp.LevelType == eHierarchyLevelType.Style)
                    //    {
                    //        DataRow aRow = dtAllStyles.NewRow();
                    //        aRow[0] = overrideNodeRid;
                    //        dtAllStyles.Rows.Add(aRow);
                    //        totalStyles = 1;
                    //    }
                    //    else
                    //    {
                    //        NodeDescendantList nodeDescList = _SAB.HierarchyServerSession.GetNodeDescendantList(overrideNodeRid, eHierarchyLevelType.Style, eNodeSelectType.All);
                    //        totalStyles = nodeDescList.Count;
                    //        StoreVariableHistoryBin dlStoreVarHist = new StoreVariableHistoryBin();
                    //        dlStoreVarHist.GetStylesForTimeRange(sqlTimeList, nodeDescList, ref dtAllStyles);

                    //        //foreach (NodeDescendantProfile ndp in nodeDescList.ArrayList)
                    //        //{
                    //        //    object[] objs = new object[] { ndp.Key.ToString() };

                    //        //    dtAllStyles.LoadDataRow(objs, false);
                    //        //}
                    //        //dtAllStyles.AcceptChanges();
                    //    }
                    //}
                    //LogMessage("Number of Styles: " + dtAllStyles.Rows.Count);

					//===========
					// by STYLE
					//===========
					//_summaryProcessList = new ArrayList();
                    //foreach (DataRow row in dtAllStyles.Rows)
                    //{
                    //    _styleCnt++;
                    //    styleRid = int.Parse(row["HN_RID"].ToString());
                    //    if (_LOGGING)
                    //    {
                    //        _styleId = _SAB.HierarchyServerSession.GetNodeID(styleRid);
                    //        LogMessage("Style: " + _styleId + "(" + styleRid + ")");
                    //    }

                    //    //========================================================
                    //    // Build Summary Process classes to add to process stack
                    //    //========================================================
                    //    SizeDayToWeekSummaryProcess summaryProcess = new SizeDayToWeekSummaryProcess(_SAB, _SAB.ApplicationServerSession.Audit, styleRid, mainHier, _styleLevelProf, _colorLevelProf, weekList, storeRIDs, _storeRidDictionary, _storeIdDictionary, _LOGGING, dtOOSColorNodes, dtSellThruColorNodes);
                    //    // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
                    //    if (drpRid == Include.UndefinedCalendarDateRange)
                    //    {
                    //        summaryProcess.ProcessingCurrentDateInd = true;
                    //    }
                    //    // End TT#2257
                    //    summaryStack.Push(summaryProcess);
                    //    //_summaryProcessList.Add(summaryProcess);
                    //}


					//string cMsg = "Completed loading SizeDayToWeekSummary process stack. Preparing to process " + dtAllStyles.Rows.Count + " styles out of a total of " + totalStyles + ".";
					//Debug.WriteLine(cMsg);
					//System.Console.Write(cMsg + System.Environment.NewLine);
					//_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, cMsg, this.ToString(), true);

					//dtAllStyles = null;

					//SummarizeStyles(ref errorFound, concurrentProcesses, summaryStack);


                    //Execute the Size Curve Summary Process via the stored procedure and obtain a summary of the execution times and rows affected
                    // Begin TT#3423 -JSmith - Ran the Day to Week Summary in the Post Conversion and receive an Error Invalid Object Name
                    //int startSQLTimeID;
                    //int endSQLTimeID;

                    //startSQLTimeID = sqlTimeList.Min();
                    //endSQLTimeID = sqlTimeList.Max();
                    // End TT#3423 -JSmith - Ran the Day to Week Summary in the Post Conversion and receive an Error Invalid Object Name

                    if (overrideNodeRid == Include.NoRID)
                    {
                        // Begin TT#3423 -JSmith - Ran the Day to Week Summary in the Post Conversion and receive an Error Invalid Object Name
                        //overrideNodeRid = 101; //all nodes
                        HierarchyNodeList nodeList = _SAB.HierarchyServerSession.GetRootNodes(eHierarchySelectType.OrganizationalHierarchyRoot);
                        if (nodeList.Count > 0)
                        {
                            overrideNodeRid = ((HierarchyNodeProfile)nodeList[0]).Key;
                        }
                        // End TT#3423 -JSmith - Ran the Day to Week Summary in the Post Conversion and receive an Error Invalid Object Name
                    }
                    // Begin TT#3423 -JSmith - Ran the Day to Week Summary in the Post Conversion and receive an Error Invalid Object Name
                    //DataTable dtSummaryForLog = _SAB.HierarchyServerSession.ExecuteSizeDayToWeekSummary(overrideNodeRid, startSQLTimeID, endSQLTimeID);

                    ////put the summary messages into the file log
                    //foreach (DataRow dr in dtSummaryForLog.Rows)
                    //{
                    //    string srow = string.Empty;
                    //    foreach (DataColumn dc in dtSummaryForLog.Columns)
                    //    {
                    //        if (dr[dc] != DBNull.Value)
                    //        {
                    //            srow += "\t" + dr[dc];
                    //        }
                    //    }
                    //    LogMessage(srow);
                    //}
                    DataTable dtSummaryForLog = null;
                    foreach (WeekProfile weekProf in weekList)
                    {
                        LogMessage("Processing " + weekProf);
                        dtSummaryForLog = _SAB.HierarchyServerSession.ExecuteSizeDayToWeekSummary(overrideNodeRid, weekProf.Days[0].Key, weekProf.Days[weekProf.DaysInWeek - 1].Key);

                        //put the summary messages into the file log
                        foreach (DataRow dr in dtSummaryForLog.Rows)
                        {
                            string srow = string.Empty;
                            foreach (DataColumn dc in dtSummaryForLog.Columns)
                            {
                                if (dr[dc] != DBNull.Value)
                                {
                                    srow += "\t" + dr[dc];
                                }
                            }
                            LogMessage(srow);
                        }
                        dtSummaryForLog = null;
                    }
                    // End TT#3423 -JSmith - Ran the Day to Week Summary in the Post Conversion and receive an Error Invalid Object Name

					processTimer.Stop("Read Data");
					//Debug.WriteLine("Styles: " + _styleCnt + " Colors: " + _colorCnt + " Sizes: " + _sizeCnt + " Total Reads: " + _totReads + " Total Updates: " + _totUpdates + " Value Cnt: " + _valCnt);
					LogMessage("Processing Time: " + processTimer.ElaspedTimeString);
					//LogMessage("Styles: " + _styleCnt + " Colors: " + _colorCnt + " Sizes: " + _sizeCnt + " Total Daily Store Reads: " + _totReads + " Values Read: " + _valCnt + " Total Weekly Store Records Updated: " + _totUpdates);
				}
				catch (Exception ex)
				{
					LogMessage(ex.ToString());
					_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.ToString(), this.ToString());
					throw;
				}
				catch 
				{
					throw;
				}
				return errorFound;
			}

            //private void BuildLevelProfiles(HierarchyProfile mainHier)
            //{
            //    try
            //    {
            //        for (int levelIndex = 1; levelIndex <= mainHier.HierarchyLevels.Count; levelIndex++)
            //        {
            //            HierarchyLevelProfile hlp = (HierarchyLevelProfile)mainHier.HierarchyLevels[levelIndex];
            //            //hlp.LevelID is level name 
            //            //hlp.Level is level number 
            //            //hlp.LevelType is level type 
            //            if (hlp.LevelType == eHierarchyLevelType.Style)
            //            {
            //                _styleLevelProf = hlp;
            //            }
            //            if (hlp.LevelType == eHierarchyLevelType.Color)
            //            {
            //                _colorLevelProf = hlp;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        LogMessage(ex.ToString());
            //        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.ToString(), this.ToString());
            //        throw;
            //    }
            //}

            //private void BuildStoreDictionaries(int[] storeRIDs, ProfileList storeList)
            //{
            //    try
            //    {
            //        _storeRidDictionary = new Dictionary<int,int>();
            //        _storeIdDictionary = new Dictionary<int,string>();

            //        for (int i = 0; i < storeRIDs.Length; i++)
            //        {
            //            StoreProfile sp = (StoreProfile)storeList.FindKey(i + 1);
            //            // Begin TT#646 - stodd - inactive stores causing viewer to abend.
            //            //_storeIdDictionary.Add(i, sp.StoreId);
            //            // This catches inactive stores that have not been retured from the store service.
            //            if (sp != null)
            //            {
            //                _storeIdDictionary.Add(i, sp.StoreId);
            //            }
            //            else
            //            {
            //                _storeIdDictionary.Add(i, "Inactive");
            //            }
            //            // End TT#646 - stodd - inactive stores causing viewer to abend.
            //            _storeRidDictionary.Add(i, storeRIDs[i]);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        LogMessage(ex.ToString());
            //        _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Error, ex.ToString(), this.ToString());
            //        throw;
            //    }
            //}

            ///// <summary>
            ///// Processes the summaryStack thats full of Styles to be processed.
            ///// </summary>
            ///// <param name="errorFound"></param>
            ///// <param name="aConcurrentProcesses"></param>
            ///// <param name="summaryStack"></param>
            ///// <returns></returns>
            //public bool SummarizeStyles(ref bool errorFound, int aConcurrentProcesses, Stack summaryStack)
            //{
            //    // use ArrayList to maintain reference until errors are counted
            //    int _totalErrors = 0;

            //    try
            //    {
            //        if (aConcurrentProcesses > 1)
            //        {
            //            SizeDayToWeekSummaryProcessManager cpm = new SizeDayToWeekSummaryProcessManager(_SAB.ClientServerSession.Audit, summaryStack, aConcurrentProcesses, 50);
            //            cpm.ProcessCommands(false);

            //            _styleCnt = cpm.SytleCnt;
            //            _sizeCnt = cpm.SizeCnt;
            //            _colorCnt = cpm.ColorCnt;
            //            _valCnt = cpm.ValCnt;
            //            _totReads = cpm.TotReads;
            //            _totUpdates = cpm.TotUpdates;
            //            _totalErrors = cpm.ErrorCnt;
            //        }
            //        else
            //        {
            //            int styleCnt = 0;
            //            while (summaryStack.Count > 0)
            //            {
            //                SizeDayToWeekSummaryProcess sp = (SizeDayToWeekSummaryProcess)summaryStack.Pop();
            //                sp.ExecuteProcess();
            //                styleCnt++;
            //                _sizeCnt += sp.SizeCnt;
            //                _colorCnt += sp.ColorCnt;
            //                _valCnt += sp.ValCnt;
            //                _totReads += sp.TotReads;
            //                _totUpdates += sp.TotUpdates;
            //                _totalErrors += sp.NumberOfErrors;
            //                //Debug.WriteLine(_styleId + " colors: " + sp.ColorCnt + "sizes: " + sp.SizeCnt);

            //                if (_LOGGING)
            //                {
            //                    LogMessage(" ");
            //                    LogMessage("====================================================");
            //                    LogMessage("====================================================");
            //                    LogMessage("====  STYLE: " + sp.StyleId);
            //                    LogMessage("====================================================");
            //                    LogMessage("====================================================");
            //                    foreach (string msg in sp.LogMessages)
            //                    {
            //                        LogMessage(msg);
            //                    }
            //                }
            //                sp = null;
            //                //GC.Collect();
            //            }
            //            _styleCnt = styleCnt;
            //        }

            //        //foreach (SizeDayToWeekSummaryProcess sp in _summaryProcessList)
            //        //{
            //        //    _sizeCnt += sp.SizeCnt;
            //        //    _colorCnt += sp.ColorCnt;
            //        //    _valCnt += sp.ValCnt;
            //        //    _totReads += sp.TotReads;
            //        //    _totUpdates += sp.TotUpdates;
            //        //    _totalErrors += sp.NumberOfErrors;

            //        //    if (_LOGGING)
            //        //    {
            //        //        LogMessage(" ");
            //        //        LogMessage("====================================================");
            //        //        LogMessage("====================================================");
            //        //        LogMessage("====  STYLE: " + sp.StyleId);
            //        //        LogMessage("====================================================");
            //        //        LogMessage("====================================================");
            //        //        foreach (string msg in sp.LogMessages)
            //        //        {
            //        //            LogMessage(msg);
            //        //        }
            //        //    }
            //        //}
            //    }

            //    catch (Exception exception)
            //    {
            //        errorFound = true;
            //        _SAB.ClientServerSession.Audit.Add_Msg(eMIDMessageLevel.Severe, exception.Message, sourceModule);

            //        throw;
            //    }
            //    finally
            //    {
            //        _SAB.ClientServerSession.Audit.SizeDayToWeekSummaryAuditInfo_Add(_styleCnt, _colorCnt,
            //            _sizeCnt, _valCnt, _totReads, _totUpdates, _totalErrors);
            //    }
            //    return false;
            //}

            //private DataTable BuildAllStylesTable()
            //{
            //    DataTable dtAllStyles = MIDEnvironment.CreateDataTable();

            //    DataColumn dataColumn = new DataColumn();
            //    dataColumn.DataType = System.Type.GetType("System.Int32");
            //    dataColumn.ColumnName = "HN_RID";
            //    dtAllStyles.Columns.Add(dataColumn);

            //    return dtAllStyles;
            //}

			private void LogMessage(string msg)
			{
				if (_LOGGING)
				{
					if (_log != null)
					{
						_log.WriteLine(msg);
					}
				}
			}
		}
	}

    //public class SizePreviousWeek 
    //{
    //    private int _styleRid;
    //    private int _colorCodeRid;
    //    private int _sizeCodeRid;
    //    private SQL_TimeID _timeID;
    //    private SQL_TimeID _previousTimeID;
    //    private int[] _storeRids;
    //    private double[] _accumSellThruSales;
    //    private double[] _accumSellThruStock;
    //    private double[] _receivedStock;
    //    StoreVariableHistoryBin _dlStoreVarHist;

    //    public int SizeCodeRid
    //    {
    //        get
    //        {
    //            return _sizeCodeRid;
    //        }
    //    }

    //    public SizePreviousWeek(int styleRid, int colorCodeRid, int sizeCodeRid, int[] storeRids, SQL_TimeID timeID)
    //    {
    //        _styleRid = styleRid;
    //        _colorCodeRid = colorCodeRid;
    //        _sizeCodeRid = sizeCodeRid;
    //        _timeID = timeID;
    //        short sqlTimeId = ((short)(timeID.SqlTimeID - 7));
    //        _previousTimeID = new SQL_TimeID('W', sqlTimeId);
    //        _storeRids = storeRids;
    //        _accumSellThruSales = null;
    //        _accumSellThruStock = null;
    //        _receivedStock = null;
    //        _dlStoreVarHist = new StoreVariableHistoryBin();
    //    }


    //    public int GetVariableValue(int store, string varName, eForecastBaseDatabaseStoreVariables varType)
    //    {
    //        int rValue = 0;

    //        try
    //        {
    //            if (varType == eForecastBaseDatabaseStoreVariables.ReceivedStock)
    //            {
    //                if (_receivedStock == null)
    //                {
    //                    _receivedStock = _dlStoreVarHist.GetStoreVariableWeekValue(varName, _styleRid, _previousTimeID, _colorCodeRid, _sizeCodeRid, _storeRids);
    //                }
    //                rValue = Convert.ToInt32(_receivedStock[store]);

    //            }
    //            else if (varType == eForecastBaseDatabaseStoreVariables.AccumSellThruSales)
    //            {
    //                if (_accumSellThruSales == null)
    //                {
    //                    _accumSellThruSales = _dlStoreVarHist.GetStoreVariableWeekValue(varName, _styleRid, _previousTimeID, _colorCodeRid, _sizeCodeRid, _storeRids);
    //                }
    //                rValue = Convert.ToInt32(_accumSellThruSales[store]);
    //            }
    //            else if (varType == eForecastBaseDatabaseStoreVariables.AccumSellThruStock)
    //            {
    //                if (_accumSellThruStock == null)
    //                {
    //                    _accumSellThruStock = _dlStoreVarHist.GetStoreVariableWeekValue(varName, _styleRid, _previousTimeID, _colorCodeRid, _sizeCodeRid, _storeRids);
    //                }
    //                rValue = Convert.ToInt32(_accumSellThruStock[store]);
    //            }
    //        }
    //        catch
    //        {
    //            throw;
    //        }

    //        return rValue;
    //    }

    //    ///// <summary>
    //    ///// Get whether or not this size has ever gotten sock in the past. 0 = false.
    //    ///// </summary>
    //    ///// <param name="store"></param>
    //    ///// <returns></returns>
    //    //public bool GetReceivedStock(int store)
    //    //{
    //    //    if (_receivedStock == null)
    //    //    {
    //    //        string varName = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.ReceivedStock);
    //    //        _receivedStock = _dlStoreVarHist.GetStoreVariableWeekValue(varName, _styleRid, _previousTimeID, _colorCodeRid, _sizeCodeRid, _storeRids);
    //    //    }
    //    //    if (_receivedStock[store] == 0)
    //    //        return false;
    //    //    else
    //    //        return true;
    //    //}

    //    //public int GetAccumSales(int store)
    //    //{
    //    //    if (_accumSellThruSales == null)
    //    //    {
    //    //        string varName = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.AccumSellThruSales);
    //    //        _accumSellThruSales = _dlStoreVarHist.GetStoreVariableWeekValue(varName, _styleRid, _previousTimeID, _colorCodeRid, _sizeCodeRid, _storeRids);
    //    //    }
    //    //    return Convert.ToInt32(_accumSellThruSales[store]);
    //    //}

    //    //public int GetAccumStock(int store)
    //    //{
    //    //    if (_accumSellThruStock == null)
    //    //    {
    //    //        string varName = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.AccumSellThruStock);
    //    //        _accumSellThruStock = _dlStoreVarHist.GetStoreVariableWeekValue(varName, _styleRid, _previousTimeID, _colorCodeRid, _sizeCodeRid, _storeRids);
    //    //    }
    //    //    return Convert.ToInt32(_accumSellThruStock[store]);
    //    //}
    //}

	// begin TT#391 - stodd - size day to week summary
	/// <summary>
	/// For a Size Code Rid, contains a collection of lists representing specific variable values by time. 
	/// </summary>
    //public class SizeTimeCollection : IComparable
    //{
    //    private int _sizeCodeRID;
    //    private int _sizeRID;

    //    private Dictionary<int, int> _salesByTimeList;
    //    private Dictionary<int, int> _salesRegByTimeList;
    //    private Dictionary<int, int> _salesPromoByTimeList;
    //    private Dictionary<int, int> _salesMkdnByTimeList;
    //    // Equal to SALES if not = to 0, else equal to SALES_REG + SALES_MKDN + SALES_PROMO
    //    private Dictionary<int, int> _salesTotalByTimeList;

    //    private Dictionary<int, int> _stockByTimeList;
    //    private Dictionary<int, int> _stockRegByTimeList;
    //    private Dictionary<int, int> _stockMkdnByTimeList;
    //    // Equal to STOCK if not = to 0, else equal to STOCK_REG + STOCK_MKDN
    //    private Dictionary<int, int> _stockTotalByTimeList;

    //    private bool _allZeroes = true;

    //    public int SizeCodeRID
    //    {
    //        get
    //        {
    //            return _sizeCodeRID;
    //        }
    //    }

    //    public int SizeRID
    //    {
    //        get
    //        {
    //            return _sizeRID;
    //        }
    //        set
    //        {
    //            value = _sizeRID;
    //        }
    //    }

    //    public bool AllZeroes
    //    {
    //        get
    //        {
    //            return _allZeroes;
    //        }
    //    }

    //    public Dictionary<int, int> SalesByTimeList
    //    {
    //        get
    //        {
    //            return _salesByTimeList;
    //        }
    //    }
    //    public Dictionary<int, int> SalesRegByTimeList
    //    {
    //        get
    //        {
    //            return _salesRegByTimeList;
    //        }
    //    }
    //    public Dictionary<int, int> SalesPromoByTimeList
    //    {
    //        get
    //        {
    //            return _salesPromoByTimeList;
    //        }
    //    }
    //    public Dictionary<int, int> SalesMkdnByTimeList
    //    {
    //        get
    //        {
    //            return _salesMkdnByTimeList;
    //        }
    //    }
    //    public Dictionary<int, int> SalesTotalByTimeList
    //    {
    //        get
    //        {
    //            return _salesTotalByTimeList;
    //        }
    //    }

    //    public Dictionary<int, int> StockByTimeList
    //    {
    //        get
    //        {
    //            return _stockByTimeList;
    //        }
    //    }
    //    public Dictionary<int, int> StockRegByTimeList
    //    {
    //        get
    //        {
    //            return _stockRegByTimeList;
    //        }
    //    }
    //    public Dictionary<int, int> StockMkdnByTimeList
    //    {
    //        get
    //        {
    //            return _stockMkdnByTimeList;
    //        }
    //    }
    //    public Dictionary<int, int> StockTotalByTimeList
    //    {
    //        get
    //        {
    //            return _stockTotalByTimeList;
    //        }
    //    }

    //    public SizeTimeCollection(int aSizeCodeRID)
    //    {
    //        _sizeCodeRID = aSizeCodeRID;
    //        _salesByTimeList = new Dictionary<int, int>();
    //        _salesRegByTimeList = new Dictionary<int, int>();
    //        _salesPromoByTimeList = new Dictionary<int, int>();
    //        _salesMkdnByTimeList = new Dictionary<int, int>();
    //        _salesTotalByTimeList = new Dictionary<int, int>();
    //        _stockByTimeList = new Dictionary<int, int>();
    //        _stockRegByTimeList = new Dictionary<int, int>();
    //        _stockMkdnByTimeList = new Dictionary<int, int>();
    //        _stockTotalByTimeList = new Dictionary<int, int>();
    //    }

    //    /// <summary>
    //    /// Uses the filled collections to accum and build the sales total and stock total collections.
    //    /// </summary>
    //    public void AccumTotals(ArrayList timeKeyList)
    //    {
    //        //===========
    //        // Sales
    //        //===========
    //        foreach (SQL_TimeID aSQLTime in timeKeyList)
    //        {
    //            int timeId = aSQLTime.SqlTimeID;
    //            if (_salesByTimeList.Count > 0)
    //            {
    //                int sValue = GetSalesValue(timeId);
    //                _salesTotalByTimeList.Add(timeId, sValue);
    //                if (sValue != 0)
    //                    _allZeroes = false;
    //            }
    //            else
    //            {
    //                int regVal = GetSalesRegValue(timeId);
    //                int mkdnVal = GetSalesMkdnValue(timeId);
    //                int promoVal = GetSalesPromoValue(timeId);
    //                _salesTotalByTimeList.Add(timeId, regVal + mkdnVal + promoVal);
    //                if (regVal != 0 || mkdnVal != 0 || promoVal != 0)
    //                    _allZeroes = false;
    //            }
    //            //===========
    //            // Stock
    //            //===========
    //            if (_stockByTimeList.Count > 0)
    //            {
    //                int sValue = GetStockValue(timeId);
    //                _stockTotalByTimeList.Add(timeId, sValue);
    //                if (sValue != 0)
    //                    _allZeroes = false;
    //            }
    //            else
    //            {
    //                int regVal = GetStockRegValue(timeId);
    //                int mkdnVal = GetStockMkdnValue(timeId);
    //                _stockTotalByTimeList.Add(timeId, regVal + mkdnVal);
    //                if (regVal != 0 || mkdnVal != 0)
    //                    _allZeroes = false;
    //            }
    //        }
    //    }

    //    public void SetSalesValue(int time, int value)
    //    {
    //        _salesByTimeList.Add(time, value);
    //    }
    //    public void SetSalesRegValue(int time, int value)
    //    {
    //        _salesRegByTimeList.Add(time, value);
    //    }
    //    public void SetSalesMkdnValue(int time, int value)
    //    {
    //        _salesMkdnByTimeList.Add(time, value);
    //    }
    //    public void SetSalesPromoValue(int time, int value)
    //    {
    //        _salesPromoByTimeList.Add(time, value);
    //    }
    //    public void SetStockValue(int time, int value)
    //    {
    //        _stockByTimeList.Add(time, value);
    //    }
    //    public void SetStockRegValue(int time, int value)
    //    {
    //        _stockRegByTimeList.Add(time, value);
    //    }
    //    public void SetStockMkdnValue(int time, int value)
    //    {
    //        _stockMkdnByTimeList.Add(time, value);
    //    }

    //    public int GetSalesValue(int time)
    //    {
    //        try
    //        {
    //            if (_salesByTimeList.ContainsKey(time))
    //                return (int)_salesByTimeList[time];
    //            else
    //                return 0;
    //        }
    //        catch (Exception ex)
    //        {
    //            string i = ex.ToString();
    //            throw;
    //        }
    //    }
    //    public int GetSalesRegValue(int time)
    //    {
    //        try
    //        {
    //            if (_salesRegByTimeList.ContainsKey(time))
    //                return (int)_salesRegByTimeList[time];
    //            else
    //                return 0;
    //        }
    //        catch (Exception ex)
    //        {
    //            string i = ex.ToString();
    //            throw;
    //        }
    //    }
    //    public int GetSalesMkdnValue(int time)
    //    {
    //        try
    //        {
    //            if (_salesMkdnByTimeList.ContainsKey(time))
    //                return (int)_salesMkdnByTimeList[time];
    //            else
    //                return 0;
    //        }
    //        catch (Exception ex)
    //        {
    //            string i = ex.ToString();
    //            throw;
    //        }
    //    }
    //    public int GetSalesPromoValue(int time)
    //    {
    //        try
    //        {
    //            if (_salesPromoByTimeList.ContainsKey(time))
    //                return (int)_salesPromoByTimeList[time];
    //            else
    //                return 0;
    //        }
    //        catch (Exception ex)
    //        {
    //            string i = ex.ToString();
    //            throw;
    //        }
    //    }
    //    public int GetSalesTotalValue(int time)
    //    {
    //        try
    //        {
    //            if (_salesTotalByTimeList.ContainsKey(time))
    //                return (int)_salesTotalByTimeList[time];
    //            else
    //                return 0;
    //        }
    //        catch (Exception ex)
    //        {
    //            string i = ex.ToString();
    //            throw;
    //        }
    //    }

    //    public int GetStockValue(int time)
    //    {
    //        try
    //        {
    //            if (_stockByTimeList.ContainsKey(time))
    //                return (int)_stockByTimeList[time];
    //            else
    //                return 0;
    //        }
    //        catch (Exception ex)
    //        {
    //            string i = ex.ToString();
    //            throw;
    //        }
    //    }
    //    public int GetStockRegValue(int time)
    //    {
    //        try
    //        {
    //            if (_stockRegByTimeList.ContainsKey(time))
    //                return (int)_stockRegByTimeList[time];
    //            else
    //                return 0;
    //        }
    //        catch (Exception ex)
    //        {
    //            string i = ex.ToString();
    //            throw;
    //        }
    //    }
    //    public int GetStockMkdnValue(int time)
    //    {
    //        try
    //        {
    //            if (_stockMkdnByTimeList.ContainsKey(time))
    //                return (int)_stockMkdnByTimeList[time];
    //            else
    //                return 0;
    //        }
    //        catch (Exception ex)
    //        {
    //            string i = ex.ToString();
    //            throw;
    //        }
    //    }
    //    public int GetStockTotalValue(int time)
    //    {
    //        try
    //        {
    //            if (_stockTotalByTimeList.ContainsKey(time))
    //                return (int)_stockTotalByTimeList[time];
    //            else
    //                return 0;
    //        }
    //        catch (Exception ex)
    //        {
    //            string i = ex.ToString();
    //            throw;
    //        }
    //    }

    //    public void Clear()
    //    {
    //        _salesByTimeList.Clear();
    //        _salesRegByTimeList.Clear();
    //        _salesPromoByTimeList.Clear();
    //        _salesMkdnByTimeList.Clear();
    //        _salesTotalByTimeList.Clear();
    //        _stockByTimeList.Clear();
    //        _stockRegByTimeList.Clear(); 
    //        _stockMkdnByTimeList.Clear(); 
    //        _stockTotalByTimeList.Clear();

    //        _salesByTimeList = null;
    //        _salesRegByTimeList = null;
    //        _salesPromoByTimeList = null;
    //        _salesMkdnByTimeList = null;
    //        _salesTotalByTimeList = null;
    //        _stockByTimeList = null;
    //        _stockRegByTimeList = null;
    //        _stockMkdnByTimeList = null;
    //        _stockTotalByTimeList = null;
    //    }

    //    #region IComparable Members

    //    public int CompareTo(object obj)
    //    {
    //        SizeTimeCollection entry;

    //        try
    //        {
    //            if (obj.GetType() != typeof(StoreSizeValue))
    //            {
    //                throw new Exception("Invalid comparison to SizeTimeCollection object");
    //            }

    //            entry = (SizeTimeCollection)obj;

    //            if (_sizeCodeRID < entry.SizeCodeRID)
    //            {
    //                return -1;
    //            }
    //            else if (_sizeCodeRID > entry.SizeCodeRID)
    //            {
    //                return 1;
    //            }
    //            else
    //            {
    //                return 0;
    //            }
				
    //        }
    //        catch (Exception exc)
    //        {
    //            string message = exc.ToString();
    //            throw;
    //        }
    //    }

    //    #endregion
    //}

	/// <summary>
	/// Contains a collection of store values for a specific Size Code Rid and Variable Name.
	/// </summary>
    //public class SizeVarStoreCollection
    //{
    //    private int _sizeCodeRID;
    //    private string _variableName;
    //    private List<double> _valuesByStoreList;

    //    public int SizeCodeRID
    //    {
    //        get
    //        {
    //            return _sizeCodeRID;
    //        }
    //    }

    //    public string VariableName
    //    {
    //        get
    //        {
    //            return _variableName;
    //        }
    //    }

    //    public List<double> ValuesByStoreList
    //    {
    //        get
    //        {
    //            return _valuesByStoreList;
    //        }
    //    }

    //    public SizeVarStoreCollection(int aSizeCodeRID, string aVarName)
    //    {
    //        _sizeCodeRID = aSizeCodeRID;
    //        _variableName = aVarName;
    //        _valuesByStoreList = new List<double>();
    //    }

    //    public void AddValue(double value)
    //    {
    //        _valuesByStoreList.Add(value);
    //    }
    //}

	// Begin TT#522 - stodd - memory issues

	/// <summary>
	/// Contains a collection of store values for a specific Size Code Rid and Variable Name.
	/// </summary>
    //public class SizeDayToWeekSummaryProcessManager : ConcurrentProcessManager
    //{
    //    private ArrayList _lockControl;
    //    private int _sytleCnt = 0;
    //    private int _colorCnt = 0;
    //    private int _sizeCnt = 0;
    //    private int _totReads = 0;
    //    private int _totUpdates = 0;
    //    private int _valCnt = 0;
    //    private int _errorCnt = 0;

    //    public int SytleCnt
    //    {
    //        get { return _sytleCnt; }
    //    }
    //    public int ColorCnt
    //    {
    //        get { return _colorCnt; }
    //    }
    //    public int SizeCnt
    //    {
    //        get { return _sizeCnt; }
    //    }
    //    public int TotReads
    //    {
    //        get { return _totReads; }
    //    }
    //    public int TotUpdates
    //    {
    //        get { return _totUpdates; }
    //    }
    //    public int ValCnt
    //    {
    //        get { return _valCnt; }
    //    }
    //    public int ErrorCnt
    //    {
    //        get { return _errorCnt; }
    //    }
		

    //    public SizeDayToWeekSummaryProcessManager(Audit aAudit, Stack aCommandStack, int aConcurrentProcesses, int aIntervalWaitTime)
    //        : base(aAudit, aCommandStack, aConcurrentProcesses, aIntervalWaitTime)
    //    {
    //        _lockControl = new ArrayList();
    //        ConcurrentProcess[] commandStackArray = new ConcurrentProcess[aCommandStack.Count];
    //        aCommandStack.CopyTo(commandStackArray, 0);
    //        foreach (SizeDayToWeekSummaryProcess cp in commandStackArray)
    //        {
    //            cp.OnSizeDayToWeekSummaryProcessEndHandler += new SizeDayToWeekSummaryProcess.SizeDayToWeekSummaryProcessEndEventHandler(SizeDayToWeekSummaryProcess_ProcessInfo);
    //        }
    //        commandStackArray = null;
    //    }


    //    private void SizeDayToWeekSummaryProcess_ProcessInfo(object source, SizeDayToWeekSummaryProcessEndEventArgs e)
    //    {
    //        SetProcessInfo(e.SizeDayToWeekSummaryProcess);
    //    }

    //    override public void SetProcessInfo(ConcurrentProcess aProcess)
    //    {
    //        try
    //        {
    //            lock (_lockControl.SyncRoot)
    //            {
    //                SizeDayToWeekSummaryProcess sProcess = (SizeDayToWeekSummaryProcess)aProcess;
    //                _sytleCnt++;
    //                _sizeCnt += sProcess.SizeCnt;
    //                _colorCnt += sProcess.ColorCnt;
    //                _valCnt += sProcess.ValCnt;
    //                _totReads += sProcess.TotReads;
    //                _totUpdates += sProcess.TotUpdates;
    //                _errorCnt += sProcess.NumberOfErrors;
    //            }
    //        }
    //        catch
    //        {
    //            throw;
    //        }
    //    }
    //}
	// end TT#522 - stodd - memory issues

    //public class SizeDayToWeekSummaryProcess : ConcurrentProcess
    //{
    //    //=======
    //    // FIELDS
    //    //=======
    //    private SessionAddressBlock _SAB;
    //    private bool _LOGGING;
    //    private int _styleRid;
    //    private string _styleId;
    //    private HierarchyProfile _mainHier;
    //    private int _colorCodeRid;
    //    private int _colorRid;
    //    private string _colorCodeId;
    //    private ProfileList _weekList;
    //    private ArrayList _timeKeyList;
    //    private int _sizeCodeRid;
    //    private Hashtable _sizeCodeHash;
    //    private Hashtable _sizeCodeProfileHash;
    //    private ArrayList _varKeyList;
    //    private string _varName;
    //    private eForecastBaseDatabaseStoreVariables _varEnum;
    //    private int _timeId;
    //    private StoreVariableHistoryBin _dlStoreVarHist;
    //    private int[] _storeRIDs;
    //    private Dictionary<int, int> _storeRidDictionary;
    //    private Dictionary<int, string> _storeIdDictionary;
    //    //private Hashtable _storeIdHash;
    //    //private Hashtable _storeRidHash;
    //    private HierarchyLevelProfile _styleLevelProf;
    //    private HierarchyLevelProfile _colorLevelProf;
    //    List<string> _logMessages;
    //    private Dictionary<int, SizeSellThruProfile> _sellThruLimitDictionary;
    //    private Dictionary<int, SizeOutOfStockProfile> _OutOfStockDictionary;
    //    private DataTable _dtOOSColorNodes;
    //    private DataTable _dtSellThruColorNodes;
    //    // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //    private bool _processingCurrentDateInd = false;
    //    // End TT#2257
		
    //    private int _colorCnt = 0;
    //    private int _sizeCnt = 0;
    //    private int _totReads = 0;
    //    private int _totUpdates = 0;
    //    private int _valCnt = 0;

    //    private string SALES_VAR_NAME = string.Empty;
    //    private string SALES_REG_VAR_NAME = string.Empty;
    //    private string SALES_PROMO_VAR_NAME = string.Empty;
    //    private string SALES_MKDN_VAR_NAME = string.Empty;
    //    private string STOCK_VAR_NAME = string.Empty;
    //    private string STOCK_REG_VAR_NAME = string.Empty;
    //    private string STOCK_MKDN_VAR_NAME = string.Empty;
    //    private string IN_STOCK_SALES_VAR_NAME = string.Empty;
    //    private string IN_STOCK_SALES_REG_VAR_NAME = string.Empty;
    //    private string IN_STOCK_SALES_PROMO_VAR_NAME = string.Empty;
    //    private string IN_STOCK_SALES_MKDN_VAR_NAME = string.Empty;
    //    private string ACCUM_SELL_THRU_SALES_VAR_NAME = string.Empty;
    //    private string ACCUM_SELL_THRU_STOCK_VAR_NAME = string.Empty;
    //    private string DAYS_IN_STOCK_VAR_NAME = string.Empty;
    //    private string RECEIVED_STOCK_VAR_NAME = string.Empty;

    //    private double[] _dSales = null;
    //    private double[] _dSalesReg = null;
    //    private double[] _dSalesPromo = null;
    //    private double[] _dSalesMkdn = null;
    //    private double[] _dStock = null;
    //    private double[] _dStockReg = null;
    //    private double[] _dStockMkdn = null;
    //    private double[] _dInStockSales = null;
    //    private double[] _dInStockSalesReg = null;
    //    private double[] _dInStockSalesPromo = null;
    //    private double[] _dInStockSalesMkdn = null;
    //    private double[] _dAccumSellThruSales = null;
    //    private double[] _dAccumSellThruStock = null;
    //    private double[] _dDaysInStock = null;
    //    private double[] _dReceivedStock = null;

    //    //================================================================
    //    // This is so the insert and update of the DT row values can be
    //    // done by column, which is faster than using column names.
    //    //================================================================
    //    private DataColumn	_dcStoreIdx;
    //    private DataColumn	_dcSizeCodeRid;
    //    private DataColumn	_dcSales;
    //    private DataColumn	_dcSalesReg;
    //    private DataColumn	_dcSalesPromo;
    //    private DataColumn	_dcSalesMkdn;
    //    private DataColumn	_dcStock;
    //    private DataColumn	_dcStockReg;
    //    private DataColumn	_dcStockMkdn;
    //    private DataColumn	_dcReceivedStock;

    //    private DataColumn	_dcInStockSales;
    //    private DataColumn	_dcInStockSalesReg;
    //    private DataColumn	_dcInStockSalesPromo;
    //    private DataColumn	_dcInStockSalesMkdn;
    //    private DataColumn	_dcAccumSellThruSales;
    //    private DataColumn	_dcAccumSellThruStock;
    //    private DataColumn	_dcDaysInStock;


    //    public delegate void SizeDayToWeekSummaryProcessEndEventHandler(object source, SizeDayToWeekSummaryProcessEndEventArgs e);
    //    public event SizeDayToWeekSummaryProcessEndEventHandler OnSizeDayToWeekSummaryProcessEndHandler;

    //    internal struct VarNameStruct
    //    {
    //        private string _name;
    //        private eForecastBaseDatabaseStoreVariables _enum;

    //        public string Name
    //        {
    //            get { return _name; }
    //            set { _name = value; }
    //        }
    //        public eForecastBaseDatabaseStoreVariables Enum
    //        {
    //            get { return _enum; }
    //            set { _enum = value; }
    //        }

    //        public VarNameStruct(string name, eForecastBaseDatabaseStoreVariables aEnum)
    //        {
    //            _name = name;
    //            _enum = aEnum;
    //        }
    //    }

    //    //=============
    //    // CONSTRUCTORS
    //    //=============

    //    public SizeDayToWeekSummaryProcess(SessionAddressBlock aSAB, Audit aAudit, int styleRid, HierarchyProfile mainHier, HierarchyLevelProfile styleLevelProf, 
    //        HierarchyLevelProfile colorLevelProf, ProfileList weekList, int[] storeRIDs, Dictionary<int, int> storeRidDictionary, Dictionary<int, string> storeIdDictionary,
    //        bool logging, DataTable dtOOSColorNodes, DataTable dtSellThruColorNodes)
    //        : base(aAudit)
    //    {
    //        try
    //        {
    //            _SAB = aSAB;
    //            _styleRid = styleRid;
    //            _mainHier = mainHier;
    //            _styleLevelProf = styleLevelProf;
    //            _colorLevelProf = colorLevelProf;
    //            _weekList = weekList;
    //            _storeRIDs = storeRIDs;
    //            _storeRidDictionary = storeRidDictionary;
    //            _storeIdDictionary = storeIdDictionary;
    //            _LOGGING = logging;
    //            _logMessages = new List<string>();
    //            _dtOOSColorNodes = dtOOSColorNodes;
    //            _dtSellThruColorNodes = dtSellThruColorNodes;
    //            SALES_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.SalesTotal);
    //            SALES_REG_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.SalesRegular);
    //            SALES_PROMO_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.SalesPromo);
    //            SALES_MKDN_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.SalesMarkdown);
    //            STOCK_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.StockTotal);
    //            STOCK_REG_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.StockRegular);
    //            STOCK_MKDN_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.StockMarkdown);

    //            IN_STOCK_SALES_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.InStockSales);
    //            IN_STOCK_SALES_REG_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.InStockSalesReg);
    //            IN_STOCK_SALES_PROMO_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.InStockSalesPromo);
    //            IN_STOCK_SALES_MKDN_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.InStockSalesMkdn);
    //            ACCUM_SELL_THRU_SALES_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.AccumSellThruSales);
    //            ACCUM_SELL_THRU_STOCK_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.AccumSellThruStock);
    //            DAYS_IN_STOCK_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.DaysInStock);
    //            RECEIVED_STOCK_VAR_NAME = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.ReceivedStock);
    //        }
    //        catch (Exception exc)
    //        {
    //            ExitMessageLevel = eMIDMessageLevel.Severe;
    //            Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "SizeDayToWeekSummaryProcess");
    //            Audit.Log_Exception(exc, GetType().Name);
    //            throw;
    //        }
    //    }

    //    //===========
    //    // PROPERTIES
    //    //===========

    //    public SessionAddressBlock SAB
    //    {
    //        get { return _SAB; }
    //    }
    //    public int ColorCnt
    //    {
    //        get { return _colorCnt; }
    //        set { _colorCnt = value; }

    //    }
    //    public int SizeCnt
    //    {
    //        get { return _sizeCnt; }
    //        set { _sizeCnt = value; }

    //    }
    //    public int TotReads
    //    {
    //        get { return _totReads; }
    //        set { _totReads = value; }

    //    }
    //    public int TotUpdates
    //    {
    //        get { return _totUpdates; }
    //        set { _totUpdates = value; }

    //    }
    //    public int ValCnt
    //    {
    //        get { return _valCnt; }
    //        set { _valCnt = value; }

    //    }

    //    public List<string> LogMessages
    //    {
    //        get { return _logMessages; }
    //        set { _logMessages = value; }

    //    }

    //    public string StyleId
    //    {
    //        get { return _styleId; }
    //        set { _styleId = value; }

    //    }

    //    // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //    public bool ProcessingCurrentDateInd
    //    {
    //        get { return _processingCurrentDateInd; }
    //        set { _processingCurrentDateInd = value; }

    //    }
    //    // End TT#2257

    //    //========
    //    // METHODS
    //    //========

    //    override public void ExecuteProcess()
    //    {
    //        eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
    //        string message = "Executing Size Day To Week Summary for Style: ";
    //        Hashtable storeHash = new Hashtable();
    //        List<SizeTimeCollection> sizeList = new List<SizeTimeCollection>();

    //        try
    //        {
    //            IsRunning = true;
    //            _styleId = _SAB.HierarchyServerSession.GetNodeID(_styleRid);
    //            message = message + _styleId;
    //            Audit.Add_Msg(eMIDMessageLevel.Information, message, "SizeDayToWeekSummaryProcess:ExecuteProcess");

    //            try
    //            {
    //                try
    //                {
    //                    _dlStoreVarHist = new StoreVariableHistoryBin(true, 0);

    //                    if (_LOGGING)
    //                    {
    //                        LogMessage("Style: " + _styleId + "(" + _styleRid + ")");
    //                    }

    //                    // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //                    // get color and sizes for style and convert to dictionary
    //                    HierarchyNodeProfile styleProfile = _SAB.HierarchyServerSession.GetNodeData(_styleRid, false);
    //                    ArrayList StyleColorSizeKeyList = SAB.HierarchyServerSession.GetAggregateSizeNodeList(styleProfile, Include.NoRID, Include.NoRID, null);
    //                    Dictionary<int, List<SizeAggregateBasisKey>> colorNodeList = new Dictionary<int, List<SizeAggregateBasisKey>>();
    //                    List<SizeAggregateBasisKey> sizesList;
    //                    foreach (SizeAggregateBasisKey sabk in StyleColorSizeKeyList)
    //                    {
    //                        if (!colorNodeList.TryGetValue(sabk.ColorCodeRID, out sizesList))
    //                        {
    //                            sizesList = new List<SizeAggregateBasisKey>();
    //                            colorNodeList[sabk.ColorCodeRID] = sizesList;
    //                        }
    //                        sizesList.Add(sabk);
    //                    }

    //                    //HierarchyNodeList colorNodeList = _SAB.HierarchyServerSession.GetHierarchyChildren(_styleLevelProf.Level, _mainHier.Key, _mainHier.Key, _styleRid, false, eNodeSelectType.NoVirtual);
    //                    // End TT#2257

    //                    if (colorNodeList.Count > 0)
    //                        LogMessage("No of Colors: " + colorNodeList.Count);

    //                    string msg = "STYLE:" + _styleId.ToString() + "  No of Colors: " + colorNodeList.Count;
    //                    //System.Console.Write(msg + System.Environment.NewLine);
    //                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Debug, msg, this.ToString());


    //                    //=================================================================
    //                    // Only bother to check if there are more than one color under Style
    //                    //=================================================================
    //                    if (colorNodeList.Count > 0)
    //                    {
    //                        //=================================
    //                        // Only process Colors with data
    //                        //=================================
    //                        List<int> sqlTimeList = new List<int>();
    //                        foreach (WeekProfile weekProf in _weekList)
    //                        {
    //                            foreach (DayProfile dayProf in weekProf.Days.ArrayList)
    //                            {
    //                                SQL_TimeID sqlTime = new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, dayProf.Key);
    //                                sqlTimeList.Add(sqlTime.SqlTimeID);
    //                            }
    //                        }
    //                        DataTable DtBinColor = _dlStoreVarHist.GetColorsForTimeRange(sqlTimeList);

    //                        // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //                        //HierarchyNodeList hnl = new HierarchyNodeList(eProfileType.HierarchyNode);
    //                        //foreach (HierarchyNodeProfile hnp in colorNodeList.ArrayList)
    //                        //{
    //                        //    DataRow[] rows = DtBinColor.Select("HN_RID = " + _styleRid + " and COLOR_CODE_RID = " + hnp.ColorOrSizeCodeRID);
    //                        //    if (rows.Length > 0)
    //                        //    {
    //                        //        hnl.Add(hnp);
    //                        //    }
    //                        //}
    //                        //string colorMsg = " Style ID: " + _styleId + ". " + colorNodeList.Count + " colors found. " + hnl.Count + " will be processed.";
    //                        ////Debug.WriteLine(colorMsg);
    //                        //LogMessage(colorMsg);
    //                        //colorNodeList = hnl;

    //                        Dictionary<int, List<SizeAggregateBasisKey>> colorNodeList2 = new Dictionary<int, List<SizeAggregateBasisKey>>();
    //                        foreach (KeyValuePair<int, List<SizeAggregateBasisKey>> colorSizeList in colorNodeList)
    //                        {
    //                            DataRow[] rows = DtBinColor.Select("HN_RID = " + _styleRid + " and COLOR_CODE_RID = " + colorSizeList.Key);
    //                            if (rows.Length > 0)
    //                            {
    //                                colorNodeList2[colorSizeList.Key] = colorSizeList.Value;
    //                            }
    //                        }
    //                        string colorMsg = " Style ID: " + _styleId + ". " + colorNodeList.Count + " colors found. " + colorNodeList.Count + " will be processed.";
    //                        LogMessage(colorMsg);
    //                        colorNodeList = colorNodeList2;
    //                        // End TT#2257
    //                    }

    //                    //===========
    //                    // by COLOR
    //                    //===========
    //                    //dlStoreVarHist.FlushAll();
    //                    // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //                    //foreach (HierarchyNodeProfile colorNode in colorNodeList.ArrayList)
    //                    foreach (KeyValuePair<int, List<SizeAggregateBasisKey>> colorSizeList in colorNodeList)
    //                    // End TT#2257
    //                    {
    //                        _sizeCodeHash = new Hashtable();
    //                        // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //                        //_colorCodeRid = colorNode.ColorOrSizeCodeRID;
    //                        _colorCodeRid = colorSizeList.Key;
    //                        // End TT#2257
    //                        _colorCnt++;
    //                        ColorCodeProfile ccp = null;
    //                        if (_LOGGING)
    //                        {
    //                            ccp = _SAB.HierarchyServerSession.GetColorCodeProfile(_colorCodeRid);
    //                            _colorCodeId = ccp.ColorCodeID;
    //                            LogMessage("  Color: " + _colorCodeRid);
    //                        }


    //                        //===================
    //                        // TIME (each week)
    //                        //===================
    //                        foreach (WeekProfile weekProf in _weekList)
    //                        {
    //                            bool valuesAllZeros = true;
    //                            //==========================
    //                            // Build day list for week
    //                            //==========================
    //                            _timeKeyList = new ArrayList();
    //                            foreach (DayProfile dayProf in weekProf.Days.ArrayList)
    //                            {
    //                                _timeKeyList.Add(new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, dayProf.Key));
    //                            }
    //                            LogMessage("    Week: " + weekProf.Text() + "(" + weekProf.Key + ") Bin Time ID: " + ((SQL_TimeID)(_timeKeyList[0])).SqlTimeID);

    //                            // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //                            //HierarchyNodeList sizeNodeList = _SAB.HierarchyServerSession.GetHierarchyChildren(_colorLevelProf.Level, _mainHier.Key, _mainHier.Key, colorNode.Key, false, eNodeSelectType.NoVirtual);
    //                            // End TT#2257

    //                            //================
    //                            // Debug Code
    //                            //================
    //                            //ccp = _SAB.HierarchyServerSession.GetColorCodeProfile(_colorCodeRid);
    //                            //_colorCodeId = ccp.ColorCodeID;
    //                            //System.Console.Write("  COLOR:" + _colorCodeId.ToString() + "  No of Sizes: " + sizeNodeList.Count + System.Environment.NewLine);


    //                            if (_LOGGING)
    //                            {
    //                                _sizeCodeHash.Clear();
    //                                // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //                                //foreach (HierarchyNodeProfile sizeNode in sizeNodeList.ArrayList)
    //                                //{
    //                                //    SizeCodeProfile scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(sizeNode.ColorOrSizeCodeRID);
    //                                //    _sizeCodeHash.Add(scp.Key, scp.SizeCodeID + "-" + scp.SizeCodeName);
    //                                //}
    //                                //LogMessage("    No of Sizes: " + sizeNodeList.ArrayList.Count);
    //                                foreach (SizeAggregateBasisKey sabk in colorSizeList.Value)
    //                                {
    //                                    HierarchyNodeProfile sizeNode = _SAB.HierarchyServerSession.GetNodeData(sabk.SizeNodeRID);
    //                                    SizeCodeProfile scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(sizeNode.ColorOrSizeCodeRID);
    //                                    _sizeCodeHash.Add(scp.Key, scp.SizeCodeID + "-" + scp.SizeCodeName);
    //                                }
    //                                LogMessage("    No of Sizes: " + colorSizeList.Value.Count);
    //                                // End TT#2257
    //                            }


    //                            storeHash = new Hashtable();
    //                            sizeList = new List<SizeTimeCollection>();
    //                            SizeTimeCollection sizeTimeCollection = null;

    //                            //===========
    //                            // by SIZES
    //                            //===========

    //                            // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //                            //foreach (HierarchyNodeProfile sizeNode in sizeNodeList.ArrayList)
    //                            foreach (SizeAggregateBasisKey sizeNode in colorSizeList.Value)
    //                            // End TT#2257
    //                            {
    //                                _sizeCnt++;

    //                                // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //                                // Do not process inactive sizes if processing the current date
    //                                if (_processingCurrentDateInd &&
    //                                    !sizeNode.SizeIsActive)
    //                                {
    //                                    if (_LOGGING)
    //                                    {
    //                                        StringBuilder logMsg = new StringBuilder(" Size ");
    //                                        SizeCodeProfile scp = _SAB.HierarchyServerSession.GetSizeCodeProfile(sizeNode.SizeCodeRID);
    //                                        logMsg.Append(scp.SizeCodeID);
    //                                        logMsg.Append( " is inactive and will be bypassed");
    //                                        LogMessage(logMsg.ToString());
    //                                    }
    //                                    continue;
    //                                }
    //                                // End TT#2257 

    //                                _varKeyList = new ArrayList();
    //                                // BEGIN TT#1979 - stodd - change size day to week summary to always do both in-stock-sales and in-stock-sales-reg
    //                                // NOTE: Putting all varaibles in list will cause both IN_STOCK_SALE and IN_STOCK_SALE_REG to be calculated all of the time.
    //                                //if (sizeNode.OTSPlanLevelType == eOTSPlanLevelType.Total)
    //                                {
    //                                    VarNameStruct vnsSales = new VarNameStruct(SALES_VAR_NAME, eForecastBaseDatabaseStoreVariables.SalesTotal);
    //                                    _varKeyList.Add(vnsSales);
    //                                    VarNameStruct vnsStock = new VarNameStruct(STOCK_VAR_NAME, eForecastBaseDatabaseStoreVariables.StockTotal);
    //                                    _varKeyList.Add(vnsStock);
    //                                }
    //                                //else
    //                                {
    //                                    VarNameStruct vnsSalesReg = new VarNameStruct(SALES_REG_VAR_NAME, eForecastBaseDatabaseStoreVariables.SalesRegular);
    //                                    _varKeyList.Add(vnsSalesReg);
    //                                    VarNameStruct vnsSalesMkdn = new VarNameStruct(SALES_MKDN_VAR_NAME, eForecastBaseDatabaseStoreVariables.SalesMarkdown);
    //                                    _varKeyList.Add(vnsSalesMkdn);
    //                                    VarNameStruct vnsSalesPromo = new VarNameStruct(SALES_PROMO_VAR_NAME, eForecastBaseDatabaseStoreVariables.SalesPromo);
    //                                    _varKeyList.Add(vnsSalesPromo);
    //                                    VarNameStruct vnsStockReg = new VarNameStruct(STOCK_REG_VAR_NAME, eForecastBaseDatabaseStoreVariables.StockRegular);
    //                                    _varKeyList.Add(vnsStockReg);
    //                                    VarNameStruct vnsStockMkdn = new VarNameStruct(STOCK_MKDN_VAR_NAME, eForecastBaseDatabaseStoreVariables.StockMarkdown);
    //                                    _varKeyList.Add(vnsStockMkdn);
    //                                }
    //                                // BEGIN TT#1979 - stodd - change size day to week summary to always do both in-stock-sales and in-stock-sales-reg

    //                                // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //                                //_sizeCodeRid = sizeNode.ColorOrSizeCodeRID;
    //                                _sizeCodeRid = sizeNode.SizeCodeRID;
    //                                // End TT#2257
    //                                LogMessage("      Size: " + _sizeCodeHash[_sizeCodeRid] + "(" + _sizeCodeRid + ")");

    //                                double[] valueList = null;

    //                                if (_LOGGING)
    //                                {
    //                                    StringBuilder logMsg = new StringBuilder("        Variable: ");
    //                                    for (int v = 0; v < _varKeyList.Count; v++)
    //                                    {
    //                                        logMsg.Append(((VarNameStruct)_varKeyList[v]).Name + "  ");
    //                                    }
    //                                    LogMessage(logMsg.ToString());
    //                                }


    //                                //=============
    //                                // by TIME
    //                                //=============
    //                                for (int t = 0; t < _timeKeyList.Count; t++)
    //                                {
    //                                    SQL_TimeID timeID = (SQL_TimeID)_timeKeyList[t];
    //                                    _timeId = timeID.SqlTimeID;
    //                                    _totReads++;

    //                                    //=============
    //                                    // by VARIABLE
    //                                    //=============
    //                                    for (int v = 0; v < _varKeyList.Count; v++)
    //                                    {
    //                                        _varName = ((VarNameStruct)_varKeyList[v]).Name;
    //                                        _varEnum = ((VarNameStruct)_varKeyList[v]).Enum;

    //                                        //=====================
    //                                        // get values by STORE
    //                                        //=====================
    //                                        //Debug.WriteLine(varName + " time: " + timeID.TimeID + "(" + timeID.SqlTimeID + ") style: " + _styleRid + " Color Cd: " + _colorCodeRid + " Size Cd: " + _sizeCodeRid + " no of Stores: " + storeRIDs.Length);
    //                                        valueList = _dlStoreVarHist.GetStoreVariableDayValue(_varName, _styleRid, timeID, _colorCodeRid, _sizeCodeRid, _storeRIDs);
    //                                        //==================================================================================================================
    //                                        // This is looking to see if all the values we've read for this color are zeros, so no processing needs to be done.
    //                                        //==================================================================================================================
    //                                        if (valuesAllZeros)
    //                                        {
    //                                            valuesAllZeros = IsAllZeroes(valueList);
    //                                        }

    //                                        if (valueList != null)
    //                                        {
    //                                            int storeValue = 0;
    //                                            for (int s = 0; s < _storeRIDs.Length; s++)
    //                                            {
    //                                                _valCnt++;
    //                                                storeValue = (int)valueList[s];
    //                                                if (storeHash.ContainsKey(s))
    //                                                    sizeList = (List<SizeTimeCollection>)storeHash[s];
    //                                                else
    //                                                {
    //                                                    sizeList = new List<SizeTimeCollection>();
    //                                                    storeHash.Add(s, sizeList);
    //                                                }
    //                                                //====================================================================
    //                                                // Set the SizeTimeCollection either by finding it already in the list
    //                                                // or by creating a new, empty, one.
    //                                                //====================================================================
    //                                                sizeTimeCollection = sizeList.Find(delegate(SizeTimeCollection svt) { return svt.SizeCodeRID == _sizeCodeRid; });

    //                                                if (sizeTimeCollection == null)
    //                                                {
    //                                                    sizeTimeCollection = new SizeTimeCollection(_sizeCodeRid);
    //                                                    // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //                                                    //sizeTimeCollection.SizeRID = sizeNode.Key;
    //                                                    sizeTimeCollection.SizeRID = sizeNode.SizeNodeRID;
    //                                                    // End TT#2257
    //                                                    sizeList.Add(sizeTimeCollection);
    //                                                }

    //                                                //if (storeValue != 0)
    //                                                //{
    //                                                //    Debug.WriteLine(_varName + " time: " + timeID.TimeID + "(" + timeID.SqlTimeID + ") style: " + _styleRid + " Color Cd: " + _colorCodeRid + " Size Cd: " + _sizeCodeRid + " Store: " + s + " Value: " + storeValue);
    //                                                //}

    //                                                SetSizeTimeCollectionValue(sizeTimeCollection, _varEnum, timeID.SqlTimeID, storeValue);

    //                                                //if (valueList[s] != 0)
    //                                                //{
    //                                                //    Debug.WriteLine("**" + _varName + " time: " + timeID.TimeID + "(" + timeID.SqlTimeID + ") style: " + _styleRid + " Color Cd: " + _colorCodeRid + " Size Cd: " + _sizeCodeRid + " VALUE: " + valueList[s]);
    //                                                //}
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                            if (!valuesAllZeros)
    //                            {
    //                                // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //                                //ProcessEachStoreForWeek(_styleRid, _colorCodeRid, storeHash, _storeRIDs, _timeKeyList, sizeNodeList);
    //                                ProcessEachStoreForWeek(_styleRid, _colorCodeRid, storeHash, _storeRIDs, _timeKeyList, colorSizeList.Value);
    //                                // End TT#2257
    //                            }
    //                            else
    //                            {
    //                                //===============
    //                                // Debug code
    //                                //===============
    //                                //ColorCodeProfile aColor = _SAB.HierarchyServerSession.GetColorCodeProfile(_colorCodeRid);
    //                                //_colorCodeId = aColor.ColorCodeID;
    //                                //string eMsg = _styleId + " " + _colorCodeId + " Color is all zeros.";
    //                                //System.Console.Write(eMsg + System.Environment.NewLine);
    //                                //_SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Debug, eMsg, this.ToString());
    //                            }
    //                        }
    //                    }
						
    //                }
    //                catch (Exception ex)
    //                {
    //                    ++NumberOfErrors;
    //                    Audit.Log_Exception(ex, GetType().Name);
    //                }
    //                finally
    //                {
    //                    //=========
    //                    // Cleanup
    //                    //=========
    //                    _dSales = null;
    //                    _dSalesReg = null;
    //                    _dSalesPromo = null;
    //                    _dSalesMkdn = null;
    //                    _dStock = null;
    //                    _dStockReg = null;
    //                    _dStockMkdn = null;
    //                    _dInStockSales = null;
    //                    _dInStockSalesReg = null;
    //                    _dInStockSalesPromo = null;
    //                    _dInStockSalesMkdn = null;
    //                    _dAccumSellThruSales = null;
    //                    _dAccumSellThruStock = null;
    //                    _dDaysInStock = null;
    //                    _dReceivedStock = null;

    //                    _dlStoreVarHist = null;
    //                    //IDictionaryEnumerator storeHashEnum;
    //                    //storeHashEnum = storeHash.GetEnumerator();
    //                    //while (storeHashEnum.MoveNext())
    //                    //{
    //                    //    List<SizeTimeCollection> sl = (List<SizeTimeCollection>)storeHashEnum.Value;
    //                    //    foreach (SizeTimeCollection stc in sl)
    //                    //    {
    //                    //        stc.Clear();
    //                    //    }
    //                    //    sl.Clear();
    //                    //    sl = null;
    //                    //}
    //                    //storeHash.Clear();
    //                    //storeHash = null;
    //                    // End Cleanup

    //                    string fMsg = "Style " + _styleId + " Colors: " + _colorCnt + " Sizes: " + _sizeCnt + " Total Daily Store Reads: " + _totReads + " Values Read: " + _valCnt + " Total Weekly Store Records Updated: " + _totUpdates + " No of Errors: " + NumberOfErrors;
    //                    LogMessage(fMsg);
    //                    Debug.WriteLine(fMsg);
    //                    //System.Console.Write(fMsg + System.Environment.NewLine);
    //                    _SAB.ApplicationServerSession.Audit.Add_Msg(eMIDMessageLevel.Debug, fMsg, this.ToString());
    //                    //VarData.CloseUpdateConnection();
    //                }
    //            }
    //            catch (Exception ex)
    //            {
    //                Audit.Log_Exception(ex, GetType().Name);
    //            }

    //            messageLevel = Audit.HighestMessageLevel;
    //        }
    //        catch (ThreadAbortException)
    //        {
    //            try
    //            {
    //                Audit.Add_Msg(eMIDMessageLevel.Warning, "Cancelled by User", "CommandThreadProcessor");

    //                messageLevel = eMIDMessageLevel.Severe;
    //            }
    //            catch (InvalidOperationException)
    //            {
    //                messageLevel = eMIDMessageLevel.Severe;
    //            }
    //        }
    //        catch (Exception exc)
    //        {
    //            Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteProcess", "CommandThreadProcessor");
    //            Audit.Log_Exception(exc, GetType().Name);

    //            messageLevel = eMIDMessageLevel.Severe;
    //        }
    //        finally
    //        {
    //            //==================================
    //            // throw counts to process manager
    //            //==================================
    //            SizeDayToWeekSummaryProcessEndEventArgs eventArgs = new SizeDayToWeekSummaryProcessEndEventArgs(this);
    //            if (OnSizeDayToWeekSummaryProcessEndHandler != null)
    //            {
    //                OnSizeDayToWeekSummaryProcessEndHandler(this, eventArgs);
    //            }

    //            IsRunning = false;
    //            CompletionDateTime = DateTime.Now;
    //            ExitMessageLevel = messageLevel;
    //            message = "Completed Size Day To Week Summary for Style: " + _styleId;

    //            Audit.Add_Msg(eMIDMessageLevel.Information, message + " with Message Level " + MIDText.GetTextOnly((int)ExitMessageLevel), "CommandThreadProcessor");
    //        }
    //    }

    //    private void ProcessEachStoreForWeek(
    //        int styleCodeRid,
    //        int colorCodeRid,
    //        Hashtable storeHash,
    //        int[] storeRIDs,
    //        ArrayList timeKeyList,
    //        // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //        //HierarchyNodeList sizeNodeList)
    //        List<SizeAggregateBasisKey> sizeNodeList)
    //        // End TT#2257
    //    {
    //        // Begin TT#861 - stodd
    //        int[] styleRidVector = new int[sizeNodeList.Count];
    //        int[] colorCodeRidVector = new int[sizeNodeList.Count];
    //        int[] sizeNodeRidVector = new int[sizeNodeList.Count];
    //        int[] sizeCodeRidVector = new int[sizeNodeList.Count];
    //        SQL_TimeID[] timeVector = new SQL_TimeID[sizeNodeList.Count];
    //        // End TT#861 - stodd

    //        try
    //        {
    //            string varName = MIDText.GetTextOnly((int)eForecastBaseDatabaseStoreVariables.DaysInStock);
    //            SQL_TimeID firstDayTimeID = (SQL_TimeID)timeKeyList[0];
    //            SQL_TimeID weekTimeID = new SQL_TimeID(eSQLTimeIdType.TimeIdIsWeekly, firstDayTimeID.TimeID);
    //            IDictionaryEnumerator storeHashEnum;
    //            storeHashEnum = storeHash.GetEnumerator();
    //            bool usesOutOfStock = false;
    //            bool usesSellThruLimit = false;
    //            DataTable dtStoreSizeWeek = BuildStoreSizeWeekDataTable();

    //            Hashtable prevWeekHash = new Hashtable(); // holds the previous week by sizeCodeRid
    //            _sellThruLimitDictionary = new Dictionary<int, SizeSellThruProfile>();
    //            _OutOfStockDictionary = new Dictionary<int, SizeOutOfStockProfile>();

    //            //=====================
    //            // Out Of Stock Setup
    //            //=====================
    //            DataRow[] colorRows = _dtOOSColorNodes.Select("HN_RID = " + _colorRid);
    //            if (colorRows.Length > 0)
    //            {
    //                usesOutOfStock = true;
    //            }
    //            //MIDTimer oosTimer = new MIDTimer();
    //            //oosTimer.Start();
    //            CollectionOOSLookupDecoder oosLookupDecoder = null;
    //            if (usesOutOfStock)
    //            {
    //                SizeOutOfStockProfile sizeOutOfStockProfile = _SAB.HierarchyServerSession.GetSizeOutOfStockProfile(_styleRid, Include.NoRID, Include.NoRID, false);
    //                SizeOutOfStockLookupProfile oosLookup = new SizeOutOfStockLookupProfile(sizeOutOfStockProfile);
    //                Hashtable oosStoreGroupLevelHash = SAB.StoreServerSession.GetStoreGroupLevelHashTable(oosLookup.StoreGroupRid);
    //                oosLookupDecoder = new CollectionOOSLookupDecoder(oosLookup.CollectionSets, oosStoreGroupLevelHash);
    //                oosLookupDecoder.DebugSetsCollection();
    //            }
    //            //oosTimer.Stop(_colorCodeId + " OOS Init Timer: ");
    //            //========================
    //            // END Out Of Stock Setup
    //            //========================

    //            //=====================
    //            // Sell Thru Limit Setup
    //            //=====================
    //            colorRows = _dtSellThruColorNodes.Select("HN_RID = " + _colorRid);
    //            if (colorRows.Length > 0)
    //            {
    //                usesSellThruLimit = true;
    //            }
    //            //============================
    //            // END Sell Thru Limit Setup
    //            //============================


    //            //=====================
    //            // Process each Store
    //            //=====================
    //            while (storeHashEnum.MoveNext())
    //            {
    //                // inStockDay notes if we are using a specifics week's data in the IN_STOCK totals
    //                List<bool> inStockDay = new List<bool>(new bool[] { true, true, true, true, true, true, true });
    //                int storeIdx = (int)storeHashEnum.Key;
    //                int storeRid = 	_storeRidDictionary[storeIdx];

    //                int outOfStock = 0;
    //                //======================
    //                // Out of Stock lookup
    //                //======================
    //                if (usesOutOfStock)
    //                {
    //                    SizeOOSLookupItemBase oosLookup = (SizeOOSLookupItemBase)oosLookupDecoder.GetItemForStore(storeRid, _colorCodeRid, (SizeCodeProfile)_sizeCodeProfileHash[_sizeCodeRid]);
    //                    //Debug.WriteLine("oosLookup - Store: " + _storeIdDictionary[storeIdx] +
    //                    //    " ColorCodeRid: " + _colorCodeRid +
    //                    //    " size: " + ((SizeCodeProfile)_sizeCodeProfileHash[_sizeCodeRid]).SizeCodeID +
    //                    //    " Value: " + oosLookup.OOSQuantity);
    //                    outOfStock = oosLookup.OOSQuantity;
    //                }
    //                //=========================
    //                // END Out of Stock lookup
    //                //=========================

    //                List<SizeTimeCollection> sizeList = (List<SizeTimeCollection>)storeHashEnum.Value;
    //                bool receivedStock = false;
    //                LogMessage("      STORE " + _storeIdDictionary[storeIdx] + "(" + storeRIDs[storeIdx].ToString() + ")");
    //                //=============================================================
    //                // FIRST time through: we figure out which days to exclude
    //                // based upon no stock. Also, any variables not dependent upon
    //                // the 'no stock' indicator are summed for each day.
    //                //=============================================================
    //                // Look at each size...
    //                //=============================================================
    //                foreach (SizeTimeCollection sizeTimeCollection in sizeList)
    //                {
    //                    int salesAccum = 0;
    //                    int salesRegAccum = 0;
    //                    int salesPromoAccum = 0;
    //                    int salesMkdnAccum = 0;
    //                    int stockAccum = 0;
    //                    int stockRegAccum = 0;
    //                    int stockMkdnAccum = 0;

    //                    sizeTimeCollection.AccumTotals(timeKeyList);

    //                    SizePreviousWeek prevWeek = null;
    //                    if (prevWeekHash.ContainsKey(sizeTimeCollection.SizeCodeRID))
    //                    {
    //                        prevWeek = (SizePreviousWeek)prevWeekHash[sizeTimeCollection.SizeCodeRID];

    //                    }
    //                    else
    //                    {
    //                        prevWeek = new SizePreviousWeek(styleCodeRid, colorCodeRid, sizeTimeCollection.SizeCodeRID, storeRIDs, weekTimeID);
    //                        prevWeekHash.Add(sizeTimeCollection.SizeCodeRID, prevWeek);
    //                    }
    //                    receivedStock = Include.ConvertIntToBool(prevWeek.GetVariableValue(storeIdx, RECEIVED_STOCK_VAR_NAME, eForecastBaseDatabaseStoreVariables.ReceivedStock));
    //                    double prevWeekAccumSales = prevWeek.GetVariableValue(storeIdx, ACCUM_SELL_THRU_SALES_VAR_NAME, eForecastBaseDatabaseStoreVariables.AccumSellThruSales);
    //                    double prevWeekAccumStock = prevWeek.GetVariableValue(storeIdx, ACCUM_SELL_THRU_STOCK_VAR_NAME, eForecastBaseDatabaseStoreVariables.AccumSellThruStock);
    //                    LogMessage("        Previously Received Stock = " + receivedStock);
    //                    LogMessage("        Previous Week's Accum Sales = " + prevWeekAccumSales);
    //                    LogMessage("        Previous Week's Accum Stock = " + prevWeekAccumStock);
    //                    //============
    //                    // LOGGING
    //                    //============
    //                    if (_LOGGING)
    //                    {
    //                        LogCache(styleCodeRid, colorCodeRid, storeIdx, sizeTimeCollection);
    //                    }

    //                    if (sizeTimeCollection.AllZeroes)
    //                    {
    //                        if (receivedStock)
    //                        {
    //                            bool[] allFalse = { false, false, false, false, false, false, false };

    //                            inStockDay.Clear();
    //                            inStockDay.AddRange(allFalse);
    //                        }
    //                        //=====================================================================================
    //                        // The following variables can be summed independent of the "Received Stock" indicator
    //                        //=====================================================================================
    //                        for (int t = 0; t < timeKeyList.Count; t++)
    //                        {
    //                            SQL_TimeID aTimeID = (SQL_TimeID)timeKeyList[t];
    //                            int aValue = sizeTimeCollection.GetSalesValue(aTimeID.SqlTimeID);
    //                            salesAccum += aValue;
    //                            aValue = sizeTimeCollection.GetSalesRegValue(aTimeID.SqlTimeID);
    //                            salesRegAccum += aValue;
    //                            aValue = sizeTimeCollection.GetSalesPromoValue(aTimeID.SqlTimeID);
    //                            salesPromoAccum += aValue;
    //                            aValue = sizeTimeCollection.GetSalesMkdnValue(aTimeID.SqlTimeID);
    //                            salesMkdnAccum += aValue;
    //                            aValue = sizeTimeCollection.GetStockValue(aTimeID.SqlTimeID);
    //                            stockAccum += aValue;
    //                            aValue = sizeTimeCollection.GetStockRegValue(aTimeID.SqlTimeID);
    //                            stockRegAccum += aValue;
    //                            aValue = sizeTimeCollection.GetStockMkdnValue(aTimeID.SqlTimeID);
    //                            stockMkdnAccum += aValue;
    //                        }
    //                        StoreSizeWeekInsert(dtStoreSizeWeek, storeIdx, sizeTimeCollection.SizeCodeRID, salesAccum, salesRegAccum, salesPromoAccum, salesMkdnAccum, stockAccum, stockRegAccum, stockMkdnAccum, receivedStock);
    //                    }
    //                    else
    //                    {
    //                        double accumSellThruSales = prevWeekAccumSales;
    //                        double accumSellThruStock = prevWeekAccumStock;

    //                        for (int t = 0; t < timeKeyList.Count; t++)
    //                        {
    //                            SQL_TimeID aTimeID = (SQL_TimeID)timeKeyList[t];
    //                            double storeDayStock = (double)sizeTimeCollection.GetStockTotalValue(aTimeID.SqlTimeID);
    //                            double storeDaySales = (double)sizeTimeCollection.GetSalesTotalValue(aTimeID.SqlTimeID);
    //                            //===========================
    //                            // SELL THRU THRESHOLD LOGIC
    //                            //===========================
    //                            accumSellThruSales += storeDaySales;
    //                            accumSellThruStock += storeDayStock;
    //                            double daySellThru = 0;
    //                            if (storeDayStock != 0)
    //                            {
    //                                daySellThru = storeDaySales / storeDayStock;
    //                            }

    //                            // Begin TT#483 - stodd - add out of stock / lost sales 
    //                            float sellThruThreshold = 0.0f;
    //                            if (usesSellThruLimit)
    //                            {
    //                                sellThruThreshold = GetSizeSellThruLimit(sizeTimeCollection.SizeRID);
    //                            }
    //                            // End TT#483 - stodd - add out of stock / lost sales 

    //                            if (daySellThru < sellThruThreshold)
    //                            {
    //                                receivedStock = false;
    //                                LogMessage("        SellThru (" + daySellThru + ") dropped below Threshold (" + sellThruThreshold + "). Received Stock set to false as of " + aTimeID.TimeID);
    //                            }
    //                            else
    //                            {
    //                                if (storeDayStock > 0)
    //                                {
    //                                    receivedStock = true;
    //                                }
    //                            }
								
    //                            //================================
    //                            // END SELL THRU THRESHOLD LOGIC
    //                            //================================

    //                            if (receivedStock)
    //                            {
    //                                //=================================================================
    //                                // Out of Stock value defaults to 0, but could be changed above.
    //                                //=================================================================
    //                                if (storeDayStock <= outOfStock)
    //                                {
    //                                    inStockDay[t] = false;
    //                                }
    //                            }
    //                            //=====================================================================================
    //                            // The following variables can be summed independent of the "Received Stock" indicator
    //                            //=====================================================================================
    //                            int aValue = sizeTimeCollection.GetSalesValue(aTimeID.SqlTimeID);
    //                            salesAccum += aValue;
    //                            aValue = sizeTimeCollection.GetSalesRegValue(aTimeID.SqlTimeID);
    //                            salesRegAccum += aValue;
    //                            aValue = sizeTimeCollection.GetSalesPromoValue(aTimeID.SqlTimeID);
    //                            salesPromoAccum += aValue;
    //                            aValue = sizeTimeCollection.GetSalesMkdnValue(aTimeID.SqlTimeID);
    //                            salesMkdnAccum += aValue;
    //                            aValue = sizeTimeCollection.GetStockValue(aTimeID.SqlTimeID);
    //                            stockAccum += aValue;
    //                            aValue = sizeTimeCollection.GetStockRegValue(aTimeID.SqlTimeID);
    //                            stockRegAccum += aValue;
    //                            aValue = sizeTimeCollection.GetStockMkdnValue(aTimeID.SqlTimeID);
    //                            stockMkdnAccum += aValue;
    //                        }
    //                        StoreSizeWeekInsert(dtStoreSizeWeek, storeIdx, sizeTimeCollection.SizeCodeRID, salesAccum, salesRegAccum, salesPromoAccum, salesMkdnAccum, stockAccum, stockRegAccum, stockMkdnAccum, receivedStock);
    //                    }
    //                }

    //                LogMessage("      In Stock Days for All Sizes: " +
    //                        ((SQL_TimeID)timeKeyList[0]).SqlTimeID + "/" + inStockDay[0] + " " +
    //                        ((SQL_TimeID)timeKeyList[1]).SqlTimeID + "/" + inStockDay[1] + " " +
    //                        ((SQL_TimeID)timeKeyList[2]).SqlTimeID + "/" + inStockDay[2] + " " +
    //                        ((SQL_TimeID)timeKeyList[3]).SqlTimeID + "/" + inStockDay[3] + " " +
    //                        ((SQL_TimeID)timeKeyList[4]).SqlTimeID + "/" + inStockDay[4] + " " +
    //                        ((SQL_TimeID)timeKeyList[5]).SqlTimeID + "/" + inStockDay[5] + " " +
    //                        ((SQL_TimeID)timeKeyList[6]).SqlTimeID + "/" + inStockDay[6]);
    //                LogMessage(" ");

    //                //=============================================================
    //                // SECOND time through we accum the sales and stock totals
    //                // for all weeks that have not been excluded.
    //                //=============================================================
    //                // Look at each size...
    //                //=============================================================
    //                foreach (SizeTimeCollection sizeTimeCollection in sizeList)
    //                {
    //                    SizePreviousWeek prevWeek = null;
    //                    if (prevWeekHash.ContainsKey(sizeTimeCollection.SizeCodeRID))
    //                    {
    //                        prevWeek = (SizePreviousWeek)prevWeekHash[sizeTimeCollection.SizeCodeRID];

    //                    }
    //                    else
    //                    {
    //                        prevWeek = new SizePreviousWeek(styleCodeRid, colorCodeRid, sizeTimeCollection.SizeCodeRID, storeRIDs, weekTimeID);
    //                        prevWeekHash.Add(sizeTimeCollection.SizeCodeRID, prevWeek);
    //                    }

    //                    int inStockSalesAccum = 0;
    //                    int inStockSalesRegAccum = 0;
    //                    int inStockSalesPromoAccum = 0;
    //                    int inStockSalesMkdnAccum = 0;
    //                    int accumSellThruSales = prevWeek.GetVariableValue(storeIdx, ACCUM_SELL_THRU_SALES_VAR_NAME, eForecastBaseDatabaseStoreVariables.AccumSellThruSales);
    //                    int accumSellThruStock = prevWeek.GetVariableValue(storeIdx, ACCUM_SELL_THRU_STOCK_VAR_NAME, eForecastBaseDatabaseStoreVariables.AccumSellThruStock);
    //                    int daysInStock = 0;

    //                    //receivedStock = false;

    //                    int aValue = 0;
    //                    for (int t = 0; t < timeKeyList.Count; t++)
    //                    {
    //                        SQL_TimeID aTimeID = (SQL_TimeID)timeKeyList[t];
    //                        //=========================================================
    //                        // Accum values that depend up In Stock switch
    //                        //=========================================================
    //                        if (inStockDay[t])
    //                        {
    //                            aValue = sizeTimeCollection.GetSalesValue(aTimeID.SqlTimeID);
    //                            inStockSalesAccum += aValue;
    //                            aValue = sizeTimeCollection.GetSalesRegValue(aTimeID.SqlTimeID);
    //                            inStockSalesRegAccum += aValue;
    //                            aValue = sizeTimeCollection.GetSalesPromoValue(aTimeID.SqlTimeID);
    //                            inStockSalesPromoAccum += aValue;
    //                            aValue = sizeTimeCollection.GetSalesMkdnValue(aTimeID.SqlTimeID);
    //                            inStockSalesMkdnAccum += aValue;
    //                        }
    //                        //=========================================================
    //                        // Accum values that DO NOT depend up In Stock switch
    //                        //=========================================================
    //                        aValue = sizeTimeCollection.GetSalesTotalValue(aTimeID.SqlTimeID);
    //                        accumSellThruSales += aValue;
    //                        aValue = sizeTimeCollection.GetStockTotalValue(aTimeID.SqlTimeID);
    //                        accumSellThruStock += aValue;
    //                        //=========================================================
    //                        // This "aValue" should be from Stock Total just above
    //                        //=========================================================
    //                        if (aValue > 0)
    //                        {
    //                            daysInStock++;
    //                            //receivedStock = true;
    //                        }
    //                    }

    //                    //int receivedStockNum = 0;
    //                    //if (receivedStock)
    //                    //{
    //                    //    receivedStockNum = 1;
    //                    //}
    //                    StoreSizeWeekUpdate(dtStoreSizeWeek, storeIdx, sizeTimeCollection.SizeCodeRID, inStockSalesAccum, inStockSalesRegAccum, inStockSalesPromoAccum, inStockSalesMkdnAccum,
    //                            accumSellThruSales, accumSellThruStock, daysInStock);
    //                }
    //            }

    //            //=================================================================================
    //            // At this points all stores / size info has been accumed and saved into the 
    //            // dtStoreSizeWeek datatable. Now we unload it and update the weekly bin values.
    //            //=================================================================================

    //            LogMessage(new String('=', 80));
    //            LogMessage(" BEGIN WRITING WEEKLY VALUES FOR STYLE/COLOR CODE " + _styleId + "(" + styleCodeRid + ")" + " / " + _colorCodeId + "(" + colorCodeRid + ")");
    //            LogMessage(new String('=', 80));

    //            DataView dvStoreSizeWeek = new DataView(dtStoreSizeWeek);
    //            dvStoreSizeWeek.Sort = "SIZE_CODE_RID asc, STORE_IDX asc";
    //            int currSizeCodeRid = Include.NoRID;

    //            //==============================================================
    //            // Gathers up all of the keeys needed for locking & unlocking
    //            //==============================================================
    //            // Begin TT#861 - stodd
    //            styleRidVector = new int[sizeNodeList.Count];
    //            colorCodeRidVector = new int[sizeNodeList.Count];
    //            sizeNodeRidVector = new int[sizeNodeList.Count];
    //            sizeCodeRidVector = new int[sizeNodeList.Count];
    //            timeVector = new SQL_TimeID[sizeNodeList.Count];
    //            // Begin TT#861 - stodd
    //            int si = 0;
    //            // Begin TT#2257 - JSmith - Size Day to Week Summary Performance
    //            //foreach (HierarchyNodeProfile sizeNode in sizeNodeList.ArrayList)
    //            //{
    //            //    timeVector[si] = weekTimeID;
    //            //    styleRidVector[si] = styleCodeRid;
    //            //    colorCodeRidVector[si] = colorCodeRid;
    //            //    sizeNodeRidVector[si] = sizeNode.Key;
    //            //    sizeCodeRidVector[si] = sizeNode.ColorOrSizeCodeRID;
    //            //    si++;
    //            //}
    //            foreach (SizeAggregateBasisKey sizeNode in sizeNodeList)
    //            {
    //                timeVector[si] = weekTimeID;
    //                styleRidVector[si] = styleCodeRid;
    //                colorCodeRidVector[si] = colorCodeRid;
    //                sizeNodeRidVector[si] = sizeNode.SizeNodeRID;
    //                sizeCodeRidVector[si] = sizeNode.SizeCodeRID;
    //                si++;
    //            }
    //            // End TT#2257
    //            //==============================================================
    //            //==============================================================
    //            _dlStoreVarHist.FlushAll();

    //            _dlStoreVarHist.LockTimeHnRIDNode(_SAB.ClientServerSession.UserRID, timeVector, sizeNodeRidVector, styleRidVector, colorCodeRidVector, sizeCodeRidVector);


    //            foreach (DataRowView aRowView in dvStoreSizeWeek)
    //            {
    //                int rowSizeCodeRid = int.Parse(aRowView["SIZE_CODE_RID"].ToString());
    //                if (rowSizeCodeRid == currSizeCodeRid)
    //                {
    //                    // accum
    //                    AccumDoubleArrays(aRowView);
    //                }
    //                else
    //                {
    //                    // write accums 
    //                    if (currSizeCodeRid != Include.NoRID)
    //                    {
    //                        WriteDoubleArrays(styleCodeRid, colorCodeRid, currSizeCodeRid, weekTimeID, storeRIDs);
    //                    }
    //                    // Init Accuks and  accum next size
    //                    currSizeCodeRid = rowSizeCodeRid;
    //                    InitDoubleArrays();
    //                    AccumDoubleArrays(aRowView);
    //                }

    //            }
    //            // This catches the accum of the last size code and writes it out.
    //            if (currSizeCodeRid != Include.NoRID)
    //            {
    //                WriteDoubleArrays(styleCodeRid, colorCodeRid, currSizeCodeRid, weekTimeID, storeRIDs);
    //            }

    //            string message;
    //            if (!_dlStoreVarHist.Commit(out message))
    //            {
    //                LogMessage(message);
    //                //returnCode = eReturnCode.severe;
    //            }
    //            //_dlStoreVarHist.UnLockTimeHnRID(timeVector, styleRidVector, colorCodeRidVector, sizeCodeRidVector);

    //            //dvStoreSizeWeek.Table = null;
    //            //dvStoreSizeWeek = null;
    //            //dtStoreSizeWeek.Rows.Clear();
    //            //dtStoreSizeWeek.Clear();
    //            //dtStoreSizeWeek = null;
    //            //prevWeekHash.Clear();
    //            //prevWeekHash = null;

    //            LogMessage(new String('=', 80));
    //            LogMessage(" COMPLETED WRITING WEEKLY VALUES FOR STYLE/COLOR CODE " + _styleId + "(" + styleCodeRid + ")" + " / " + _colorCodeId + "(" + colorCodeRid + ")");
    //            LogMessage(new String('=', 80));
    //            LogMessage(" ");
    //        }
    //        catch (Exception ex)
    //        {
    //            string msg = ex.ToString();
    //            throw;
    //        }
    //        // Begin TT#861 - stodd
    //        finally
    //        {
    //            _dlStoreVarHist.UnLockTimeHnRID(timeVector, styleRidVector, colorCodeRidVector, sizeCodeRidVector);
    //        }
    //        // End TT#861
    //    }

    //    private float GetSizeSellThruLimit(int sizeRid)
    //    {
    //        SizeSellThruProfile sst = null;
    //        if (_sellThruLimitDictionary.ContainsKey(sizeRid))
    //        {
    //            sst = _sellThruLimitDictionary[sizeRid];
    //        }
    //        else
    //        {
    //            sst = _SAB.HierarchyServerSession.GetSizeSellThruProfile(sizeRid, false);
    //            _sellThruLimitDictionary.Add(sizeRid, sst);
    //        }
    //        return sst.SellThruLimit;
    //    }

    //    private DataTable BuildStoreSizeWeekDataTable()
    //    {
    //        DataTable dtStoreSizeWeek = MIDEnvironment.CreateDataTable();

    //        DataColumn dataColumn = new DataColumn();
    //        dataColumn.DataType = System.Type.GetType("System.Int32");
    //        dataColumn.ColumnName = "STORE_IDX";
    //        dtStoreSizeWeek.Columns.Add(dataColumn);

    //        dataColumn = new DataColumn();
    //        dataColumn.DataType = System.Type.GetType("System.Int32");
    //        dataColumn.ColumnName = "SIZE_CODE_RID";
    //        dtStoreSizeWeek.Columns.Add(dataColumn);


    //        DataTable dtVar = MIDText.GetLabels((int)eForecastBaseDatabaseStoreVariables.SalesTotal, (int)eForecastBaseDatabaseStoreVariables.ReceivedStock);
    //        foreach (DataRow aRow in dtVar.Rows)
    //        {
    //            string textValue = aRow["TEXT_VALUE"].ToString();
    //            dataColumn = new DataColumn();
    //            dataColumn.DataType = System.Type.GetType("System.Int32");
    //            dataColumn.ColumnName = textValue;
    //            dataColumn.DefaultValue = 0;
    //            dtStoreSizeWeek.Columns.Add(dataColumn);
    //        }

    //        DataColumn[] PrimaryKeyColumn;
    //        PrimaryKeyColumn = new DataColumn[2];
    //        PrimaryKeyColumn[0] = dtStoreSizeWeek.Columns["STORE_IDX"];
    //        PrimaryKeyColumn[1] = dtStoreSizeWeek.Columns["SIZE_CODE_RID"];
    //        dtStoreSizeWeek.PrimaryKey = PrimaryKeyColumn;

    //        //================================================================
    //        // This is so the insert and update of the DT row values can be
    //        // done by column, which is faster than using column names.
    //        //================================================================
    //        _dcStoreIdx = dtStoreSizeWeek.Columns["STORE_IDX"];
    //        _dcSizeCodeRid = dtStoreSizeWeek.Columns["SIZE_CODE_RID"];
    //        _dcSales = dtStoreSizeWeek.Columns[SALES_VAR_NAME];
    //        _dcSalesReg = dtStoreSizeWeek.Columns[SALES_REG_VAR_NAME];
    //        _dcSalesPromo = dtStoreSizeWeek.Columns[SALES_PROMO_VAR_NAME];
    //        _dcSalesMkdn = dtStoreSizeWeek.Columns[SALES_MKDN_VAR_NAME];
    //        _dcStock = dtStoreSizeWeek.Columns[STOCK_VAR_NAME];
    //        _dcStockReg = dtStoreSizeWeek.Columns[STOCK_REG_VAR_NAME];
    //        _dcStockMkdn = dtStoreSizeWeek.Columns[STOCK_MKDN_VAR_NAME];
    //        _dcReceivedStock = dtStoreSizeWeek.Columns[RECEIVED_STOCK_VAR_NAME];

    //        _dcInStockSales = dtStoreSizeWeek.Columns[IN_STOCK_SALES_VAR_NAME];
    //        _dcInStockSalesReg = dtStoreSizeWeek.Columns[IN_STOCK_SALES_REG_VAR_NAME];
    //        _dcInStockSalesPromo = dtStoreSizeWeek.Columns[IN_STOCK_SALES_PROMO_VAR_NAME];
    //        _dcInStockSalesMkdn = dtStoreSizeWeek.Columns[IN_STOCK_SALES_MKDN_VAR_NAME];
    //        _dcAccumSellThruSales = dtStoreSizeWeek.Columns[ACCUM_SELL_THRU_SALES_VAR_NAME];
    //        _dcAccumSellThruStock = dtStoreSizeWeek.Columns[ACCUM_SELL_THRU_STOCK_VAR_NAME];
    //        _dcDaysInStock = dtStoreSizeWeek.Columns[DAYS_IN_STOCK_VAR_NAME];

    //        return dtStoreSizeWeek;
    //    }

    //    private void SetSizeTimeCollectionValue(SizeTimeCollection sizeColl, eForecastBaseDatabaseStoreVariables varEnum, int sqlTimeId, int storeValue)
    //    {
    //        if (varEnum == eForecastBaseDatabaseStoreVariables.SalesTotal)
    //            sizeColl.SetSalesValue(sqlTimeId, storeValue);
    //        if (varEnum == eForecastBaseDatabaseStoreVariables.SalesRegular)
    //            sizeColl.SetSalesRegValue(sqlTimeId, storeValue);
    //        if (varEnum == eForecastBaseDatabaseStoreVariables.SalesPromo)
    //            sizeColl.SetSalesPromoValue(sqlTimeId, storeValue);
    //        if (varEnum == eForecastBaseDatabaseStoreVariables.SalesMarkdown)
    //            sizeColl.SetSalesMkdnValue(sqlTimeId, storeValue);
    //        if (varEnum == eForecastBaseDatabaseStoreVariables.StockTotal)
    //            sizeColl.SetStockValue(sqlTimeId, storeValue);
    //        if (varEnum == eForecastBaseDatabaseStoreVariables.StockRegular)
    //            sizeColl.SetStockRegValue(sqlTimeId, storeValue);
    //        if (varEnum == eForecastBaseDatabaseStoreVariables.StockMarkdown)
    //            sizeColl.SetStockMkdnValue(sqlTimeId, storeValue);
    //    }

    //    private void StoreSizeWeekInsert(DataTable dt, int store, int size, int sales, int salesReg, int salesPromo, int salesMkdn, int stock, int stockReg, int stockMkdn, bool receivedStock)
    //    {
    //        DataRow aRow = dt.NewRow();
    //        aRow[_dcStoreIdx] = store;
    //        aRow[_dcSizeCodeRid] = size;
    //        aRow[_dcSales] = sales;
    //        aRow[_dcSalesReg] = salesReg;
    //        aRow[_dcSalesPromo] = salesPromo;
    //        aRow[_dcSalesMkdn] = salesMkdn;
    //        aRow[_dcStock] = stock;
    //        aRow[_dcStockReg] = stockReg;
    //        aRow[_dcStockMkdn] = stockMkdn;
    //        if (receivedStock)
    //            aRow[_dcReceivedStock] = 1;
    //        else
    //            aRow[_dcReceivedStock] = 0;

    //        //aRow["STORE_IDX"] = store;
    //        //aRow["SIZE_CODE_RID"] = size;
    //        //aRow[SALES_VAR_NAME] = sales;
    //        //aRow[SALES_REG_VAR_NAME] = salesReg;
    //        //aRow[SALES_PROMO_VAR_NAME] = salesPromo;
    //        //aRow[SALES_MKDN_VAR_NAME] = salesMkdn;
    //        //aRow[STOCK_VAR_NAME] = stock;
    //        //aRow[STOCK_REG_VAR_NAME] = stockReg;
    //        //aRow[STOCK_MKDN_VAR_NAME] = stockMkdn;
    //        //if (receivedStock)
    //        //    aRow[RECEIVED_STOCK_VAR_NAME] = 1;
    //        //else
    //        //    aRow[RECEIVED_STOCK_VAR_NAME] = 0;

    //        //aRow["IN_STOCK_SALES_VAR_NAME"] = 0;
    //        //aRow["IN_STOCK_SALES_REG_VAR_NAME"] = 0;
    //        //aRow["IN_STOCK_SALES_PROMO_VAR_NAME"] = 0;
    //        //aRow["IN_STOCK_SALES_MKDN_VAR_NAME"] = 0;
    //        //aRow["ACCUM_SELL_THRU_SALES_VAR_NAME"] = 0;
    //        //aRow["ACCUM_SELL_THRU_STOCK_VAR_NAME"] = 0;
    //        //aRow["DAYS_IN_STOCK_VAR_NAME"] = 0;
    //        //aRow["RECEIVED_STOCK_VAR_NAME"] = 0;
    //        dt.Rows.Add(aRow);
    //    }

    //    private void StoreSizeWeekUpdate(DataTable dt, int store, int size,
    //        int inStockSales,
    //        int inStockSalesReg,
    //        int inStockSalesPromo,
    //        int inStockSalesMkdn,
    //        int accumSellThruSales,
    //        int accumSellThruStock,
    //        int daysInStock)
    //    {
    //        object[] keys = new object[] { store, size };
    //        DataRow aRow = dt.Rows.Find(keys);
    //        if (aRow == null)
    //        {
    //            StoreSizeWeekInsert(dt, store, size, 0, 0, 0, 0, 0, 0, 0, false);
    //            aRow = dt.Rows.Find(keys);
    //        }

    //        aRow[_dcInStockSales] = inStockSales;
    //        aRow[_dcInStockSalesReg] = inStockSalesReg;
    //        aRow[_dcInStockSalesPromo] = inStockSalesPromo;
    //        aRow[_dcInStockSalesMkdn] = inStockSalesMkdn;
    //        aRow[_dcAccumSellThruSales] = accumSellThruSales;
    //        aRow[_dcAccumSellThruStock] = accumSellThruStock;
    //        aRow[_dcDaysInStock] = daysInStock;

    //        //aRow[IN_STOCK_SALES_VAR_NAME] = inStockSales;
    //        //aRow[IN_STOCK_SALES_REG_VAR_NAME] = inStockSalesReg;
    //        //aRow[IN_STOCK_SALES_PROMO_VAR_NAME] = inStockSalesPromo;
    //        //aRow[IN_STOCK_SALES_MKDN_VAR_NAME] = inStockSalesMkdn;
    //        //aRow[ACCUM_SELL_THRU_SALES_VAR_NAME] = accumSellThruSales;
    //        //aRow[ACCUM_SELL_THRU_STOCK_VAR_NAME] = accumSellThruStock;
    //        //aRow[DAYS_IN_STOCK_VAR_NAME] = daysInStock;


    //    }

    //    private void LogCache(int style, int color, int storeIdx, List<SizeTimeCollection> sizeList)
    //    {
    //        LogMessage("      STORE " + _storeIdDictionary[storeIdx] + "(" + _storeRidDictionary[storeIdx].ToString() + ")");
    //        foreach (SizeTimeCollection svtv in sizeList)
    //        {
    //            LogCache(style, color, storeIdx, svtv);
    //        }
    //    }

    //    private void LogCache(int style, int color, int storeIdx, SizeTimeCollection svtv)
    //    {
    //        LogMessage("        Style/ColorCode/SizeCode: " + _styleId + "(" + style + ")" + "/" + _colorCodeId + "(" + color + ")" + "/" + _sizeCodeHash[svtv.SizeCodeRID] + "(" + svtv.SizeCodeRID + ")");

    //        if (svtv.AllZeroes)
    //        {
    //            LogMessage("          *All variables have zero values*");
    //        }
    //        else
    //        {
    //            LogMessage("          Sales Time/Value ");
    //            StringBuilder salesLine = new StringBuilder("          ");
    //            foreach (var pair in svtv.SalesByTimeList)
    //            {
    //                salesLine.Append(" " + pair.Key + " / " + pair.Value);
    //            }
    //            LogMessage(salesLine.ToString());
    //            LogMessage("          Sales Reg Time/Value ");
    //            salesLine = new StringBuilder("          ");
    //            foreach (var pair in svtv.SalesRegByTimeList)
    //            {
    //                salesLine.Append(" " + pair.Key + " / " + pair.Value);
    //            }
    //            LogMessage(salesLine.ToString());
    //            LogMessage("          Sales Promo Time/Value ");
    //            salesLine = new StringBuilder("          ");
    //            foreach (var pair in svtv.SalesPromoByTimeList)
    //            {
    //                salesLine.Append(" " + pair.Key + " / " + pair.Value);
    //            }
    //            LogMessage(salesLine.ToString());
    //            LogMessage("          Sales Mkdn Time/Value ");
    //            salesLine = new StringBuilder("          ");
    //            foreach (var pair in svtv.SalesMkdnByTimeList)
    //            {
    //                salesLine.Append(" " + pair.Key + " / " + pair.Value);
    //            }
    //            LogMessage(salesLine.ToString());
    //            //LogMessage("          Sales Total Time/Value ");
    //            //salesLine = new StringBuilder("          ");
    //            //foreach (var pair in svtv.SalesTotalByTimeList)
    //            //{
    //            //    salesLine.Append(" " + pair.Key + " / " + pair.Value);
    //            //}
    //            //LogMessage(salesLine.ToString());

    //            LogMessage("          Stock Time/Value ");
    //            StringBuilder stockLine = new StringBuilder("          ");
    //            foreach (var pair in svtv.StockByTimeList)
    //            {
    //                stockLine.Append(" " + pair.Key + " / " + pair.Value);
    //            }
    //            LogMessage(stockLine.ToString());
    //            LogMessage("          Stock Reg Time/Value ");
    //            stockLine = new StringBuilder("          ");
    //            foreach (var pair in svtv.StockRegByTimeList)
    //            {
    //                stockLine.Append(" " + pair.Key + " / " + pair.Value);
    //            }
    //            LogMessage(stockLine.ToString());
    //            LogMessage("          Stock Mkdn Time/Value ");
    //            stockLine = new StringBuilder("          ");
    //            foreach (var pair in svtv.StockMkdnByTimeList)
    //            {
    //                stockLine.Append(" " + pair.Key + " / " + pair.Value);
    //            }
    //            LogMessage(stockLine.ToString());
    //            //LogMessage("          Stock Total Time/Value ");
    //            //stockLine = new StringBuilder("          ");
    //            //foreach (var pair in svtv.StockTotalByTimeList)
    //            //{
    //            //    stockLine.Append(" " + pair.Key + " / " + pair.Value);
    //            //}
    //            //LogMessage(stockLine.ToString());


    //        }
    //    }

    //    private void InitDoubleArrays()
    //    {
    //        _dSales = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dSalesReg = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dSalesPromo = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dSalesMkdn = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dStock = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dStockReg = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dStockMkdn = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dInStockSales = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dInStockSalesReg = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dInStockSalesPromo = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dInStockSalesMkdn = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dAccumSellThruSales = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dAccumSellThruStock = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dDaysInStock = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //        _dReceivedStock = new double[MIDStorageTypeInfo.GetStoreMaxRID(0)];
    //    }

    //    private void AccumDoubleArrays(DataRowView aRowView)
    //    {
    //        int storeIdx = int.Parse(aRowView["STORE_IDX"].ToString());
    //        _dSales[storeIdx] += double.Parse(aRowView[SALES_VAR_NAME].ToString());
    //        _dSalesReg[storeIdx] += double.Parse(aRowView[SALES_REG_VAR_NAME].ToString());
    //        _dSalesPromo[storeIdx] += double.Parse(aRowView[SALES_PROMO_VAR_NAME].ToString());
    //        _dSalesMkdn[storeIdx] += double.Parse(aRowView[SALES_MKDN_VAR_NAME].ToString());
    //        _dStock[storeIdx] += double.Parse(aRowView[STOCK_VAR_NAME].ToString());
    //        _dStockReg[storeIdx] += double.Parse(aRowView[STOCK_REG_VAR_NAME].ToString());
    //        _dStockMkdn[storeIdx] += double.Parse(aRowView[STOCK_MKDN_VAR_NAME].ToString());
    //        _dInStockSales[storeIdx] += double.Parse(aRowView[IN_STOCK_SALES_VAR_NAME].ToString());
    //        _dInStockSalesReg[storeIdx] += double.Parse(aRowView[IN_STOCK_SALES_REG_VAR_NAME].ToString());
    //        _dInStockSalesPromo[storeIdx] += double.Parse(aRowView[IN_STOCK_SALES_PROMO_VAR_NAME].ToString());
    //        _dInStockSalesMkdn[storeIdx] += double.Parse(aRowView[IN_STOCK_SALES_MKDN_VAR_NAME].ToString());
    //        _dAccumSellThruSales[storeIdx] += double.Parse(aRowView[ACCUM_SELL_THRU_SALES_VAR_NAME].ToString());
    //        _dAccumSellThruStock[storeIdx] += double.Parse(aRowView[ACCUM_SELL_THRU_STOCK_VAR_NAME].ToString());
    //        _dDaysInStock[storeIdx] += double.Parse(aRowView[DAYS_IN_STOCK_VAR_NAME].ToString());
    //        _dReceivedStock[storeIdx] += double.Parse(aRowView[RECEIVED_STOCK_VAR_NAME].ToString());
    //    }

    //    private void WriteDoubleArrays(int styleRid, int colorCodeRid, int sizeCodeRid, SQL_TimeID timeId, int[] storeRIDs)
    //    {
    //        if (_LOGGING)
    //        {
    //            string msg = timeId.TimeID + "(" + timeId.SqlTimeID + ")" + "Style/ColorCode/SizeCode: " +
    //                _styleId + "(" + styleRid + ")" + " / " + _colorCodeId + "(" + colorCodeRid + ")" + " / " + _sizeCodeHash[sizeCodeRid] + "(" + sizeCodeRid + ")";
    //            LogMessage(msg);
    //            LogDoubleArrays(storeRIDs);
    //        }

    //        bool recWritten = false;
    //        SQL_TimeID weekTimeId = new SQL_TimeID(eSQLTimeIdType.TimeIdIsWeekly, timeId.TimeID);
    //        if (!IsAllZeroes(_dSales))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(SALES_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dSales);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dSalesReg))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(SALES_REG_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dSalesReg);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dSalesMkdn))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(SALES_MKDN_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dSalesMkdn);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dSalesPromo))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(SALES_PROMO_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dSalesPromo);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dStock))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(STOCK_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dStock);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dStockReg))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(STOCK_REG_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dStockReg);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dStockMkdn))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(STOCK_MKDN_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dStockMkdn);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dInStockSales))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(IN_STOCK_SALES_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dInStockSales);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dInStockSalesReg))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(IN_STOCK_SALES_REG_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dInStockSalesReg);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dInStockSalesPromo))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(IN_STOCK_SALES_PROMO_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dInStockSalesPromo);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dInStockSalesMkdn))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(IN_STOCK_SALES_MKDN_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dInStockSalesMkdn);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dAccumSellThruSales))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(ACCUM_SELL_THRU_SALES_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dAccumSellThruSales);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dAccumSellThruStock))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(ACCUM_SELL_THRU_STOCK_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dAccumSellThruStock);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dDaysInStock))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(DAYS_IN_STOCK_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dDaysInStock);
    //            recWritten = true;
    //        }
    //        if (!IsAllZeroes(_dReceivedStock))
    //        {
    //            _dlStoreVarHist.SetStoreVariableWeekValue(RECEIVED_STOCK_VAR_NAME, styleRid, weekTimeId, colorCodeRid, sizeCodeRid, storeRIDs, _dReceivedStock);
    //            recWritten = true;
    //        }

    //        //====================================================================
    //        // All variables are on a single record on the database.
    //        // If we set any of the variables above, a record will be written.
    //        //====================================================================
    //        if (recWritten)
    //        {
    //            _totUpdates++;
    //        }
    //    }

    //    private bool IsAllZeroes(double [] values)
    //    { 
    //        bool isAllZeroes = true;
    //        foreach (double val in values)
    //        {
    //            if (val != 0d)
    //            {
    //                isAllZeroes = false;
    //                break;
    //            }
    //        }
    //        return isAllZeroes;
    //    }

    //    private void LogDoubleArrays(int[] storeRIDs)
    //    {
    //        string msg = "(Note: Any store not listed below had zeros for all values.)";
    //        LogMessage(msg);
    //        StringBuilder sb = new StringBuilder(225);
    //        sb.AppendLine(new String(' ', 225));
    //        sb.Insert(1, "STORE");
    //        sb.Insert(11, IN_STOCK_SALES_VAR_NAME);
    //        sb.Insert(26, IN_STOCK_SALES_REG_VAR_NAME);
    //        sb.Insert(45, IN_STOCK_SALES_PROMO_VAR_NAME);
    //        sb.Insert(66, IN_STOCK_SALES_MKDN_VAR_NAME);
    //        sb.Insert(86, ACCUM_SELL_THRU_SALES_VAR_NAME);
    //        sb.Insert(108, ACCUM_SELL_THRU_STOCK_VAR_NAME);
    //        sb.Insert(130, DAYS_IN_STOCK_VAR_NAME);
    //        sb.Insert(144, RECEIVED_STOCK_VAR_NAME);
    //        sb.Insert(159, SALES_VAR_NAME);
    //        sb.Insert(169, SALES_REG_VAR_NAME);
    //        sb.Insert(179, SALES_MKDN_VAR_NAME);
    //        sb.Insert(191, SALES_PROMO_VAR_NAME);
    //        sb.Insert(203, STOCK_VAR_NAME);
    //        sb.Insert(212, STOCK_REG_VAR_NAME);
    //        sb.Insert(222, STOCK_MKDN_VAR_NAME);
    //        LogMessage(sb.ToString().Trim());


    //        for (int i = 0; i < storeRIDs.Length; i++)
    //        {
    //            if (_dInStockSales[i] == 0 &&
    //                _dInStockSalesReg[i] == 0 &&
    //                _dInStockSalesPromo[i] == 0 &&
    //                _dInStockSalesMkdn[i] == 0 &&
    //                _dAccumSellThruSales[i] == 0 &&
    //                _dAccumSellThruStock[i] == 0 &&
    //                _dDaysInStock[i] == 0 &&
    //                _dReceivedStock[i] == 0 &&
    //                _dSales[i] == 0 &&
    //                _dSalesReg[i] == 0 &&
    //                _dSalesMkdn[i] == 0 &&
    //                _dSalesPromo[i] == 0 &&
    //                _dStock[i] == 0 &&
    //                _dStockReg[i] == 0 &&
    //                _dStockMkdn[i] == 0)
    //            {
    //                // Don't log.
    //            }
    //            else
    //            {
    //                //int s = storeRIDs[i];
    //                sb = new StringBuilder(225);
    //                sb.AppendLine(new String(' ', 225));
    //                sb.Insert(1, _storeIdDictionary[i] + "(" + storeRIDs[i].ToString() + ")");
    //                sb.Insert(11, _dInStockSales[i]);
    //                sb.Insert(26, _dInStockSalesReg[i]);
    //                sb.Insert(45, _dInStockSalesPromo[i]);
    //                sb.Insert(66, _dInStockSalesMkdn[i]);
    //                sb.Insert(86, _dAccumSellThruSales[i]);
    //                sb.Insert(108, _dAccumSellThruStock[i]);
    //                sb.Insert(130, _dDaysInStock[i]);
    //                sb.Insert(144, _dReceivedStock[i]);
    //                sb.Insert(159, _dSales[i]);
    //                sb.Insert(169, _dSalesReg[i]);
    //                sb.Insert(179, _dSalesMkdn[i]);
    //                sb.Insert(191, _dSalesPromo[i]);
    //                sb.Insert(203, _dStock[i]);
    //                sb.Insert(212, _dStockReg[i]);
    //                sb.Insert(222, _dStockMkdn[i]);
    //                LogMessage(sb.ToString().Trim());
    //            }
    //        }
    //        LogMessage(" ");
    //    }

    //    private void LogMessage(string msg)
    //    {
    //        if (_LOGGING)
    //        {
    //            _logMessages.Add(msg);
    //        }
    //    }
    //}

    //public class SizeDayToWeekSummaryProcessEndEventArgs : EventArgs
    //{
    //    private int _styleCnt = 0;
    //    private int _colorCnt = 0;
    //    private int _sizeCnt = 0;
    //    private int _totReads = 0;
    //    private int _totUpdates = 0;
    //    private int _valCnt = 0;
    //    private int _errorCnt = 0;
    //    private SizeDayToWeekSummaryProcess _aProcess;

    //    public SizeDayToWeekSummaryProcessEndEventArgs(SizeDayToWeekSummaryProcess aProcess)
    //    //public SizeDayToWeekSummaryProcessEndEventArgs(int styleCnt, int colorCnt, int sizeCnt, int totReads, int totUpdates, int valCnt, int errorCnt)
    //    {
    //        _aProcess = aProcess;
    //        //_styleCnt = styleCnt;
    //        //_colorCnt = colorCnt;
    //        //_sizeCnt = colorCnt;
    //        //_totReads = totReads;
    //        //_totUpdates = totUpdates;
    //        //_valCnt = valCnt;
    //        //_errorCnt = errorCnt;
			
    //    }

    //    public SizeDayToWeekSummaryProcess SizeDayToWeekSummaryProcess
    //    {
    //        get { return _aProcess; }
    //        set { _aProcess = value; }
    //    }

    //    //public int SytleCnt
    //    //{
    //    //    get { return _styleCnt; }
    //    //    set { _styleCnt = value; }
    //    //}
    //    //public int ColorCnt
    //    //{
    //    //    get { return _colorCnt; }
    //    //    set { _colorCnt = value; }
    //    //}
    //    //public int SizeCnt
    //    //{
    //    //    get { return _sizeCnt; }
    //    //    set { _sizeCnt = value; }
    //    //}
    //    //public int TotReads
    //    //{
    //    //    get { return _totReads; }
    //    //    set { _totReads = value; }
    //    //}
    //    //public int TotUpdates
    //    //{
    //    //    get { return _totUpdates; }
    //    //    set { _totUpdates = value; }
    //    //}
    //    //public int ValCnt
    //    //{
    //    //    get { return _valCnt; }
    //    //    set { _valCnt = value; }
    //    //}
    //    //public int ErrorCnt
    //    //{
    //    //    get { return _errorCnt; }
    //    //    set { _errorCnt = value; }
    //    //}


    //}
}
