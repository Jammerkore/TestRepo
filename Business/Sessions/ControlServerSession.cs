using System;
using System.Diagnostics;
//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
using System.Collections;
//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
using System.Collections.Generic;	// TT#1581-MD - stodd - API Header Reconcile
//Begin TT#708 - JScott - Services need a Retry availalbe.
using System.Threading;
//End TT#708 - JScott - Services need a Retry availalbe.
using System.Data;	// TT#1581-MD - stodd - API Header Reconcile


using MIDRetail.Common;
using MIDRetail.Data;
using MIDRetail.DataCommon;

namespace MIDRetail.Business
{
	/// <summary>
	/// ControlServerGlobal is a static class that contains fields that are global to all ControlServerSession objects.
	/// </summary>
	/// <remarks>
	/// The ControlServerGlobal class is used to store information that is global to all ControlServerSession objects
	/// within the process.  A common use for this class would be to cache static information from the database in order to
	/// reduce accesses to the database.
	/// </remarks>

	public class ControlServerGlobal : Global
	{
		//=======
		// FIELDS
		//=======

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

		static private ArrayList _loadLock;
		static private bool _loaded;
		static private Audit _audit;

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private ServerList _clientServerList;
		static private ServerList _storeServerList;
		static private ServerList _hierarchyServerList;
		static private ServerList _applicationServerList;
		static private ServerList _schedulerServerList;
		static private ServerList _headerServerList;

        // Begin Track #6346 - JSmith - Duplicate rows after Rollup
        static private ArrayList _sessionIDLock;
        static private int _sessionID = 0;
        // End Track #6346

		// Begin TT#1581-MD - stodd - API Header Reconcile
        //static private Dictionary<eProcesses, bool> _processState;
        static private ProcessStateManager _processStateManager;
        static private Dictionary<eProcesses, List<ProcessControlRule>> _processRules;
        static private ArrayList _permissionLock;
        // End TT#1581-MD - stodd - API Header Reconcile

        // Begin TT#2131-MD - JSmith - Halo Integration
        static private string _ROExtractConnectionString;
        static private Dictionary<long, ExtractSessionEntry> _extractSessions;
        static private ArrayList _extractLock;
        // End TT#2131-MD - JSmith - Halo Integration


        //=============
        // CONSTRUCTORS
        //=============

        /// <summary>
        /// Creates a new instance of ControlServerGlobal
        /// </summary>

        static ControlServerGlobal()
		{
			try
			{
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				_loadLock = new ArrayList();
				_loaded = false;
                // Begin Track #6346 - JSmith - Duplicate rows after Rollup
                _sessionIDLock = new ArrayList();
                // End Track #6346
				// Begin TT#1581-MD - stodd - API Header Reconcile
                _permissionLock = new ArrayList();
                _processStateManager = new ProcessStateManager();
                // End TT#1581-MD - stodd - API Header Reconcile
                _extractLock = new ArrayList();  // TT#2131-MD - JSmith - Halo Integration

                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                if (!EventLog.SourceExists("MIDControlService"))
				{
					EventLog.CreateEventSource("MIDControlService", null);
				}
				try
				{
				}
				catch (Exception ex)
				{
					EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
				}

				_clientServerList = new ServerList(MIDConfigurationManager.AppSettings["ClientServers"]);
				_storeServerList = new ServerList(MIDConfigurationManager.AppSettings["StoreServers"]);
				_hierarchyServerList = new ServerList(MIDConfigurationManager.AppSettings["HierarchyServers"]);
				_applicationServerList = new ServerList(MIDConfigurationManager.AppSettings["ApplicationServers"]);
				_schedulerServerList = new ServerList(MIDConfigurationManager.AppSettings["SchedulerServer"]);
				_headerServerList = new ServerList(MIDConfigurationManager.AppSettings["HeaderServers"]);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		static private Audit Audit
		{
			get
			{
				return _audit;
			}
		}

		static public bool Loaded
		{
			get
			{
				return _loaded;
			}
		}

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		/// <summary>
		/// Gets the ClientServerList object
		/// </summary>

		static public ServerList ClientServerList
		{
			get
			{
				return _clientServerList;
			}
		}

		/// <summary>
		/// Gets the StoreServerList object
		/// </summary>

		static public ServerList StoreServerList
		{
			get
			{
				return _storeServerList;
			}
		}

		/// <summary>
		/// Gets the HierarchyServerList object
		/// </summary>

		static public ServerList HierarchyServerList
		{
			get
			{
				return _hierarchyServerList;
			}
		}

		/// <summary>
		/// Gets the ApplicationServerList object
		/// </summary>

		static public ServerList ApplicationServerList
		{
			get
			{
				return _applicationServerList;
			}
		}

		/// <summary>
		/// Gets the SchedulerServerList object
		/// </summary>

		static public ServerList SchedulerServerList
		{
			get
			{
				return _schedulerServerList;
			}
		}

		/// <summary>
		/// Gets the HeaderServerList object
		/// </summary>

		static public ServerList HeaderServerList
		{
			get
			{
				return _headerServerList;
			}
		}

		// Begin TT#1581-MD - stodd - API Header Reconcile
        static public ProcessStateManager ProcessStateManager
        {
            get
            {
                return _processStateManager;
            }
        }

        static public Dictionary<eProcesses, List<ProcessControlRule>> ProcessRules
        {
            get
            {
                return _processRules;
            }
        }
        // End TT#1581-MD - stodd - API Header Reconcile

        //========
        // METHODS
        //========

        /// <summary>
        /// The Load method is called by the service or client to trigger the instantiation of the static ControlServerGlobal
        /// object.
        /// </summary>

        static public void Load(bool aLocalCtrlServer)
		{
			try
			{
		
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
				lock (_loadLock.SyncRoot)
				{
		
					if (!_loaded)
					{
		
				//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
						// BEGIN MID Track #4373 - John Smith - Add FISCAL_WEEKS table
						if (MIDConnectionString.ConnectionString == null ||
							MIDConnectionString.ConnectionString.Length == 0)
						{
							MIDConnectionString.ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];
						}
						// END MID Track #4373

						MarkRunningProcesses();
		
						// BEGIN MID Track #4572 - John Smith - clear enqueues on global start
						if (!aLocalCtrlServer)
						{
							DeleteAllEnqueues();
						}
						// END MID Track #4572
		
                    // BEGIN TT187 - Truncate Virtual Locks RBeck
                        if (!aLocalCtrlServer)
                        {
                            DeleteAllVirtualLocks();
                        }
                    // END TT187
		
						//Begin TT#1206 - JScott - Unable to Assign a User to Another User
						if (!aLocalCtrlServer)
						{
							CloseAllUserSessions();
						}
		
						//End TT#1206 - JScott - Unable to Assign a User to Another User
						//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
						//LoadBase(eProcesses.controlService);
                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                        //_audit = new Audit(eProcesses.controlService, Include.AdministratorUserRID);
                        if (!aLocalCtrlServer)
                        {
                            _audit = new Audit(eProcesses.controlService, Include.AdministratorUserRID);
                        }
                        // End TT#189

                        

                        // Begin TT#2307 - JSmith - Incorrect Stock Values
                        int messagingInterval = Include.Undefined;
                        object parm = MIDConfigurationManager.AppSettings["MessagingInterval"];
                        if (parm != null)
                        {
                            messagingInterval = Convert.ToInt32(parm);
                        }
                        //LoadBase();
		
                        LoadBase(eMIDMessageSenderRecepient.controlService, messagingInterval, aLocalCtrlServer, eProcesses.controlService);
		
                        // End TT#2307;
						//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
						// BEGIN MID Track #4373 - John Smith - Add FISCAL_WEEKS table
						BuildFiscalWeekDates();
						// END MID Track #4373
				//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                        // Begin TT#739-MD - JSmith - Delete Stores
                        // Uncomment if using SQLBulkCopy
                        //Header headerData = new Header();
                        //headerData.CleanupAllWorkTables();
                        // End TT#739-MD - JSmith - Delete Stores
		
                        // Begin TT#195 MD - JSmith - Add environment authentication
                        if (!aLocalCtrlServer)
                        {
                            RegisterServiceStart();
                        }
                        // End TT#195 MD
                       

                        if (!aLocalCtrlServer)
                        {
                            LoadSocketServerManager(_audit);
                        }

                        //Begin TT#1517-MD -jsobek -Store Service Optimization
                        if (!aLocalCtrlServer)
                        {
                            StoreGroupMaint groupData = new StoreGroupMaint();
                            try
                            {
                                groupData.OpenUpdateConnection();
                                groupData.StoreGroupJoinHistory_DeleteAll();
                                groupData.CommitData();
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                            finally
                            {
                                groupData.CloseUpdateConnection();
                            }
                        }
                        //End TT#1517-MD -jsobek -Store Service Optimization

                        // Begin TT#2131-MD - JSmith - Halo Integration
                        _ROExtractConnectionString = MIDConfigurationManager.AppSettings["ROExtractConnectionString"];
                        CloseAllExtractSessions();
                        _extractSessions = new Dictionary<long, ExtractSessionEntry>();
                        // End TT#2131-MD - JSmith - Halo Integration

                        // TESTING OINLY
                        //string msg = "";
                        //GetProcessingPermission(eProcesses.headerLoad, 10001, ref msg);
                        _loaded = true;
					}
				}
				//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
			}
		}

		//Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		/// <summary>
		/// Cleans up all resources for the service
		/// </summary>

		static public void CleanUp()
		{
			try
			{
                // Begin TT#2307 - JSmith - Incorrect Stock Values
                if (isExecutingLocal &&
                    MessageProcessor.isListeningForMessages)
                {
                    StopMessageListener();
                }
                // End TT#2307

                StopSocketServerManager();

				if (Audit != null)
				{
					Audit.UpdateHeader(eProcessCompletionStatus.Successful, eMIDTextCode.sum_Successful, "", Audit.HighestMessageLevel);
                    // Begin TT#1243 - JSmith - Audit Performance
                    Audit.CloseUpdateConnection();
                    // End TT#1243
				}
			}
			catch (Exception ex)
			{
				if (Audit != null)
				{
					Audit.Log_Exception(ex);
				}
			}
		}

  

        // Begin TT#1243 - JSmith - Audit Performance
        /// <summary>
        /// Clean up the global resources
        /// </summary>
        static public void CloseAudit()
        {
            try
            {
                Audit.CloseUpdateConnection();
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

		//End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
		// BEGIN MID Track #4373 - John Smith - Add FISCAL_WEEKS table
		static private void BuildFiscalWeekDates()
		{
			CalendarData cd = new CalendarData();
			// remove dates
			try
			{
				cd.OpenUpdateConnection();
				cd.DeleteAllFiscalWeeks();
				cd.CommitData();
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
			}
			finally
			{
				cd.CloseUpdateConnection();
			}
			// build dates
			try
			{
                // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                int rowSequence = 0;
                // End TT#988
				cd.OpenUpdateConnection();
				ProfileList weeks = Calendar.GetWeekRange(Calendar.FirstCalendarFiscalWeek, Calendar.LastCalendarFiscalWeek);
				foreach (WeekProfile wk in weeks)
				{
                    // Begin TT#460 - JSmith - Add size daily blob tables to Purge
                    //cd.AddFiscalWeek(wk.YearWeek, ((DayProfile)wk.Days[0]).Key, ((DayProfile)wk.Days[wk.DaysInWeek - 1]).Key);

                    SQL_TimeID sqlFirstDate = new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, ((DayProfile)wk.Days[0]).Key);
                    SQL_TimeID sqlEndDate = new SQL_TimeID(eSQLTimeIdType.TimeIdIsDaily, ((DayProfile)wk.Days[wk.DaysInWeek - 1]).Key);

                    // Begin TT#988 - JSmith - Add Active Only indicator to Override Low Level Model
                    //cd.AddFiscalWeek(wk.YearWeek, ((DayProfile)wk.Days[0]).Key, ((DayProfile)wk.Days[wk.DaysInWeek - 1]).Key, sqlFirstDate.SqlTimeID, sqlEndDate.SqlTimeID);
                    ++rowSequence;
                    cd.AddFiscalWeek(rowSequence, wk.YearWeek, ((DayProfile)wk.Days[0]).Key, ((DayProfile)wk.Days[wk.DaysInWeek - 1]).Key, sqlFirstDate.SqlTimeID, sqlEndDate.SqlTimeID);
                    // End TT#988
                    // End TT#460
				}

				cd.CommitData();
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
			}
			finally
			{
				cd.CloseUpdateConnection();
			}
		}
		// END MID Track #4373

		static private void MarkRunningProcesses()
		{
			AuditData auditData = new AuditData();
			// remove dates
			try
			{
				auditData.OpenUpdateConnection();
				auditData.MarkAllRunningAsUnexpectedTermination();
				auditData.CommitData();
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
				throw new MIDException (eErrorLevel.severe,	0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
			}
			finally
			{
				auditData.CloseUpdateConnection();
			}
		}

        // BEGIN MID Track #4572 - John Smith - clear enqueues on global start
        static private void DeleteAllEnqueues()
        {
            MIDEnqueue enqueueData = new MIDEnqueue();
            try
            {
                enqueueData.OpenUpdateConnection();
                enqueueData.DeleteAllEnqueues();
                enqueueData.CommitData();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
            finally
            {
                enqueueData.CloseUpdateConnection();
            }
        }
        // END MID Track #4572

    // BEGIN TT#187 - Truncate Virtual Locks RBeck
        //Note: MIDEnqueue inherits the methods needed for db connection from DataLayer
        //      This is Not so for class ResourceLock and the simplest technique was
        //      to put the DeleteAllVirtualLocks method in MIDEnqueue.
        static private void DeleteAllVirtualLocks()
        {
            MIDEnqueue resourceData = new MIDEnqueue();
            try
            {
                resourceData.OpenUpdateConnection();
                resourceData.DeleteAllVirtualLocks();
                resourceData.CommitData();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
            finally
            {
                resourceData.CloseUpdateConnection();
            }
        }

		// Begin TT#1581-MD - stodd - API Header Reconcile
        /// <summary>
        /// Reads the process control rules from the database for a single process.
        /// </summary>
        /// <param name="aProcess"></param>
        /// <returns></returns>
        static private DataTable GetProcessControlRulesFromDB(eProcesses aProcess)
        {
            SystemData sd = new SystemData();
            DataTable dt = null;
            
            // remove dates
            try
            {
                sd.OpenUpdateConnection();
                dt = sd.GetAPIProcessControlRules((int)aProcess);
                sd.CommitData();
                return dt;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
            finally
            {
                sd.CloseUpdateConnection();
            }
        }

        /// <summary>
        /// determines if the process can execute based upon the rules defined.
        /// </summary>
        /// <param name="aProcess"></param>
        /// <returns></returns>
        static public bool GetProcessingPermission(eProcesses aProcess, int aProcessId, ref string reason)
        {
            DataTable dt = null;
            bool permissionGranted = true;

            try
            {
                lock (_permissionLock.SyncRoot)
                {
                    GetProcessingRules(aProcess);
                    permissionGranted = ValidateProcessAgainstRules(aProcess, aProcessId, ref reason);
                    return permissionGranted;
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
            finally
            {
            }
        }

        static public List<ProcessControlRule> GetProcessingRules(eProcesses aProcess)
        {
            DataTable dt = null;

            try
            {
                lock (_permissionLock.SyncRoot)
                {
                    if (_processRules == null)
                    {
                        _processRules = new Dictionary<eProcesses,List<ProcessControlRule>>();
                    }
                    if (!_processRules.ContainsKey(aProcess))
                    {
                        dt = GetProcessControlRulesFromDB(aProcess);
                        // No rules defined, let process run.
                        if (dt.Rows.Count != 0)
                        {
                            AddProcessingRules(dt);
                        }
                    }

                    if (_processRules.ContainsKey(aProcess))
                    {
                        return _processRules[aProcess];
                    }
                    else
                    { 
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Takes the processing rules datatable and concerts it into a list keyed on by the process.
        /// </summary>
        /// <param name="dt"></param>
        static private void AddProcessingRules(DataTable dt)
        {
            List<ProcessControlRule> ruleList = new List<ProcessControlRule>();
            int API_ID = -1;
            try
            {
                foreach (DataRow aRow in dt.Rows)
                {
                    API_ID = int.Parse(aRow["API_ID"].ToString());
                    bool processMustBeRunning = false;
                    if (aRow["PROCESS_MUST_BE_RUNNING_IND"] != DBNull.Value)
                    {
                        processMustBeRunning = Include.ConvertCharToBool(Convert.ToChar(aRow["PROCESS_MUST_BE_RUNNING_IND"]));
                    }
                    bool processCannotBeRunning = false;
                    if (aRow["PROCESS_CANNOT_BE_RUNNING_IND"] != DBNull.Value)
                    {
                        processCannotBeRunning = Include.ConvertCharToBool(Convert.ToChar(aRow["PROCESS_CANNOT_BE_RUNNING_IND"]));
                    }
                    int processID = int.Parse(aRow["PROCESS_ID"].ToString());
                    DateTime lastModifiedDatTime = Include.UndefinedDate;
                    if (aRow["LAST_MODIFIED_DATETIME"] != DBNull.Value)
                    {
                        lastModifiedDatTime = Convert.ToDateTime(aRow["LAST_MODIFIED_DATETIME"]);
                    }
                    string lastModifiedBy = string.Empty;
                    if (aRow["LAST_MODIFIED_BY"] != DBNull.Value)
                    {
                        lastModifiedBy = Convert.ToString(aRow["LAST_MODIFIED_BY"]);
                    }

                    ProcessControlRule aRule = new ProcessControlRule(API_ID, processMustBeRunning, processCannotBeRunning, processID, lastModifiedDatTime, lastModifiedBy);
                    ruleList.Add(aRule);
                }

                if (_processRules == null)
                {
                    _processRules = new Dictionary<eProcesses, List<ProcessControlRule>>();
                }
                _processRules.Add((eProcesses)API_ID, ruleList);
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Validates against the rules defined if the process can execute.
        /// </summary>
        /// <param name="aProcess"></param>
        /// <returns></returns>
        static private bool ValidateProcessAgainstRules(eProcesses aProcess, int aProcessId, ref string reason)
        {
            bool permissionGranted = true;
            List<ProcessControlRule> processRules = null;
            List<string> processErrors = new List<string>();
            string aProcessText = MIDText.GetTextOnly((int)aProcess);

            try
            {
                if (_processRules.ContainsKey(aProcess))
                {
                    processRules = _processRules[aProcess];
                }
                if (processRules == null)
                {
                    permissionGranted = true;
                }
                else
                {
                    foreach (ProcessControlRule pcr in processRules)
                    {
                        if (IsProcessRunning((eProcesses)pcr.ProcessID))
                        {
                            if (pcr.CannotBeRunning)
                            {
                                permissionGranted = false;
                                string otherProcessText = MIDText.GetTextOnly(pcr.ProcessID);
                                processErrors.Add(otherProcessText + " is currently executing.");
                            }
                        }
                        else // Process is not running
                        {
                            if (pcr.MustBeRunning)
                            {
                                permissionGranted = false;
                                string otherProcessText = MIDText.GetTextOnly(pcr.ProcessID);
                                processErrors.Add(otherProcessText + " is currently NOT executing.");
                            }
                        }
                    }
                }

                // Either set precess as started or record why it couldn't start
                if (permissionGranted)
                {
                    reason = string.Empty;
                    SetProcessState(aProcess, aProcessId, true);
                }
                else
                {
                    string erroMsg = aProcessText + " could not start for the following reasons: " + Environment.NewLine;
                    foreach (string processErr in processErrors)
                    {
                        erroMsg += "  " + processErr + Environment.NewLine;
                    }
                    reason = erroMsg;
                    EventLog.WriteEntry("MIDControlService", erroMsg, EventLogEntryType.Error);
                    if (Audit != null)
                    {
                        Audit.Add_Msg(eMIDMessageLevel.ProcessUnavailable, "Processing terminated due to permission to process was denied.", "Control Service");
                        Audit.Add_Msg(eMIDMessageLevel.ProcessUnavailable, reason, "Control Service");
                    }
                }
                return permissionGranted;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
            finally
            {
            }
        }


        /// <summary>
        /// Checks against the ProcessStat dictionary List and returns if process is running.
        /// </summary>
        /// <param name="aProcess"></param>
        /// <returns></returns>
        static private bool IsProcessRunning(eProcesses aProcess)
        {
            bool isRunning = false;
            try
            {
                if (_processStateManager.ContainsFirstKey(aProcess))
                {
                    isRunning = _processStateManager.IsProcessRunning(aProcess);
                }
                return isRunning;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
            finally
            {
            }
        }

        /// <summary>
        /// Updates the Process State list to say whether a process is running or not.
        /// </summary>
        /// <param name="aProcess"></param>
        /// <param name="isRunning"></param>
        static public void SetProcessState(eProcesses aProcess, int aProcessId, bool isRunning)
        {
            try
            {
                lock (_permissionLock.SyncRoot)
                {

                    if (_processStateManager.ContainsKey(aProcess, aProcessId))
                    {
                        if (isRunning)
                        {
                            _processStateManager[aProcess, aProcessId] = isRunning;
                        }
                        else
                        {
                            _processStateManager.RemoveProcess(aProcess, aProcessId);
                        }
                    }
                    else if (isRunning)
                    {
                        _processStateManager.AddProcess(aProcess, aProcessId, isRunning);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
            finally
            {
            }
        }


        static public void DumpProcessRules()
        {
            Debug.WriteLine("DumpProcessRules");
            foreach (var processRules in _processRules)
            {
                var key = processRules.Key;
                var value = processRules.Value;
                string processText = MIDText.GetTextOnly((int)processRules.Key);
                Debug.WriteLine("  Rules for " + processText);
                foreach (ProcessControlRule pcr in value)
                {
                    string otherProcessText = MIDText.GetTextOnly(pcr.ProcessID);
                    if (pcr.MustBeRunning)
                    {
                        Debug.WriteLine("    " + otherProcessText + " MUST be running.");
                    }
                    if (pcr.CannotBeRunning)
                    {
                        Debug.WriteLine("    " + otherProcessText + " CANNOT be running.");
                    }
                }
            }
        }
        // End TT#1581-MD - stodd - API Header Reconcile


       

        // BEGIN TT#187 - Truncate Virtual Locks RBeck

        // Begin Track #6346 - JSmith - Duplicate rows after Rollup
        static public int GetSessionID()
        {
            lock (_sessionIDLock.SyncRoot)
            {
                ++_sessionID;
            }
            return _sessionID;
        }
        // End Track #6346
		//Begin TT#1206 - JScott - Unable to Assign a User to Another User

		static public void CloseAllUserSessions()
		{
			SecurityAdmin secAdminData = new SecurityAdmin();

			try
			{
				secAdminData.OpenUpdateConnection();
				secAdminData.CloseAllUserSessions();
				secAdminData.CommitData();
			}
			catch (Exception ex)
			{
				EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
				throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
			}
			finally
			{
				secAdminData.CloseUpdateConnection();
			}
		}
        //End TT#1206 - JScott - Unable to Assign a User to Another User

        // Begin TT#2131-MD - JSmith - Halo Integration
        static public string GetROExtractConnectionString()
        {
            try
            {
                return _ROExtractConnectionString;
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
            finally
            {
            }
        }


        static public long OpenExtractSession()
        {
            long processId = Include.Undefined;

            try
            {
                if (!ROExtractEnabled)
                {
                    return Include.Undefined;
                }

                lock (_extractLock.SyncRoot)
                {
                    processId = DateTime.Now.Ticks;
                    ExtractSessionEntry ese = new ExtractSessionEntry(processId, true);
                    ROExtractData ROExtractData = new ROExtractData(_ROExtractConnectionString);

                    try
                    {
                        ROExtractData.OpenUpdateConnection();
                        ROExtractData.Extract_Session_Update(ese.ProcessId, ese.IsRunning, ese.StartDateTime, null);
                        ROExtractData.CommitData();
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                        throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
                    }
                    finally
                    {
                        ROExtractData.CloseUpdateConnection();
                    }
                    _extractSessions.Add(ese.ProcessId, ese);
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
            finally
            {
            }

            return processId;
        }

        static public bool CloseExtractSession(long aExtractProcessId)
        {
            try
            {
                if (!ROExtractEnabled)
                {
                    return true;
                }

                lock (_extractLock.SyncRoot)
                {
                    ExtractSessionEntry ese = null;

                    if (_extractSessions.TryGetValue(aExtractProcessId, out ese))
                    {
                        ROExtractData ROExtractData = new ROExtractData(_ROExtractConnectionString);

                        try
                        {
                            ROExtractData.OpenUpdateConnection();
                            ROExtractData.Extract_Session_Update(ese.ProcessId, false, ese.StartDateTime, DateTime.Now);
                            ROExtractData.CommitData();
                        }
                        catch (Exception ex)
                        {
                            EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                            throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
                        }
                        finally
                        {
                            ROExtractData.CloseUpdateConnection();
                        }
                        _extractSessions.Remove(ese.ProcessId);
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
            finally
            {
            }

            return true;
        }
        
        static public void CloseAllExtractSessions()
        {
            try
            {
                if (!ROExtractEnabled)
                {
                    return;
                }
                ROExtractData ROExtractData = new ROExtractData(_ROExtractConnectionString);
                if (ROExtractData.Extract_Session_Active())
                {
                    try
                    {
                        ROExtractData.OpenUpdateConnection();
                        ROExtractData.CloseAllExtractSessions();
                        ROExtractData.CommitData();
                    }
                    catch (Exception ex)
                    {
                        EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                        throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
                    }
                    finally
                    {
                        ROExtractData.CloseUpdateConnection();
                    }
                }
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDControlService: ControlServerGlobal encountered error - " + ex.Message);
            }
        }
        // End TT#2131-MD - JSmith - Halo Integration
    }

    /// <summary>
    /// ControlServerSession is a class that contains fields, properties, and methods that are available to other sessions
    /// of the system.
    /// </summary>
    /// <remarks>
    /// The ControlServerSession class is the interface to the ControlServer functionality.  All requests for functionality
    /// or information in the ControlServer should be made through methods and properties in this class.
    /// </remarks>

    //Begin TT#708 - JScott - Services need a Retry availalbe.
    //public class ControlServerSession : Session
    public class ControlServerSessionRemote : SessionRemote
	//End TT#708 - JScott - Services need a Retry availalbe.
	{

		//=======
		// FIELDS
		//=======
// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
		private string _connectionString;
// (CSMITH) - END MID Track #3369
        private int _threadID = -1;
        private int _userRID = -1;
        private string _computerName = string.Empty;
        // Begin TT#1581-MD - stodd - Header Reconcile - process control
        private eProcesses _process = eProcesses.unknown;
        private int _processId = -1;
        // End TT#1581-MD - stodd - Header Reconcile - process control

        private HeaderIDGenerator headerIDGenerator = null;

        private long _extractProcessId = Include.Undefined;  // TT#2131-MD - JSmith - Halo Integration


        //=============
        // CONSTRUCTORS
        //=============

        /// <summary>
        /// Creates a new instance of ControlSessionGlobal as either local or remote, depending on the value of aLocal
        /// </summary>
        /// <param name="aLocal">
        /// A boolean that indicates whether this class is being instantiated in the Client or in a remote service.
        /// </param>

        //Begin TT#708 - JScott - Services need a Retry availalbe.
        //public ControlServerSession(bool aLocal)
        public ControlServerSessionRemote(bool aLocal)
		//Begin TT#708 - JScott - Services need a Retry availalbe.
			: base(aLocal)
		{
		}

		//===========
		// PROPERTIES
		//===========
// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
		public string ConnectionString
		{
			get
			{
				return _connectionString;
			}

			set
			{
				_connectionString = value;
			}
		}
        // (CSMITH) - END MID Track #3369

        /// <summary>
        /// Gets the thread ID of the client.
        /// </summary>
        public int ThreadID
        {
            get
            {
                return _threadID;
            }
        }

        // Begin TT#1581-MD - stodd - Header Reconcile - process control
        public eProcesses CurrentProcess
        {
            get
            {
                return _process;
            }

            set
            {
                _process = value;
            }
        }

        public int CurrentProcessID
        {
            get
            {
                return _processId;
            }

            set
            {
                _processId = value;
            }
        }
        // End TT#1581-MD - stodd - Header Reconcile - process control



		//========
		// METHODS
		//========

        /// <summary>
		/// Initializes the session.
		/// </summary>
        public override void Initialize()
        {
         
            SecurityAdmin securityAdmin = new SecurityAdmin();
            try
            {
                if (_userRID > -1)
                {
                    securityAdmin.OpenUpdateConnection();
                    _threadID = securityAdmin.CreateUserSession(_userRID, _computerName);
                    securityAdmin.CommitData();
                    // Begin TT#739-MD - JSmith - Delete Stores
                    MIDConnectionString.ThreadID = _threadID;
                    // End TT#739-MD - JSmith - Delete Stores

                }
                // Begin TT#1808-MD - JSmith - Store Load Error
                ExceptionHandler.Initialize(SessionAddressBlock.ControlServerSession, false);
                // End TT#1808-MD - JSmith - Store Load Error

                CreateAudit();  // TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed

            }
            catch
            {
                throw;
            }
            finally
            {
                securityAdmin.CloseUpdateConnection();
            }
        }

       

        // Begin TT#195 MD - JSmith - Add environment authentication
        public void VerifyEnvironment(ClientProfile aClientProfile)
        {
            try
            {
                ControlServerGlobal.VerifyEnvironment(aClientProfile);
            }
            catch
            {
                throw;
            }
        }
        // End TT#195 MD

        /// <summary>
		/// Initializes the session.
		/// </summary>
        public void Initialize(int aUserRID, string aComputerName)
        {
            try
            {
                _userRID = aUserRID;
                _computerName = aComputerName;
                Initialize();
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
		/// Identifies resources to release as the session expires.
		/// </summary>
        protected override void ExpiredCleanup()
        {
            try
            {
                SetProcessState(CurrentProcess, CurrentProcessID, false);   // TT#1581-MD - stodd Header Reconcile

                // Begin TT#2131-MD - JSmith - Halo Integration
                if (_extractProcessId != Include.Undefined)
                {
                    CloseExtractSession(_extractProcessId);
                }
                // End TT#2131-MD - JSmith - Halo Integration

                // Begin TT#739-MD - JSmith - Delete Stores
                // Uncomment if using SQLBulkCopy
                //Header headerData = new Header();
                //headerData.DropWorkTables();
                // End TT#739-MD - JSmith - Delete Stores

                // Begin TT#1243 - JSmith - Audit Performance
                base.ExpiredCleanup();
                // End TT#1243
                CloseSession();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry("MIDControlService", ex.Message, EventLogEntryType.Error);
            }
        }

        // Begin TT#1243 - JSmith - Audit Performance
        ///// <summary>
        ///// Clean up the global resources
        ///// </summary>
        //public void CloseSession()
        //{
        //    SecurityAdmin securityAdmin = new SecurityAdmin();
        //    try
        //    {
        //        if (_threadID > -1)
        //        {
        //            securityAdmin.OpenUpdateConnection();
        //            securityAdmin.CloseUserSession(_threadID);
        //            securityAdmin.CommitData();
        //        }
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //    finally
        //    {
        //        securityAdmin.CloseUpdateConnection();
        //    }
        //}
        /// <summary>
        /// Clean up the global resources
        /// </summary>
        public override void CloseSession()
        {
            SecurityAdmin securityAdmin = new SecurityAdmin();
            try
            {
                SetProcessState(CurrentProcess, CurrentProcessID, false);	// TT#1581-MD - stodd Header Reconcile

                base.CloseSession();
                if (_threadID > -1)
                {
                    securityAdmin.OpenUpdateConnection();
                    securityAdmin.CloseUserSession(_threadID);
                    securityAdmin.CommitData();
                }
              
            }
            catch
            {
                throw;
            }
            finally
            {
                securityAdmin.CloseUpdateConnection();
            }


        }

        /// <summary>
        /// Flush buffer and close audit
        /// </summary>
        public override void CloseAudit()
        {
            try
            {
                base.CloseAudit();
                ControlServerGlobal.CloseAudit();
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243


		/// <summary>
		/// Clean up the global resources
		/// </summary>
		public void CleanUpGlobal()
		{
			ControlServerGlobal.CleanUp();
		}
        //Begin TT#901-MD -jsobek -Batch Only Mode

		// Begin TT#1581-MD - stodd - API Header Reconcile
        public bool GetProcessingPermission(eProcesses aProcess, int aProcessId, ref string reason)
        {
            return ControlServerGlobal.GetProcessingPermission(aProcess, aProcessId, ref reason);
        }

        public void SetProcessState(eProcesses aProcess, int aProcessId, bool isRunning)
        {
            ControlServerGlobal.SetProcessState(aProcess, aProcessId, isRunning);
        }

        public void DumpRunningProcesses()
        {
            ControlServerGlobal.ProcessStateManager.DumpRunningProcesses();
        }

        public void DumpProcessRules()
        {
            ControlServerGlobal.DumpProcessRules();
        }

        public List<ProcessStateEntry> GetRunningProcesses()
        {
            return ControlServerGlobal.ProcessStateManager.GetRunningProcesses();
        }

        public List<ProcessControlRule> GetProcessingRules(int aProcess)
        {
            return ControlServerGlobal.GetProcessingRules((eProcesses)aProcess);
        }

		// End TT#1581-MD - stodd - API Header Reconcile
		
        public void GetSocketSettingsFromConfigFile(out string controlServerName, out int controlServerPort, out double clientTimerIntervalInMilliseconds, out double serverTimerIntervalInMilliseconds)
        {
            ControlServerGlobal.GetSocketSettingsFromConfigFile(out controlServerName, out controlServerPort, out clientTimerIntervalInMilliseconds, out serverTimerIntervalInMilliseconds);
        }
        public bool IsApplicationInBatchOnlyMode()
        {
            return ControlServerGlobal.IsApplicationInBatchOnlyMode();
        }
        public string BatchModeLastChangedBy()
        {
            return ControlServerGlobal.BatchModeLastChangedBy();
        }
        //End TT#901-MD -jsobek -Batch Only Mode

		/// <summary>
		/// GetServers calls the Control Server to assign server names for each server type.
		/// </summary>
		/// <returns>
		/// The list of server names for each server type.
		/// </returns>

		//Begin TT#1165 - JScott - Login Performance
		//public ControlServerServerGroup GetServers()
		public ControlServerServerGroup GetServers(
			bool aLocalClientServer, 
			bool aLocalStoreServer, 
			bool aLocalHierarchyServer, 
			bool aLocalApplicationServer, 
			bool aLocalSchedulerServer, 
			bool aLocalHeaderServer)
		//End TT#1165 - JScott - Login Performance
		{
			//Begin TT#1165 - JScott - Login Performance
			//ControlServerServerGroup controlServerServerGroup;
			string clientServer = string.Empty;
			string storeServer = string.Empty;
			string hierarchyServer = string.Empty;
			string applicationServer = string.Empty;
			string schedulerServer = string.Empty;
			string headerServer = string.Empty;
			//End TT#1165 - JScott - Login Performance

			try
			{
				//Begin TT#1165 - JScott - Login Performance
				//controlServerServerGroup = new ControlServerServerGroup(
				//    ControlServerGlobal.ClientServerList.GetServer(eServerType.Application),
				//    ControlServerGlobal.StoreServerList.GetServer(eServerType.Store),
				//    ControlServerGlobal.HierarchyServerList.GetServer(eServerType.Hierarchy),
				//    ControlServerGlobal.ApplicationServerList.GetServer(eServerType.Application),
				//    ControlServerGlobal.SchedulerServerList.GetServer(eServerType.Scheduler),
				//    ControlServerGlobal.HeaderServerList.GetServer(eServerType.Header));

				//return controlServerServerGroup;
				if (!aLocalClientServer)
				{
					clientServer = ControlServerGlobal.ClientServerList.GetServer(eServerType.Client);
				}

				if (!aLocalStoreServer)
				{
					storeServer = ControlServerGlobal.StoreServerList.GetServer(eServerType.Store);
				}

				if (!aLocalHierarchyServer)
				{
					hierarchyServer = ControlServerGlobal.HierarchyServerList.GetServer(eServerType.Hierarchy);
				}

				if (!aLocalApplicationServer)
				{
					applicationServer = ControlServerGlobal.ApplicationServerList.GetServer(eServerType.Application);
				}

				if (!aLocalSchedulerServer)
				{
					schedulerServer = ControlServerGlobal.SchedulerServerList.GetServer(eServerType.Scheduler);
				}

				if (!aLocalHeaderServer)
				{
					headerServer = ControlServerGlobal.HeaderServerList.GetServer(eServerType.Header);
				}

				return new ControlServerServerGroup(clientServer, storeServer, hierarchyServer, applicationServer, schedulerServer, headerServer);
				//End TT#1165 - JScott - Login Performance
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

// (CSMITH) - BEG MID Track #3369: DB Connection String hardcoded in config files
		/// <summary>
		/// GetConnectionString returns database connection string
		/// </summary>
		/// <returns>
		/// The database connection string for the control server
		/// </returns>
		public string GetConnectionString()
		{
			try
			{
				if (ConnectionString == null)
				{
					ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];
				}

				return ConnectionString;
			}

			catch (Exception Ex)
			{
				string exceptMsg = Ex.ToString();

				throw;
			}
		}

        // Begin TT#2131-MD - JSmith - Halo Integration
        /// <summary>
		/// GetROExtractConnectionString returns RO Export database connection string
		/// </summary>
		/// <returns>
		/// The RO Export database connection string for the control server
		/// </returns>
		public string GetROExtractConnectionString()
        {
            try
            {
                return ControlServerGlobal.GetROExtractConnectionString();
            }

            catch (Exception Ex)
            {
                string exceptMsg = Ex.ToString();

                throw;
            }
        }

        public long OpenExtractSession()
        {
            _extractProcessId = ControlServerGlobal.OpenExtractSession();
            return _extractProcessId;
        }

        public bool CloseExtractSession(long aExtractProcessId)
        {
            bool extractSessionClosed = false;
            
            extractSessionClosed = ControlServerGlobal.CloseExtractSession(aExtractProcessId);
            if (extractSessionClosed)
            {
                _extractProcessId = Include.Undefined;
            }
            return extractSessionClosed;
        }
        // End TT#2131-MD - JSmith - Halo Integration

        // Begin Track #6346 - JSmith - Duplicate rows after Rollup
        public int GetSessionID()
        {
            return ControlServerGlobal.GetSessionID();
        }
        // End Track #6346
// (CSMITH) - END MID Track #3369

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                return ControlServerGlobal.ServiceProfile;
            }
            catch
            {
                throw;
            }
        }
        // Begin TT#195 MD

        // Begin TT#1966-MD - JSmith - DC Fulfillment
        public bool LoadHeaderKeys(ref List<string> lHeaderIdKeysList, ref List<string> lMasterHeaderIdKeysList, ref List<string> lHeaderKeysToMatchList, ref string errorMessage)
        {
            errorMessage = string.Empty;
            string sHeaderProcessingKeysFile = MIDConfigurationManager.AppSettings["HeaderProcessingKeysFile"];
            if (sHeaderProcessingKeysFile == null)
            {
                errorMessage = MIDText.GetText(eMIDTextCode.msg_NoFieldSpecified);
                errorMessage = errorMessage.Replace("{0}", "Header Processing Keys File");
                //Audit.Add_Msg(eMIDMessageLevel.Severe, errorMessage, this.GetType().Name);
                return false;
            }
            if (!HeaderKeys.LoadKeys(sHeaderProcessingKeysFile, ref lHeaderKeysToMatchList, ref lHeaderIdKeysList, ref lMasterHeaderIdKeysList, ref errorMessage))
            {
                //Audit.Add_Msg(eMIDMessageLevel.Severe, errorMessage, this.GetType().Name);
                return false;
            }

            return true;
        }

        public eReturnCode GetHeaderId(HeadersHeader hdrTran, int transNo, ref string aHeaderId, ref bool isResetRemove, List<string> headerIdKeyList, List<string> headerKeysToMatchList, int headerIdSequenceLength, string headerIdDelimiter, ref EditMsgs em, bool bLookForMatchingHeader = true)
        {
            if (headerIDGenerator == null)
            {
                headerIDGenerator = new HeaderIDGenerator(SessionAddressBlock);
            }

            // Begin TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
			//return headerIDGenerator.GetHeaderId(hdrTran, transNo, ref aHeaderId, ref isResetRemove, headerIdKeyList, headerKeysToMatchList, headerIdSequenceLength, headerIdDelimiter, ref em, bLookForMatchingHeader);
			return headerIDGenerator.GetHeaderId(SessionAddressBlock.ControlServerSession, hdrTran, transNo, ref aHeaderId, ref isResetRemove, headerIdKeyList, headerKeysToMatchList, headerIdSequenceLength, headerIdDelimiter, ref em, bLookForMatchingHeader);
			// End TT#2057-MD - JSmith - Header Load Error after Header Reconcile is Executed
        }
        // End TT#1966-MD - JSmith - DC Fulfillment
	}




	/// <summary>
	/// ControlServerServerGroup is a class that contains a list of the session servers that are available to the user.
	/// </summary>
	/// <remarks>
	/// The ControlServerServerGroup class is class that is passed back from a call to the GetServers method in the
	/// ControlServerSession class.  This list of servers contain the IP Address and port that should be used for each
	/// session.  An empty string indicates that an available server could not be found and that the client should create
	/// the session locally. 
	/// </remarks>

	[Serializable]
	public class ControlServerServerGroup
	{
		//=======
		// FIELDS
		//=======

		private string _clientServer;
		private string _storeServer;
		private string _hierarchyServer;
		private string _applicationServer;
		private string _schedulerServer;
		private string _headerServer;

		//=============
		// CONSTRUCTORS
		//=============

		/// <summary>
		/// Creates a new instance of ControlServerServerGroup, using the strings containing the server for each session type
		/// </summary>
		/// <param name="aClientServer">
		/// String that contains the Client Session server
		/// </param>
		/// <param name="aStoreServer">
		/// String that contains the Store Session server
		/// </param>
		/// <param name="aHierarchyServer">
		/// String that contains the Hierarchy Session server
		/// </param>
		/// <param name="aApplicationServer">
		/// String that contains the Application Session server
		/// </param>
		/// <param name="aSchedulerServer">
		/// String that contains the Scheduler Session server
		/// </param>
		/// <param name="aHeaderServer">
		/// String that contains the Header Session server
		/// </param>

		public ControlServerServerGroup(string aClientServer, string aStoreServer, string aHierarchyServer, string aApplicationServer, string aSchedulerServer, string aHeaderServer)
		{
			_clientServer = aClientServer;
			_storeServer = aStoreServer;
			_hierarchyServer = aHierarchyServer;
			_applicationServer = aApplicationServer;
			_schedulerServer = aSchedulerServer;
			_headerServer = aHeaderServer;
		}

		//===========
		// PROPERTIES
		//===========

		/// <summary>
		/// Gets the Client Session server
		/// </summary>

		public string ClientServer
		{
			get
			{
				return _clientServer;
			}
		}

		/// <summary>
		/// Gets the Store Session server
		/// </summary>

		public string StoreServer
		{
			get
			{
				return _storeServer;
			}
		}

		/// <summary>
		/// Gets the Hierarchy Session server
		/// </summary>

		public string HierarchyServer
		{
			get
			{
				return _hierarchyServer;
			}
		}

		/// <summary>
		/// Gets the Application Session server
		/// </summary>

		public string ApplicationServer
		{
			get
			{
				return _applicationServer;
			}
		}

		/// <summary>
		/// Gets the Scheduler Session server
		/// </summary>

		public string SchedulerServer
		{
			get
			{
				return _schedulerServer;
			}
		}

		/// <summary>
		/// Gets the Header Session server
		/// </summary>

		public string HeaderServer
		{
			get
			{
				return _headerServer;
			}
		}

		//========
		// METHODS
		//========
	}
	//Begin TT#708 - JScott - Services need a Retry availalbe.

	[Serializable]
	public class ControlServerSession : Session
	{
		//=======
		// FIELDS
		//=======

		//=============
		// CONSTRUCTORS
		//=============

		public ControlServerSession(ControlServerSessionRemote aSessionRemote, int aServiceRetryCount, int aServiceRetryInterval)
			: base(aSessionRemote, eProcesses.controlService, aServiceRetryCount, aServiceRetryInterval)
		{
			try
			{
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//===========
		// PROPERTIES
		//===========

		public string ConnectionString
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return ControlServerSessionRemote.ConnectionString;
						}
						catch (Exception exc)
						{
							if (isServiceRetryException(exc))
							{
								Thread.Sleep(ServiceRetryInterval);
							}
							else
							{
								throw;
							}
						}
					}

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
				}
				catch
				{
					throw;
				}
			}
		}

		public int ThreadID
		{
			get
			{
				try
				{
					for (int i = 0; i < ServiceRetryCount; i++)
					{
						try
						{
							return ControlServerSessionRemote.ThreadID;
						}
						catch (Exception exc)
						{
							if (isServiceRetryException(exc))
							{
								Thread.Sleep(ServiceRetryInterval);
							}
							else
							{
								throw;
							}
						}
					}

					throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
				}
				catch
				{
					throw;
				}
			}
		}

        // Begin TT#1581-MD - stodd - header reconcile
        public eProcesses CurrentProcess
        {
            get
            {
                try
                {
                    for (int i = 0; i < ServiceRetryCount; i++)
                    {
                        try
                        {
                            return ControlServerSessionRemote.CurrentProcess;
                        }
                        catch (Exception exc)
                        {
                            if (isServiceRetryException(exc))
                            {
                                Thread.Sleep(ServiceRetryInterval);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                    throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
                }
                catch
                {
                    throw;
                }
            }

            set
            {
                try
                {
                    for (int i = 0; i < ServiceRetryCount; i++)
                    {
                        try
                        {
                            ControlServerSessionRemote.CurrentProcess = value;
                        }
                        catch (Exception exc)
                        {
                            if (isServiceRetryException(exc))
                            {
                                Thread.Sleep(ServiceRetryInterval);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                    throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
                }
                catch
                {
                    throw;
                }
            }
        }

        public int CurrentProcessID
        {
            get
            {
                try
                {
                    for (int i = 0; i < ServiceRetryCount; i++)
                    {
                        try
                        {
                            return ControlServerSessionRemote.CurrentProcessID;
                        }
                        catch (Exception exc)
                        {
                            if (isServiceRetryException(exc))
                            {
                                Thread.Sleep(ServiceRetryInterval);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                    throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
                }
                catch
                {
                    throw;
                }
            }

            set
            {
                try
                {
                    for (int i = 0; i < ServiceRetryCount; i++)
                    {
                        try
                        {
                            ControlServerSessionRemote.CurrentProcessID = value;
                        }
                        catch (Exception exc)
                        {
                            if (isServiceRetryException(exc))
                            {
                                Thread.Sleep(ServiceRetryInterval);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                    throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
                }
                catch
                {
                    throw;
                }
            }
        }
        // End TT#1581-MD - stodd - header reconcile

		//========
		// METHODS
		//========

		public void Initialize()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						ControlServerSessionRemote.Initialize();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void Initialize(int aUserRID, string aComputerName)
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						ControlServerSessionRemote.Initialize(aUserRID, aComputerName);
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public void CloseSession()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						ControlServerSessionRemote.CloseSession();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#1243 - JSmith - Audit Performance
        public void CloseAudit()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        ControlServerSessionRemote.CloseAudit();
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

		public void CleanUpGlobal()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						ControlServerSessionRemote.CleanUpGlobal();
						return;
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		// Begin TT#1581-MD - stodd - API Header Reconcile
        /// <summary>
        /// Grants permission to execute based upon defined processing rules.
        /// If permission is granted, process will be marked as running in the control service.
        /// </summary>
        /// <param name="aProcess"></param>
        /// <returns></returns>
        public bool GetProcessingPermission(eProcesses aProcess, int aProcessId, ref string reason)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return ControlServerSessionRemote.GetProcessingPermission(aProcess, aProcessId, ref reason);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public void SetProcessState(eProcesses aProcess, int processId, bool isRunning)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        ControlServerSessionRemote.SetProcessState(aProcess, processId, isRunning);
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

		// Begin TT#1581-MD - stodd Header Reconcile
        public void SetCurrentProcess(eProcesses currentProcess)
        {
          
                try
                {
                    for (int i = 0; i < ServiceRetryCount; i++)
                    {
                        try
                        {
                            ControlServerSessionRemote.CurrentProcess = currentProcess;
                            return;
                        }
                        catch (Exception exc)
                        {
                            if (isServiceRetryException(exc))
                            {
                                Thread.Sleep(ServiceRetryInterval);
                            }
                            else
                            {
                                throw;
                            }
                        }
                    }

                    throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
                }
                catch
                {
                    throw;
                }
           
        }

        public void SetCurrentProcessID(int processId)
        {

            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        ControlServerSessionRemote.CurrentProcessID = processId;
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }

        }
		// End TT#1581-MD - stodd Header Reconcile

        public void DumpRunningProcesses()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        ControlServerSessionRemote.DumpRunningProcesses();
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public void DumpProcessRules()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        ControlServerSessionRemote.DumpProcessRules();
                        return;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public List<ProcessStateEntry> GetRunningProcesses()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return ControlServerSessionRemote.GetRunningProcesses();
                        ;
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public List<ProcessControlRule> GetProcessingRules(int aProcess)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return ControlServerSessionRemote.GetProcessingRules(aProcess);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }


		// End TT#1581-MD - stodd - API Header Reconcile



		//Begin TT#1165 - JScott - Login Performance
		//public ControlServerServerGroup GetServers()
		public ControlServerServerGroup GetServers(
			bool aLocalClientServer,
			bool aLocalStoreServer,
			bool aLocalHierarchyServer,
			bool aLocalApplicationServer,
			bool aLocalSchedulerServer,
			bool aLocalHeaderServer)
		//End TT#1165 - JScott - Login Performance
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						//Begin TT#1165 - JScott - Login Performance
						//return ControlServerSessionRemote.GetServers();
						return ControlServerSessionRemote.GetServers(
							aLocalClientServer,
							aLocalStoreServer,
							aLocalHierarchyServer,
							aLocalApplicationServer,
							aLocalSchedulerServer,
							aLocalHeaderServer);
						//End TT#1165 - JScott - Login Performance
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

		public string GetConnectionString()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return ControlServerSessionRemote.GetConnectionString();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#2131-MD - JSmith - Halo Integration
        public string GetROExtractConnectionString()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return ControlServerSessionRemote.GetROExtractConnectionString();
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public long OpenExtractSession()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return ControlServerSessionRemote.OpenExtractSession();
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public bool CloseExtractSession(long aProcessId)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return ControlServerSessionRemote.CloseExtractSession(aProcessId);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#2131-MD - JSmith - Halo Integration

        public int GetSessionID()
		{
			try
			{
				for (int i = 0; i < ServiceRetryCount; i++)
				{
					try
					{
						return ControlServerSessionRemote.GetSessionID();
					}
					catch (Exception exc)
					{
						if (isServiceRetryException(exc))
						{
							Thread.Sleep(ServiceRetryInterval);
						}
						else
						{
							throw;
						}
					}
				}

				throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return ControlServerSessionRemote.GetServiceProfile();
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }

        public void VerifyEnvironment(ClientProfile aClientProfile)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        ControlServerSessionRemote.VerifyEnvironment(aClientProfile);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#195 MD

        // Begin TT#1966-MD - JSmith - DC Fulfillment
        public bool LoadHeaderKeys(ref List<string> lHeaderIdKeysList, ref List<string> lMasterHeaderIdKeysList, ref List<string> lHeaderKeysToMatchList, ref string errorMessage)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return ControlServerSessionRemote.LoadHeaderKeys(ref lHeaderIdKeysList, ref lMasterHeaderIdKeysList, ref lHeaderKeysToMatchList, ref errorMessage);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }


         public eReturnCode GetHeaderId(HeadersHeader hdrTran, int transNo, ref string aHeaderId, ref bool isResetRemove, List<string> headerIdKeyList, List<string> headerKeysToMatchList, int headerIdSequenceLength, string headerIdDelimiter, ref EditMsgs em, bool bLookForMatchingHeader = true)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return ControlServerSessionRemote.GetHeaderId(hdrTran, transNo, ref aHeaderId, ref isResetRemove, headerIdKeyList, headerKeysToMatchList, headerIdSequenceLength, headerIdDelimiter, ref em, bLookForMatchingHeader);
                    }
                    catch (Exception exc)
                    {
                        if (isServiceRetryException(exc))
                        {
                            Thread.Sleep(ServiceRetryInterval);
                        }
                        else
                        {
                            throw;
                        }
                    }
                }

                throw new ServiceUnavailable(MIDText.GetTextOnly((int)SessionType));
            }
            catch
            {
                throw;
            }
        }
        // End TT#1966-MD - JSmith - DC Fulfillment
	}
	//End TT#708 - JScott - Services need a Retry availalbe.

	// Begin TT#1581-MD - stodd - API Header Reconcile
    [Serializable()]
    public class ProcessControlRule
    {
        private int _API_ID;
        private bool _mustBeRunning;
        private bool _cannotBeRunning;
        private int _processID;
        private DateTime _lastModifiedDateTime;
        private string _lastModifiedBy;

        public int API_ID
        {
			get
			{
				return _API_ID;
			}
        }

        public bool MustBeRunning
        {
            get
            {
                return _mustBeRunning;
            }
            set
            {
                _mustBeRunning = value;
            }
        }

        public bool CannotBeRunning
        {
            get
            {
                return _cannotBeRunning;
            }
            set
            {
                _cannotBeRunning = value;
            }
        }

        public int ProcessID
        {
            get
            {
                return _processID;
            }
        }

        public DateTime LastModifiedDateTime
        {
            get
            {
                return _lastModifiedDateTime;
            }
            set
            {
                _lastModifiedDateTime = value;
            }
        }

        public string LastModifiedBy
        {
            get
            {
                return _lastModifiedBy;
            }
            set
            {
                _lastModifiedBy = value;
            }
        }

        public ProcessControlRule(int API_ID, bool mustBeRunning, bool cannotBeRunning, int ProcessID, DateTime lastModifiedDateTime, string lastModifiedBy)
        {
            _API_ID = API_ID;
            _mustBeRunning = mustBeRunning;
            _cannotBeRunning = cannotBeRunning;
            _processID = ProcessID;
            _lastModifiedDateTime = lastModifiedDateTime;
            _lastModifiedBy = lastModifiedBy;
        }
    }

    [Serializable()]
    public class ProcessStateManager
    {
        private Dictionary<eProcesses, Dictionary<int, bool>> _dic = new Dictionary<eProcesses, Dictionary<int, bool>>();

        public ProcessStateManager()
        {
        }

        public bool this[eProcesses key1, int key2]
        {
            get
            {
                return _dic[key1][key2];
            }
            set
            {
                if (!_dic.ContainsKey(key1))
                {
                    _dic[key1] = new Dictionary<int, bool>();
                }

                if (_dic[key1].ContainsKey(key2))
                {
                    _dic[key1][key2] = value;
                }
                else
                {
                    //_dic[key1][key2] = new Dictionary<int, bool>();
                    _dic[key1][key2] = value;
                }
            }
        }

        public bool ContainsKey(eProcesses key1, int key2)
        {
            bool containsKey = false;

            try
            {
                object x = this[key1, key2];
                containsKey = true;
            }
            catch (KeyNotFoundException ex)
            {

            }
            catch
            {
                throw;
            }

            return containsKey;
        }

        public bool ContainsFirstKey(eProcesses key1)
        {
            bool containsKey = false;

            try
            {
                if (_dic.ContainsKey(key1))
                {
                    containsKey = true;
                }
            }
            catch (KeyNotFoundException ex)
            {

            }
            catch
            {
                throw;
            }

            return containsKey;
        }

        public void AddProcess(eProcesses key1, int key2, bool value)
        {

            try
            {
                this[key1, key2] = value;
            }
            catch (KeyNotFoundException ex)
            {

            }
            catch
            {
                throw;
            }
        }

        public void RemoveProcess(eProcesses key1, int key2)
        {

            try
            {
                if (_dic.ContainsKey(key1))
                {
                    if (_dic[key1].ContainsKey(key2))
                    {
                        _dic[key1].Remove(key2);
                        if (_dic[key1].Values.Count == 0)
                        {
                            _dic.Remove(key1);
                        }
                    }
                }

            }
            catch (KeyNotFoundException ex)
            {

            }
            catch
            {
                throw;
            }
        }


        public bool IsProcessRunning(eProcesses key1)
        {
            bool isRunning = false;
            try
            {
                if (_dic.ContainsKey(key1))
                {
                    Dictionary<int, bool> processes = _dic[key1];
                    foreach (KeyValuePair<int, bool> aProcess in processes)
                    {
                        if (aProcess.Value)
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    isRunning = false;
                }
            }
            catch
            {
                throw;
            }

            return isRunning;
        }

        public void DumpRunningProcesses()
        {
            Debug.WriteLine("DumpRunningProcesses");

            foreach (KeyValuePair<eProcesses, Dictionary<int, bool>> aProcess in _dic)
            {
                eProcesses process = aProcess.Key;
                string processText = MIDText.GetTextOnly((int)aProcess.Key);
                foreach (KeyValuePair<int, bool> processMinor in aProcess.Value)
                {
                    Debug.WriteLine("  " + processText + " (" + processMinor.Key + ") is running: " + processMinor.Value);
                }
            }
        }

        public List<ProcessStateEntry> GetRunningProcesses()
        {
            List<ProcessStateEntry> processList = new List<ProcessStateEntry>();

            foreach (KeyValuePair<eProcesses, Dictionary<int, bool>> aProcess in _dic)
            {
                eProcesses process = aProcess.Key;
                foreach (KeyValuePair<int, bool> processMinor in aProcess.Value)
                {
                    processList.Add(new ProcessStateEntry(process, processMinor.Key, processMinor.Value));
                }
            }
            return processList;
        }
    }

    [Serializable()]
    public class ProcessStateEntry
    {
        private eProcesses _process;
        private int _processId;
        private bool _isRunning;

        public ProcessStateEntry(eProcesses process, int processId, bool isRunning)
        {
            _process = process;
            _processId = processId;
            _isRunning = isRunning;
        }

        public eProcesses Process
        {
            get { return _process; }
            set { _process = value; }
        }

        public int ProcessId
        {
            get { return _processId; }
            set { _processId = value; }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }
    }
    // End TT#1581-MD - stodd - API Header Reconcile

    // Begin TT#2131-MD - JSmith - Halo Integration
    [Serializable()]
    public class ExtractSessionEntry
    {
       
        private long _processId;
        private bool _isRunning;
        private DateTime _startDateTime;
        private DateTime _endDateTime;

        public ExtractSessionEntry(long processId, bool isRunning)
        {
           _processId = processId;
            _isRunning = isRunning;
            _startDateTime = DateTime.Now;
        }

        public long ProcessId
        {
            get { return _processId; }
            set { _processId = value; }
        }

        public bool IsRunning
        {
            get { return _isRunning; }
            set { _isRunning = value; }
        }

        public DateTime StartDateTime
        {
            get { return _startDateTime; }
            set { _startDateTime = value; }
        }

        public DateTime EndDateTime
        {
            get { return _endDateTime; }
            set { _endDateTime = value; }
        }
    }
    // End TT#2131-MD - JSmith - Halo Integration
}
