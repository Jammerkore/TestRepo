using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Threading;

using MIDRetail.Data;
using MIDRetail.DataCommon;
using MIDRetail.Common;

namespace MIDRetail.Business
{
    /// <summary>
    /// SchedulerServerGlobal is a static class that contains fields that are global to all SchedulerServerSession objects.
    /// </summary>
    /// <remarks>
    /// The SchedulerServerGlobal class is used to Scheduler information that is global to all SchedulerServerSession objects
    /// within the process.  A common use for this class would be to cache static information from the database in order to
    /// reduce accesses to the database.
    /// </remarks>

    public class SchedulerServerGlobal : Global
    {
        //=======
        // FIELDS
        //=======

        const int cMaxRestarts = 5;

        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        static private ArrayList _loadLock;
        static private bool _loaded;
        static private Audit _audit;

        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        static private SchedulerConfigInfo _schedConfigInfo;
        static private int _restartCount;
        static private ScheduleData _dlSchedule;
        static private DataTable _dtScheduledJobs;
        static private ScheduleProcessInfo _schedProcInfo;
        static private Thread _processScheduleThread;
        static private bool _endProcessScheduleThread;
        static private MIDReaderWriterLock _scheduleLock;

        //=============
        // CONSTRUCTORS
        //=============

        /// <summary>
        /// Creates a new instance of SchedulerServerGlobal
        /// </summary>

        static SchedulerServerGlobal()
        {
            try
            {
                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                _loadLock = new ArrayList();
                _loaded = false;

                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                if (!EventLog.SourceExists("MIDSchedulerService"))
                {
                    EventLog.CreateEventSource("MIDSchedulerService", null);
                }
            }
            catch (Exception exc)
            {
                EventLog.WriteEntry("MIDSchedulerService", "MIDSchedulerService: SchedulerServerGlobal encountered error - " + exc.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDSchedulerService: SchedulerServerGlobal encountered error - " + exc.Message);
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
        static internal DataTable Schedule
        {
            get
            {
                return _dtScheduledJobs;
            }
        }

        static internal SchedulerConfigInfo SchedConfigInfo
        {
            get
            {
                return _schedConfigInfo;
            }
        }

        //===============
        // PUBLIC METHODS
        //===============

        /// <summary>
        /// The Load method is called by the service or client to trigger the instantiation of the static StoreServerGlobal object
        /// </summary>

        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
        //static public void Load()
        static public void Load(bool aLocal)
        // End TT#189
        {
            try
            {
                //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
                //LoadBase(eProcesses.schedulerService);
                lock (_loadLock.SyncRoot)
                {
                    if (!_loaded)
                    {
                        //Begin TT#5320-VStuart-deadlock issues-FinishLine
                        if (!aLocal)
                        {
                            MarkRunningProcesses(eProcesses.schedulerService);  // TT#4483 - JSmith - Modify Control Service start to not update other running services as Unexpected Termination
                        }
                        //End TT#5320-VStuart-deadlock issues-FinishLine

                        // Begin TT#189 - RMatelic - Remove excessive entries from the Audit
                        //_audit = new Audit(eProcesses.schedulerService, Include.AdministratorUserRID);
                        if (!aLocal)
                        {
                            _audit = new Audit(eProcesses.schedulerService, Include.AdministratorUserRID);
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
                        LoadBase(eMIDMessageSenderRecepient.schedulerService, messagingInterval, aLocal, eProcesses.schedulerService);
                        // End TT#2307;
                        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                        EventLog.WriteEntry("MIDSchedulerService", "MIDSchedulerService begin building scheduler information", EventLogEntryType.Information);

                        BuildSchedulerServerGlobalArea();

                        EventLog.WriteEntry("MIDSchedulerService", "MIDSchedulerService completed building scheduler information", EventLogEntryType.Information);
                        //Begin Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.

                        // Begin TT#195 MD - JSmith - Add environment authentication
                        if (!aLocal)
                        {
                            RegisterServiceStart();
                        }
                        // End TT#195 MD

                        _loaded = true;
                    }
                }
                //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
            }
            catch (Exception exc)
            {
                EventLog.WriteEntry("MIDSchedulerService", "MIDSchedulerService: Load encountered error - " + exc.Message, EventLogEntryType.Error);
                throw new MIDException(eErrorLevel.severe, 0, "MIDSchedulerService: Load encountered error - " + exc.Message);
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
                // Begin TT#1303 - stodd - null ref
                if (Audit != null)
                {
                    Audit.CloseUpdateConnection();
                }
                // End TT#1303 - stodd - null ref
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

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

        //End Track #5583 - JScott - Ran an export for Store Data for 5 weeks and it failed.  Audit report attached.
        static public void EndProcessScheduleThread()
        {
            try
            {
                _endProcessScheduleThread = true;

                while (_processScheduleThread != null && _processScheduleThread.IsAlive)
                {
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in EndProcessScheduleThread", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        //=================
        // INTERNAL METHODS
        //=================

        // Begin Alert Events Code -- DO NOT REMOVE
        static internal void ScheduleNewJob(ScheduleProfile aSchedProf, JobProfile aJobProf, int aTaskListRID, int aUserRID)
        //		static internal void ScheduleNewJob(ScheduleProfile aSchedProf, JobProfile aJobProf, int aTaskListRID, int aUserRID, JobFinishAlertEvent aJobFinishAlertEvent)
        // End Alert Events Code -- DO NOT REMOVE
        {
            DataRow schedRow;
            DataRow newRow;

            try
            {
                LockForWrite();

                try
                {
                    aJobProf.Key = _dlSchedule.Job_Insert(aJobProf, aUserRID);
                    aSchedProf.Key = _dlSchedule.Schedule_Insert(aSchedProf, aUserRID);

                    _dlSchedule.JobTaskListJoin_Insert(new JobTaskListJoinProfile(aJobProf.Key, aTaskListRID, 0));
                    _dlSchedule.ScheduleJobJoin_Insert(new ScheduleJobJoinProfile(aSchedProf.Key, aJobProf.Key, aUserRID));
                    _dlSchedule.CommitData();

                    schedRow = _dlSchedule.ReadScheduledJob(aSchedProf.Key, aJobProf.Key);
                    newRow = _dtScheduledJobs.NewRow();
                    newRow.ItemArray = schedRow.ItemArray;
                    // Begin Alert Events Code -- DO NOT REMOVE
                    //					newRow["JobFinishAlertEvent"] = aJobFinishAlertEvent;
                    // End Alert Events Code -- DO NOT REMOVE
                    _dtScheduledJobs.Rows.Add(newRow);
                    CalculateInitialRunStatus(aSchedProf, newRow);
                    _dtScheduledJobs.AcceptChanges();

                    _dlSchedule.ScheduleJobJoin_Update(newRow);
                    _dlSchedule.CommitData();
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ScheduleNewJob:1", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForWrite();
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ScheduleNewJob:2", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        // Begin Alert Events Code -- DO NOT REMOVE
        static internal void ScheduleExistingJob(ScheduleProfile aSchedProf, int aJobRID, int aUserRID)
        //		static internal void ScheduleExistingJob(ScheduleProfile aSchedProf, int aJobRID, int aUserRID, JobFinishAlertEvent aJobFinishAlertEvent)
        // End Alert Events Code -- DO NOT REMOVE
        {
            DataRow schedRow;
            DataRow newRow;

            try
            {
                LockForWrite();

                try
                {
                    aSchedProf.Key = _dlSchedule.Schedule_Insert(aSchedProf, aUserRID);

                    _dlSchedule.ScheduleJobJoin_Insert(new ScheduleJobJoinProfile(aSchedProf.Key, aJobRID, aUserRID));
                    _dlSchedule.CommitData();

                    schedRow = _dlSchedule.ReadScheduledJob(aSchedProf.Key, aJobRID);
                    newRow = _dtScheduledJobs.NewRow();
                    newRow.ItemArray = schedRow.ItemArray;
                    // Begin Alert Events Code -- DO NOT REMOVE
                    //					newRow["JobFinishAlertEvent"] = aJobFinishAlertEvent;
                    // End Alert Events Code -- DO NOT REMOVE
                    _dtScheduledJobs.Rows.Add(newRow);
                    CalculateInitialRunStatus(aSchedProf, newRow);
                    _dtScheduledJobs.AcceptChanges();

                    _dlSchedule.ScheduleJobJoin_Update(newRow);
                    _dlSchedule.CommitData();
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ScheduleExistingJob:1", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForWrite();
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ScheduleExistingJob:2", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static internal void UpdateSchedule(ScheduleProfile aSchedProf, int aJobRID, int aUserRID)
        {
            DataRow schedRow;
            DataRow newRow;
            eProcessExecutionStatus procStat;
            // Begin Alert Events Code -- DO NOT REMOVE
            //			JobFinishAlertEvent jobFinishAlertEvent;
            // End Alert Events Code -- DO NOT REMOVE

            try
            {
                LockForRead();

                try
                {
                    schedRow = _dtScheduledJobs.Rows.Find(new object[] { aSchedProf.Key, aJobRID });

                    if (schedRow != null)
                    {
                        procStat = (eProcessExecutionStatus)(int)schedRow["EXECUTION_STATUS"];
                        // Begin Alert Events Code -- DO NOT REMOVE
                        //						jobFinishAlertEvent = (JobFinishAlertEvent)schedRow["JobFinishAlertEvent"];
                        // End Alert Events Code -- DO NOT REMOVE

                        if (procStat == eProcessExecutionStatus.Waiting || procStat == eProcessExecutionStatus.Cancelled ||
                            procStat == eProcessExecutionStatus.Completed || procStat == eProcessExecutionStatus.InError ||
                            procStat == eProcessExecutionStatus.OnHold)
                        {
                            LockForWrite();

                            try
                            {
                                _dlSchedule.Schedule_Update(aSchedProf, aUserRID);
                                _dlSchedule.CommitData();

                                _schedProcInfo.ClearScheduleProfile(Convert.ToInt32(schedRow["SCHED_RID"]), Convert.ToInt32(schedRow["JOB_RID"]));
                                schedRow.Delete();

                                schedRow = _dlSchedule.ReadScheduledJob(aSchedProf.Key, aJobRID);
                                newRow = _dtScheduledJobs.NewRow();
                                newRow.ItemArray = schedRow.ItemArray;
                                // Begin Alert Events Code -- DO NOT REMOVE
                                //								newRow["JobFinishAlertEvent"] = jobFinishAlertEvent;
                                // End Alert Events Code -- DO NOT REMOVE
                                _dtScheduledJobs.Rows.Add(newRow);
                                CalculateInitialRunStatus(aSchedProf, newRow);
                                _dtScheduledJobs.AcceptChanges();

                                _dlSchedule.ScheduleJobJoin_Update(newRow);
                                _dlSchedule.CommitData();
                            }
                            catch (Exception exc)
                            {
                                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in UpdateSchedule:1", "SchedulerServerSessionGlobal");
                                Audit.Log_Exception(exc);
                                throw;
                            }
                            finally
                            {
                                UnlockForWrite();
                            }
                        }
                        else
                        {
                            throw new InvalidJobStatusForAction();
                        }
                    }
                    else
                    {
                        throw new JobDoesNotExist();
                    }
                }
                catch (InvalidJobStatusForAction exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (JobDoesNotExist exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in UpdateSchedule:2", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (InvalidJobStatusForAction exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (JobDoesNotExist exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in UpdateSchedule:3", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static internal DateTime GetNextRunDate(ScheduleProfile aSchedProf, int aJobRID)
        {
            DataRow schedRow;
            DataRow newRow;

            try
            {
                LockForRead();

                try
                {
                    newRow = _dtScheduledJobs.NewRow();
                    schedRow = _dtScheduledJobs.Rows.Find(new object[] { aSchedProf.Key, aJobRID });

                    if (schedRow != null)
                    {
                        newRow.ItemArray = schedRow.ItemArray;
                    }

                    CalculateInitialRunStatus(aSchedProf, newRow);

                    if (newRow["NEXT_RUN_DATETIME"] != DBNull.Value)
                    {
                        //Begin Track #398 - JScott - Scheduler does not recognize different time zones
                        //return Convert.ToDateTime(newRow["NEXT_RUN_DATETIME"]);
                        return Convert.ToDateTime(newRow["NEXT_RUN_DATETIME"]).ToUniversalTime();
                        //End Track #398 - JScott - Scheduler does not recognize different time zones
                    }
                    else
                    {
                        return DateTime.MinValue;
                    }
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in GetNextRunDate:1", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in GetNextRunDate:2", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static internal bool isJobInCycle(ScheduleProfile aSchedProf, int aJobRID)
        {
            DataRow schedRow;

            try
            {
                LockForRead();

                try
                {
                    schedRow = _dtScheduledJobs.Rows.Find(new object[] { aSchedProf.Key, aJobRID });

                    if (schedRow != null)
                    {
                        if (schedRow["REPEAT_UNTIL_DATETIME"] != DBNull.Value)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in isJobInCycle:1", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in isJobInCycle:2", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static internal eProcessExecutionStatus GetJobStatus(int aSchedRID, int aJobRID)
        {
            DataRow schedRow;

            try
            {
                LockForRead();

                try
                {
                    schedRow = _dtScheduledJobs.Rows.Find(new object[] { aSchedRID, aJobRID });

                    if (schedRow != null)
                    {
                        return (eProcessExecutionStatus)(int)schedRow["EXECUTION_STATUS"];
                    }
                    else
                    {
                        throw new JobDoesNotExist();
                    }
                }
                catch (JobDoesNotExist exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in GetJobStatus:1", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (JobDoesNotExist exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in GetJobStatus:2", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

		// Begin TT#1386-MD - stodd - Scheduler Job Manager
        static internal DataTable GetScheduledJobsForJobManager(string scheduleName, string jobName, int userRID)
        {
            DataTable dtJobs;

            try
            {
                LockForRead();

                try
                {
                    dtJobs = _dlSchedule.ReadScheduledJobsForJobManager(scheduleName, jobName, userRID);

                    return dtJobs;
                }
                catch (JobDoesNotExist exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ReadScheduledJobsForJobManager:1", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (JobDoesNotExist exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ReadScheduledJobsForJobManager:2", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }
		// End TT#1386-MD - stodd - Scheduler Job Manager

        static internal void HoldJob(int aSchedRID, int aJobRID, int aUserRID)	// TT#1386-MD - stodd - Scheduler Job Manager
        {
            DataRow schedRow;
            eProcessExecutionStatus procStat;

            try
            {
                LockForRead();

                try
                {
                    schedRow = _dtScheduledJobs.Rows.Find(new object[] { aSchedRID, aJobRID });

                    if (schedRow != null)
                    {
                        procStat = (eProcessExecutionStatus)(int)schedRow["EXECUTION_STATUS"];

                        if (procStat == eProcessExecutionStatus.Waiting)
                        {
                            LockForWrite();

                            try
                            {
                                schedRow["EXECUTION_STATUS"] = eProcessExecutionStatus.OnHold;
                                // Begin TT#1386-MD - stodd - Scheduler Job Manager
                                schedRow["HOLD_BY_DATETIME"] = DateTime.Now;
                                schedRow["HOLD_BY_USER_RID"] = aUserRID;
                                schedRow["RELEASED_BY_DATETIME"] = DBNull.Value;
                                schedRow["RELEASED_BY_USER_RID"] = DBNull.Value;
                                // End TT#1386-MD - stodd - Scheduler Job Manager

                                _dlSchedule.ScheduleJobJoin_Update(schedRow);
                                _dlSchedule.CommitData();
                            }
                            catch (Exception exc)
                            {
                                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HoldJob:1", "SchedulerServerSessionGlobal");
                                Audit.Log_Exception(exc);
                                throw;
                            }
                            finally
                            {
                                UnlockForWrite();
                            }
                        }
                        else
                        {
                            throw new InvalidJobStatusForAction();
                        }
                    }
                    else
                    {
                        throw new JobDoesNotExist();
                    }
                }
                catch (InvalidJobStatusForAction exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (JobDoesNotExist exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HoldJob:2", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (InvalidJobStatusForAction exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (JobDoesNotExist exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HoldJob:3", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static internal ArrayList HoldAllJobs(TaskListProfile aTaskListProf)
        {
            DataTable taskRows;
            DataRow[] runningRows;
            DataRow schedRow;
            ArrayList heldSchedules;

            try
            {
                LockForRead();

                try
                {
                    taskRows = _dlSchedule.ReadActiveJobsByTaskList(aTaskListProf.Key);

                    //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                    //runningRows = taskRows.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Executed);
                    runningRows = taskRows.Select(
                        "EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running +
                        " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Queued +
                        " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Executed);
                    //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler

                    if (runningRows.Length > 0)
                    {
                        throw new InvalidJobStatusForAction();
                    }

                    runningRows = taskRows.Select("EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Waiting);

                    heldSchedules = new ArrayList();

                    if (runningRows.Length > 0)
                    {
                        LockForWrite();

                        try
                        {
                            foreach (DataRow runningRow in runningRows)
                            {
                                schedRow = _dtScheduledJobs.Rows.Find(new object[] { Convert.ToInt32(runningRow["SCHED_RID"]), Convert.ToInt32(runningRow["JOB_RID"]) });

                                schedRow["EXECUTION_STATUS"] = eProcessExecutionStatus.OnHold;
                                _dlSchedule.ScheduleJobJoin_Update(schedRow);
                                heldSchedules.Add(new ScheduleKey(Convert.ToInt32(schedRow["SCHED_RID"]), Convert.ToInt32(schedRow["JOB_RID"])));
                            }

                            _dlSchedule.CommitData();
                        }
                        catch (Exception exc)
                        {
                            Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HoldAllActiveJobs:1", "SchedulerServerSessionGlobal");
                            Audit.Log_Exception(exc);
                            throw;
                        }
                        finally
                        {
                            UnlockForWrite();
                        }
                    }

                    return heldSchedules;
                }
                catch (InvalidJobStatusForAction exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HoldAllActiveJobs:2", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (InvalidJobStatusForAction exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HoldAllActiveJobs:3", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static internal ArrayList HoldAllJobs(JobProfile aJobProf)
        {
            DataRow[] schedRows;
            ArrayList heldSchedules;

            try
            {
                LockForRead();

                try
                {
                    //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                    //schedRows = _dtScheduledJobs.Select("JOB_RID = " + aJobProf.Key + " AND (EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running + ")");
                    schedRows = _dtScheduledJobs.Select(
                        "JOB_RID = " + aJobProf.Key +
                        " AND (EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running +
                        " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Queued +
                        " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Executed + ")");
                    //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler

                    if (schedRows.Length > 0)
                    {
                        throw new InvalidJobStatusForAction();
                    }

                    schedRows = _dtScheduledJobs.Select("JOB_RID = " + aJobProf.Key + " AND EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Waiting);
                    heldSchedules = new ArrayList();

                    if (schedRows.Length > 0)
                    {
                        LockForWrite();

                        try
                        {
                            foreach (DataRow schedRow in schedRows)
                            {
                                schedRow["EXECUTION_STATUS"] = eProcessExecutionStatus.OnHold;
                                _dlSchedule.ScheduleJobJoin_Update(schedRow);
                                heldSchedules.Add(new ScheduleKey(Convert.ToInt32(schedRow["SCHED_RID"]), Convert.ToInt32(schedRow["JOB_RID"])));
                            }

                            _dlSchedule.CommitData();
                        }
                        catch (Exception exc)
                        {
                            Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HoldAllActiveJobs:1", "SchedulerServerSessionGlobal");
                            Audit.Log_Exception(exc);
                            throw;
                        }
                        finally
                        {
                            UnlockForWrite();
                        }
                    }

                    return heldSchedules;
                }
                catch (InvalidJobStatusForAction exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HoldAllActiveJobs:2", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (InvalidJobStatusForAction exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HoldAllActiveJobs:3", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static internal ArrayList HoldAllJobs(SpecialRequestProfile aSpecialRequestProf)
        {
            DataRow[] schedRows;
            ArrayList heldSchedules;

            try
            {
                LockForRead();

                try
                {
                    //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                    //schedRows = _dtScheduledJobs.Select("JOB_RID = " + aSpecialRequestProf.Key + " AND (EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running + " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running + ")");
                    schedRows = _dtScheduledJobs.Select("JOB_RID = " + aSpecialRequestProf.Key +
                        " AND (EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Running +
                        " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Queued +
                        " OR EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Executed + ")");
                    //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler

                    if (schedRows.Length > 0)
                    {
                        throw new InvalidJobStatusForAction();
                    }

                    schedRows = _dtScheduledJobs.Select("JOB_RID = " + aSpecialRequestProf.Key + " AND EXECUTION_STATUS = " + (int)eProcessExecutionStatus.Waiting);
                    heldSchedules = new ArrayList();

                    if (schedRows.Length > 0)
                    {
                        LockForWrite();

                        try
                        {
                            foreach (DataRow schedRow in schedRows)
                            {
                                schedRow["EXECUTION_STATUS"] = eProcessExecutionStatus.OnHold;
                                _dlSchedule.ScheduleJobJoin_Update(schedRow);
                                heldSchedules.Add(new ScheduleKey(Convert.ToInt32(schedRow["SCHED_RID"]), Convert.ToInt32(schedRow["JOB_RID"])));
                            }

                            _dlSchedule.CommitData();
                        }
                        catch (Exception exc)
                        {
                            Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HoldAllActiveJobs:1", "SchedulerServerSessionGlobal");
                            Audit.Log_Exception(exc);
                            throw;
                        }
                        finally
                        {
                            UnlockForWrite();
                        }
                    }

                    return heldSchedules;
                }
                catch (InvalidJobStatusForAction exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HoldAllActiveJobs:2", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (InvalidJobStatusForAction exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HoldAllActiveJobs:3", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static internal void ResumeJob(int aSchedRID, int aJobRID, int aUserRID)	// TT#1386-MD - stodd - Scheduler Job Manager
        {
            DataRow schedRow;
            eProcessExecutionStatus procStat;

            try
            {
                LockForRead();

                try
                {
                    schedRow = _dtScheduledJobs.Rows.Find(new object[] { aSchedRID, aJobRID });

                    if (schedRow != null)
                    {
                        procStat = (eProcessExecutionStatus)(int)schedRow["EXECUTION_STATUS"];

                        if (procStat == eProcessExecutionStatus.OnHold)
                        {
                            LockForWrite();

                            try
                            {
                                SetExecutionStatus(schedRow, aUserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
                                _dlSchedule.ScheduleJobJoin_Update(schedRow);
                                _dlSchedule.CommitData();
                            }
                            catch (Exception exc)
                            {
                                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ResumeJob:1", "SchedulerServerSessionGlobal");
                                Audit.Log_Exception(exc);
                                throw;
                            }
                            finally
                            {
                                UnlockForWrite();
                            }
                        }
                        else
                        {
                            throw new InvalidJobStatusForAction();
                        }
                    }
                    else
                    {
                        throw new JobDoesNotExist();
                    }
                }
                catch (InvalidJobStatusForAction exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (JobDoesNotExist exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ResumeJob:2", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (InvalidJobStatusForAction exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (JobDoesNotExist exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ResumeJob:3", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static internal void ResumeAllJobs(ArrayList aHeldSchedules, int aUserRID)	// TT#1386-MD - stodd - Scheduler Job Manager
        {
            DataRow schedRow;
            eProcessExecutionStatus procStat;

            try
            {
                LockForRead();

                try
                {
                    if (aHeldSchedules.Count > 0)
                    {
                        LockForWrite();

                        try
                        {
                            foreach (ScheduleKey schedKey in aHeldSchedules)
                            {
                                schedRow = _dtScheduledJobs.Rows.Find(new object[] { schedKey.SchedRID, schedKey.JobRID });

                                procStat = (eProcessExecutionStatus)(int)schedRow["EXECUTION_STATUS"];

                                if (procStat == eProcessExecutionStatus.OnHold)
                                {
                                    SetExecutionStatus(schedRow, aUserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
                                    _dlSchedule.ScheduleJobJoin_Update(schedRow);
                                }
                            }

                            _dlSchedule.CommitData();
                        }
                        catch (Exception exc)
                        {
                            Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ResumeAllJobs:1", "SchedulerServerSessionGlobal");
                            Audit.Log_Exception(exc);
                            throw;
                        }
                        finally
                        {
                            UnlockForWrite();
                        }
                    }
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ResumeAllJobs:2", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ResumeAllJobs:3", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        // Begin Alert Events Code -- DO NOT REMOVE
        static internal void RunJobNow(int aSchedRID, int aJobRID)
        //		static internal void RunJobNow(int aSchedRID, int aJobRID, JobFinishAlertEvent aJobFinishAlertEvent)
        // End Alert Events Code -- DO NOT REMOVE
        {
            DataRow schedRow;
            eProcessExecutionStatus procStat;

            try
            {
                LockForRead();

                try
                {
                    schedRow = _dtScheduledJobs.Rows.Find(new object[] { aSchedRID, aJobRID });

                    if (schedRow != null)
                    {
                        procStat = (eProcessExecutionStatus)(int)schedRow["EXECUTION_STATUS"];

                        if (procStat == eProcessExecutionStatus.Waiting || procStat == eProcessExecutionStatus.Cancelled ||
                            procStat == eProcessExecutionStatus.Completed || procStat == eProcessExecutionStatus.InError)
                        {
                            LockForWrite();

                            try
                            {
                                schedRow["EXECUTION_STATUS"] = eProcessExecutionStatus.Waiting;
                                schedRow["NEXT_RUN_DATETIME"] = DateTime.Now;
                                schedRow["REPEAT_UNTIL_DATETIME"] = DBNull.Value;
                                // Begin Alert Events Code -- DO NOT REMOVE
                                //								schedRow["JobFinishAlertEvent"] = aJobFinishAlertEvent;
                                // End Alert Events Code -- DO NOT REMOVE
                                _dlSchedule.ScheduleJobJoin_Update(schedRow);
                                _dlSchedule.CommitData();
                            }
                            catch (Exception exc)
                            {
                                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in RunJobNow:1", "SchedulerServerSessionGlobal");
                                Audit.Log_Exception(exc);
                                throw;
                            }
                            finally
                            {
                                UnlockForWrite();
                            }
                        }
                        else
                        {
                            throw new InvalidJobStatusForAction();
                        }
                    }
                    else
                    {
                        throw new JobDoesNotExist();
                    }
                }
                catch (InvalidJobStatusForAction exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (JobDoesNotExist exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in RunJobNow:2", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (InvalidJobStatusForAction exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (JobDoesNotExist exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in RunJobNow:3", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static internal void CancelJob(int aSchedRID, int aJobRID, eProcessExecutionStatus aStatusAtCancel)
        {
            DataRow schedRow;
            eProcessExecutionStatus procStat;
            JobProcessor jobProc;

            try
            {
                LockForRead();

                try
                {
                    schedRow = _dtScheduledJobs.Rows.Find(new object[] { aSchedRID, aJobRID });

                    if (schedRow != null)
                    {
                        procStat = (eProcessExecutionStatus)(int)schedRow["EXECUTION_STATUS"];

                        if ((procStat == eProcessExecutionStatus.OnHold ||
                            procStat == eProcessExecutionStatus.InError ||
                            procStat == eProcessExecutionStatus.Waiting ||
                            //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                            //procStat == eProcessExecutionStatus.Running) &&
                            procStat == eProcessExecutionStatus.Running ||
                            procStat == eProcessExecutionStatus.Queued) &&
                            //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                            procStat == aStatusAtCancel)
                        {
                            if (procStat == eProcessExecutionStatus.Waiting ||
                                procStat == eProcessExecutionStatus.InError ||
                                procStat == eProcessExecutionStatus.OnHold)
                            {
                                LockForWrite();

                                try
                                {
                                    schedRow["NEXT_RUN_DATETIME"] = DBNull.Value;
                                    schedRow["REPEAT_UNTIL_DATETIME"] = DBNull.Value;
                                    schedRow["EXECUTION_STATUS"] = eProcessExecutionStatus.Cancelled;
                                    _dlSchedule.ScheduleJobJoin_Update(schedRow);
                                    _dlSchedule.CommitData();
                                }
                                catch (Exception exc)
                                {
                                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CancelJob:1", "SchedulerServerSessionGlobal");
                                    Audit.Log_Exception(exc);
                                    throw;
                                }
                                finally
                                {
                                    UnlockForWrite();
                                }
                            }
                            else
                            {
                                jobProc = _schedProcInfo.GetJobProcessor(Convert.ToInt32(schedRow["SCHED_RID"]), Convert.ToInt32(schedRow["JOB_RID"]));

                                if (jobProc != null)
                                {
                                    jobProc.AbortThread();
                                }
                            }
                        }
                        else
                        {
                            throw new InvalidJobStatusForAction();
                        }
                    }
                    else
                    {
                        throw new JobDoesNotExist();
                    }
                }
                catch (InvalidJobStatusForAction exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (JobDoesNotExist exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CancelJob:2", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForRead();
                }
            }
            catch (InvalidJobStatusForAction exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (JobDoesNotExist exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CancelJob:3", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static internal bool DeleteSchedulesFromList(DataTable aTable)
        {
            int schedRID;
            int jobRID;
            DataRow schedRow;
            eProcessExecutionStatus procStat;
            bool invalidJobStatusFound;

            try
            {
                LockForWrite();

                try
                {
                    invalidJobStatusFound = false;

                    foreach (DataRow row in aTable.Rows)
                    {
                        schedRID = Convert.ToInt32(row["SCHED_RID"]);
                        jobRID = Convert.ToInt32(row["JOB_RID"]);

                        schedRow = _dtScheduledJobs.Rows.Find(new object[] { schedRID, jobRID });

                        if (schedRow != null)
                        {
                            procStat = (eProcessExecutionStatus)(int)schedRow["EXECUTION_STATUS"];

                            //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                            //if (procStat != eProcessExecutionStatus.Running && procStat != eProcessExecutionStatus.Executed)
                            if (procStat != eProcessExecutionStatus.Running &&
                                procStat != eProcessExecutionStatus.Queued &&
                                procStat != eProcessExecutionStatus.Executed)
                            //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                            {

                                _dlSchedule.ScheduleJobJoin_Delete(schedRID, jobRID);
                                _dlSchedule.Schedule_Delete(schedRID);
                                _dlSchedule.CommitData();

                                //Begin Track #6495 - JScott - Execution Status of jobs will not change to running or completed from Queue
                                //BuildScheduleJobsTable();
                                _dtScheduledJobs.Rows.Remove(schedRow);
                                //End Track #6495 - JScott - Execution Status of jobs will not change to running or completed from Queue
                            }
                            else
                            {
                                invalidJobStatusFound = true;
                            }
                        }
                    }

                    return invalidJobStatusFound;
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in DeleteSchedulesFromList:1", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForWrite();
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in DeleteSchedulesFromList:2", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        //================
        // PRIVATE METHODS
        //================

        static private void BuildSchedulerServerGlobalArea()
        {
            try
            {
                _restartCount = 0;
                _dlSchedule = new ScheduleData();
                _scheduleLock = new MIDReaderWriterLock();
                _schedProcInfo = new ScheduleProcessInfo(Audit, _dlSchedule);

                _schedConfigInfo = new SchedulerConfigInfo();

                BuildScheduleJobsTable();

                LockForWrite();

                try
                {
                    foreach (DataRow row in _dtScheduledJobs.Rows)
                    {
                        //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                        //if ((eProcessExecutionStatus)Convert.ToInt32(row["EXECUTION_STATUS"]) == eProcessExecutionStatus.Running ||
                        if ((row["EXECUTION_STATUS"] != DBNull.Value &&
                            (eProcessExecutionStatus)Convert.ToInt32(row["EXECUTION_STATUS"]) == eProcessExecutionStatus.Running) ||
                            //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                            (row["LAST_RUN_DATETIME"] != DBNull.Value &&
                            (row["LAST_COMPLETION_DATETIME"] == DBNull.Value || Convert.ToDateTime(row["LAST_RUN_DATETIME"]) > Convert.ToDateTime(row["LAST_COMPLETION_DATETIME"]))))
                        {
                            row["EXECUTION_STATUS"] = eProcessExecutionStatus.InError;
                            row["NEXT_RUN_DATETIME"] = DBNull.Value;
                            row["REPEAT_UNTIL_DATETIME"] = DBNull.Value;
                        }
                        //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                        //else
                        //{
                        //    if (row["EXECUTION_STATUS"] == DBNull.Value ||
                        //        (eProcessExecutionStatus)Convert.ToInt32(row["EXECUTION_STATUS"]) == eProcessExecutionStatus.None)
                        //    {
                        //        CalculateInitialRunStatus(_schedProcInfo.GetScheduleProfile(Convert.ToInt32(row["SCHED_RID"]), Convert.ToInt32(row["JOB_RID"])), row);
                        //    }
                        //}
                        else if (row["EXECUTION_STATUS"] != DBNull.Value &&
                                (eProcessExecutionStatus)Convert.ToInt32(row["EXECUTION_STATUS"]) == eProcessExecutionStatus.Queued)
                        {
                            row["EXECUTION_STATUS"] = eProcessExecutionStatus.Waiting;
                        }
                        else if (row["EXECUTION_STATUS"] == DBNull.Value ||
                                (eProcessExecutionStatus)Convert.ToInt32(row["EXECUTION_STATUS"]) == eProcessExecutionStatus.None)
                        {
                            CalculateInitialRunStatus(_schedProcInfo.GetScheduleProfile(Convert.ToInt32(row["SCHED_RID"]), Convert.ToInt32(row["JOB_RID"])), row);
                        }
                        //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                    }

                    _dtScheduledJobs.AcceptChanges();
                    _dlSchedule.ScheduleJobJoin_Update(_dtScheduledJobs);
                    _dlSchedule.CommitData();
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in BuildSchedulerServerGlobalArea:1", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForWrite();
                }

                _processScheduleThread = new Thread(new ThreadStart(ProcessScheduleThread));
                _processScheduleThread.Start();
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in BuildSchedulerServerGlobalArea:2", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static private void BuildScheduleJobsTable()
        {
            try
            {
                _dtScheduledJobs = _dlSchedule.ReadScheduledJobs();
                _dtScheduledJobs.PrimaryKey = new DataColumn[] { _dtScheduledJobs.Columns["SCHED_RID"], _dtScheduledJobs.Columns["JOB_RID"] };
                // Begin Alert Events Code -- DO NOT REMOVE
                //				_dtScheduledJobs.Columns.Add("JobFinishAlertEvent", typeof(object));
                // End Alert Events Code -- DO NOT REMOVE
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in BuildScheduleJobsTable", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static private void ProcessScheduleThread()
        {
            ScheduleProfile schedProf;
            JobProcessor jobProc;
            bool condPassed;
            int i;
            int sleepCount;
            string[] condFileList;
            bool runningJobs;
            //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
            Queue jobQueue;
            JobProcessor[] runningJobList;
            QueuedJobEntry queueEntry;
            //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
            //Begin TT#1331 - JScott - Task Lists in queued status
            DataRow[] jobArray;
            //End TT#1331 - JScott - Task Lists in queued status

            try
            {
                //Begin TT#1316 - JScott - Client reported Scheduler Browser showing all Jobs as Queued
                Audit.Add_Msg(eMIDMessageLevel.Information, "MaximumConcurrentProcesses is set to " + _schedConfigInfo.MaximumConcurrentProcesses.ToString(), "SchedulerServerSessionGlobal", true);
                //End TT#1316 - JScott - Client reported Scheduler Browser showing all Jobs as Queued
                Audit.Add_Msg(eMIDMessageLevel.Information, "Beginning scan of Schedules...", "SchedulerServerSessionGlobal");
                //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                jobQueue = new Queue();
                runningJobList = new JobProcessor[_schedConfigInfo.MaximumConcurrentProcesses];
                //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler

                while (true)
                {
                    LockForRead();

                    try
                    {
                        runningJobs = false;

                        //Begin TT#1331 - JScott - Task Lists in queued status
                        //foreach (DataRow row in _dtScheduledJobs.Rows)
                        jobArray = new DataRow[_dtScheduledJobs.Rows.Count];
                        _dtScheduledJobs.Rows.CopyTo(jobArray, 0);

                        foreach (DataRow row in jobArray)
                        //End TT#1331 - JScott - Task Lists in queued status
                        {
                            try
                            {
                                schedProf = _schedProcInfo.GetScheduleProfile(Convert.ToInt32(row["SCHED_RID"]), Convert.ToInt32(row["JOB_RID"]));

                                switch ((eProcessExecutionStatus)Convert.ToInt32(row["EXECUTION_STATUS"]))
                                {
                                    case eProcessExecutionStatus.Waiting:

                                        if (Convert.ToDateTime(row["NEXT_RUN_DATETIME"]) <= DateTime.Now)
                                        {
                                            condPassed = false;

                                            switch (schedProf.ConditionType)
                                            {
                                                case eScheduleConditionType.ByFileExtension:

                                                    if (System.IO.Directory.Exists(schedProf.ConditionTriggerDirectory))
                                                    {
                                                        //Begin TT#1281 - JScott - WUB header load failed
                                                        //if (System.IO.Directory.GetFiles(schedProf.ConditionTriggerDirectory, CommonScheduleRoutines.FormatExtension(schedProf.ConditionTriggerSuffix)).Length > 0)
                                                        if (Include.GetFiles(schedProf.ConditionTriggerDirectory, CommonScheduleRoutines.FormatExtension(schedProf.ConditionTriggerSuffix)).Length > 0)
                                                        //End TT#1281 - JScott - WUB header load failed
                                                        {
                                                            condPassed = true;
                                                        }
                                                    }
                                                    break;

                                                default:

                                                    condPassed = true;
                                                    break;
                                            }

                                            if (condPassed)
                                            {
                                                LockForWrite();

                                                try
                                                {
                                                    //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                                                    //row["EXECUTION_STATUS"] = eProcessExecutionStatus.Running;
                                                    //row["LAST_RUN_DATETIME"] = DateTime.Now;
                                                    row["EXECUTION_STATUS"] = eProcessExecutionStatus.Queued;
                                                    //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler

                                                    _dlSchedule.ScheduleJobJoin_Update(row);
                                                    _dlSchedule.CommitData();
                                                }
                                                catch (Exception exc)
                                                {
                                                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessScheduleThread:1", "SchedulerServerSessionGlobal");
                                                    Audit.Log_Exception(exc);
                                                    throw;
                                                }
                                                finally
                                                {
                                                    UnlockForWrite();
                                                }

                                                jobProc = _schedProcInfo.AddJobProcessor(_schedConfigInfo, Convert.ToInt32(row["SCHED_RID"]), Convert.ToInt32(row["JOB_RID"]));
                                                //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                                                //jobProc.ExecuteJobInThread();
                                                jobQueue.Enqueue(new QueuedJobEntry(jobProc, row));
                                                //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                                            }
                                            else
                                            {
                                                LockForWrite();

                                                try
                                                {
                                                    row["EXECUTION_STATUS"] = eProcessExecutionStatus.Executed;
                                                    row["LAST_RUN_DATETIME"] = DateTime.Now;
                                                    row["LAST_COMPLETION_STATUS"] = eProcessCompletionStatus.ConditionFailed;
                                                    row["LAST_COMPLETION_DATETIME"] = DateTime.Now;

                                                    CalculateNextRunStatus(schedProf, row);

                                                    _dlSchedule.ScheduleJobJoin_Update(row);
                                                    _dlSchedule.CommitData();
                                                }
                                                catch (Exception exc)
                                                {
                                                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessScheduleThread:2", "SchedulerServerSessionGlobal");
                                                    Audit.Log_Exception(exc);
                                                    throw;
                                                }
                                                finally
                                                {
                                                    UnlockForWrite();
                                                }
                                            }
                                        }
                                        break;

                                    case eProcessExecutionStatus.Running:

                                        jobProc = _schedProcInfo.GetJobProcessor(Convert.ToInt32(row["SCHED_RID"]), Convert.ToInt32(row["JOB_RID"]));

                                        if (jobProc != null && !jobProc.isRunning)
                                        {
                                            LockForWrite();

                                            try
                                            {
                                                if (schedProf.ConditionType == eScheduleConditionType.ByFileExtension)
                                                {
                                                    //Begin TT#1281 - JScott - WUB header load failed
                                                    //condFileList = System.IO.Directory.GetFiles(schedProf.ConditionTriggerDirectory, CommonScheduleRoutines.FormatExtension(schedProf.ConditionTriggerSuffix));
                                                    condFileList = Include.GetFiles(schedProf.ConditionTriggerDirectory, CommonScheduleRoutines.FormatExtension(schedProf.ConditionTriggerSuffix));
                                                    //End TT#1281 - JScott - WUB header load failed

                                                    foreach (string file in condFileList)
                                                    {
                                                        System.IO.File.Delete(file);
                                                    }
                                                }

                                                if (jobProc.CompletionStatus == eProcessCompletionStatus.Cancelled)
                                                {
                                                    row["EXECUTION_STATUS"] = eProcessExecutionStatus.Cancelled;
                                                    row["LAST_COMPLETION_STATUS"] = jobProc.CompletionStatus;
                                                    row["LAST_COMPLETION_DATETIME"] = jobProc.CompletionDateTime;
                                                    row["NEXT_RUN_DATETIME"] = DBNull.Value;
                                                    row["REPEAT_UNTIL_DATETIME"] = DBNull.Value;
                                                }
                                                else
                                                {
                                                    row["EXECUTION_STATUS"] = eProcessExecutionStatus.Executed;
                                                    row["LAST_COMPLETION_STATUS"] = jobProc.CompletionStatus;
                                                    row["LAST_COMPLETION_DATETIME"] = jobProc.CompletionDateTime;

                                                    CalculateNextRunStatus(schedProf, row);
                                                }

                                                _schedProcInfo.ClearJobProcessor(Convert.ToInt32(row["SCHED_RID"]), Convert.ToInt32(row["JOB_RID"]));

                                                _dlSchedule.ScheduleJobJoin_Update(row);
                                                _dlSchedule.CommitData();
                                                // Begin Alert Events Code -- DO NOT REMOVE
                                                //
                                                //												if (row["JobFinishAlertEvent"] != System.DBNull.Value)
                                                //												{
                                                //													((JobFinishAlertEvent)row["JobFinishAlertEvent"]).FireEvent(new AlertEventArgs(schedProf.Name + " has Completed"));
                                                //												}
                                                // End Alert Events Code -- DO NOT REMOVE
                                            }
                                            catch (Exception exc)
                                            {
                                                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessScheduleThread:3", "SchedulerServerSessionGlobal");
                                                Audit.Log_Exception(exc);
                                                throw;
                                            }
                                            finally
                                            {
                                                UnlockForWrite();
                                            }
                                        }
                                        else
                                        {
                                            runningJobs = true;
                                        }
                                        break;
                                }
                            }
                            catch (Exception exc)
                            {
                                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessScheduleThread:4", "SchedulerServerSessionGlobal");
                                Audit.Log_Exception(exc);

                                LockForWrite();

                                try
                                {
                                    row["EXECUTION_STATUS"] = eProcessExecutionStatus.InError;

                                    _dlSchedule.ScheduleJobJoin_Update(row);
                                    _dlSchedule.CommitData();
                                }
                                catch (Exception exc2)
                                {
                                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessScheduleThread:5", "SchedulerServerSessionGlobal");
                                    Audit.Log_Exception(exc2);
                                    throw;
                                }
                                finally
                                {
                                    UnlockForWrite();
                                }
                            }
                        }
                        //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler

                        if (runningJobList.Length > 0)
                        {
                            for (i = 0; i < runningJobList.Length; i++)
                            {
                                if (runningJobList[i] != null)
                                {
                                    if (!runningJobList[i].isRunning)
                                    {
                                        runningJobList[i] = null;
                                    }
                                    else
                                    {
                                        runningJobs = true;
                                    }
                                }

                                if (runningJobList[i] == null && jobQueue.Count > 0)
                                {
                                    queueEntry = (QueuedJobEntry)jobQueue.Dequeue();

                                    ScheduleQueuedJob(queueEntry);

                                    runningJobList[i] = queueEntry.JobProcessor;
                                    runningJobList[i].ExecuteJobInThread();

                                    runningJobs = true;
                                }
                            }
                        }
                        else
                        {
                            while (jobQueue.Count > 0)
                            {
                                queueEntry = (QueuedJobEntry)jobQueue.Dequeue();

                                ScheduleQueuedJob(queueEntry);

                                queueEntry.JobProcessor.ExecuteJobInThread();

                                runningJobs = true;
                            }
                        }
                        //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
                    }
                    catch (Exception exc)
                    {
                        Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessScheduleThread:6", "SchedulerServerSessionGlobal");
                        Audit.Log_Exception(exc);
                        throw;
                    }
                    finally
                    {
                        UnlockForRead();
                    }

                    sleepCount = (int)Math.Ceiling((double)_schedConfigInfo.ScheduleScanInterval / (double)_schedConfigInfo.CheckForTerminateInterval);

                    if (_endProcessScheduleThread)
                    {
                        if (!runningJobs)
                        {
                            throw new EndScheduleProcessThreadException();
                        }
                        else
                        {
                            Audit.Add_Msg(eMIDMessageLevel.Warning, "Jobs are running -- termination suspended until jobs are finished", "SchedulerServerSessionGlobal");
                        }
                    }

                    for (i = 0; i < sleepCount; i++)
                    {
                        System.Threading.Thread.Sleep(_schedConfigInfo.CheckForTerminateInterval);
                    }
                }
            }
            catch (EndScheduleProcessThreadException)
            {
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessScheduleThread:7", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);

                if (_restartCount < cMaxRestarts)
                {
                    _restartCount++;
                    Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_SchedulerThreadReceivedExceptionRestarting, "SchedulerServerSessionGlobal");
                    _processScheduleThread = new Thread(new ThreadStart(ProcessScheduleThread));
                    _processScheduleThread.Start();
                }
                else
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, eMIDTextCode.msg_SchedulerThreadReceivedException, "SchedulerServerSessionGlobal");
                    throw;
                }
            }
            finally
            {
                Audit.Add_Msg(eMIDMessageLevel.Information, "Completed scan of Schedules.", "SchedulerServerSessionGlobal");
            }
        }

        //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
        static private void ScheduleQueuedJob(QueuedJobEntry aQueueEntry)
        {
            try
            {
                LockForWrite();

                try
                {
                    aQueueEntry.JobRow["EXECUTION_STATUS"] = eProcessExecutionStatus.Running;
                    aQueueEntry.JobRow["LAST_RUN_DATETIME"] = DateTime.Now;

                    _dlSchedule.ScheduleJobJoin_Update(aQueueEntry.JobRow);
                    _dlSchedule.CommitData();
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessScheduleThread:8", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
                finally
                {
                    UnlockForWrite();
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CalculateNextWeekDate", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
        static private void CalculateNextRunStatus(ScheduleProfile aSchedProf, DataRow aRow)
        {
            DateTime newDateTime;
            DateTime untilDateTime;
            DateTime lastRunDateTime;
            DateTime lastCompDateTime;
            DateTime repeatUntilDateTime;
            DateTime repeatDateTime;

            try
            {
                newDateTime = DateTime.MinValue;
                untilDateTime = DateTime.MinValue;

                lastRunDateTime = Convert.ToDateTime(aRow["LAST_RUN_DATETIME"]);

                if (aRow["LAST_COMPLETION_DATETIME"] != DBNull.Value)
                {
                    lastCompDateTime = Convert.ToDateTime(aRow["LAST_COMPLETION_DATETIME"]);
                }
                else
                {
                    lastCompDateTime = lastRunDateTime;
                }

                if (aSchedProf.RepeatInterval > 0)
                {
                    if (!aSchedProf.TerminateAfterConditionMet || (eProcessCompletionStatus)Convert.ToInt32(aRow["LAST_COMPLETION_STATUS"]) == eProcessCompletionStatus.ConditionFailed)
                    {
                        if (!aSchedProf.RepeatUntilSuccessful || (eProcessCompletionStatus)Convert.ToInt32(aRow["LAST_COMPLETION_STATUS"]) != eProcessCompletionStatus.Successful)
                        {
                            if (aRow["REPEAT_UNTIL_DATETIME"] == DBNull.Value || Convert.ToDateTime(aRow["REPEAT_UNTIL_DATETIME"]) == DateTime.MinValue)
                            {
                                if (aSchedProf.RepeatUntil)
                                {
                                    //Begin Track #398 - JScott - Scheduler does not recognize different time zones
                                    //repeatUntilDateTime = aSchedProf.RepeatUntilTime;
                                    //untilDateTime = new DateTime(
                                    //    lastRunDateTime.Year,
                                    //    lastRunDateTime.Month,
                                    //    lastRunDateTime.Day,
                                    //    repeatUntilDateTime.Hour,
                                    //    repeatUntilDateTime.Minute,
                                    //    repeatUntilDateTime.Second);

                                    //if (untilDateTime < lastRunDateTime)
                                    //{
                                    //    untilDateTime = untilDateTime.AddDays(1);
                                    //}
                                    if (aSchedProf.RepeatDurationHours == 0 && aSchedProf.RepeatDurationMinutes == 0)
                                    {
                                        repeatUntilDateTime = aSchedProf.RepeatUntilTime;
                                        untilDateTime = new DateTime(
                                            lastRunDateTime.Year,
                                            lastRunDateTime.Month,
                                            lastRunDateTime.Day,
                                            repeatUntilDateTime.Hour,
                                            repeatUntilDateTime.Minute,
                                            repeatUntilDateTime.Second);

                                        if (untilDateTime < lastRunDateTime)
                                        {
                                            untilDateTime = untilDateTime.AddDays(1);
                                        }
                                    }
                                    else
                                    {
                                        untilDateTime = lastCompDateTime.AddHours(aSchedProf.RepeatDurationHours);
                                        untilDateTime = untilDateTime.AddMinutes(aSchedProf.RepeatDurationMinutes);
                                    }
                                    //End Track #398 - JScott - Scheduler does not recognize different time zones
                                }
                                else if (aSchedProf.RepeatDuration)
                                {
                                    untilDateTime = lastCompDateTime.AddHours(aSchedProf.RepeatDurationHours);
                                    untilDateTime = untilDateTime.AddMinutes(aSchedProf.RepeatDurationMinutes);
                                }
                                else
                                {
                                    untilDateTime = DateTime.MaxValue;
                                }

                                if (aSchedProf.EndDate && untilDateTime.Date > aSchedProf.EndDateRange.Date)
                                {
                                    untilDateTime = new DateTime(
                                        aSchedProf.EndDateRange.Date.Year,
                                        aSchedProf.EndDateRange.Date.Month,
                                        aSchedProf.EndDateRange.Date.Day,
                                        DateTime.MinValue.Hour,
                                        DateTime.MinValue.Minute,
                                        DateTime.MinValue.Second);
                                }
                            }
                            else
                            {
                                untilDateTime = Convert.ToDateTime(aRow["REPEAT_UNTIL_DATETIME"]);
                            }

                            repeatDateTime = DateTime.MinValue;

                            switch ((eScheduleRepeatIntervalType)aSchedProf.RepeatIntervalType)
                            {
                                case eScheduleRepeatIntervalType.Seconds:
                                    repeatDateTime = lastCompDateTime.AddSeconds(aSchedProf.RepeatInterval);
                                    break;

                                case eScheduleRepeatIntervalType.Minutes:
                                    repeatDateTime = lastCompDateTime.AddMinutes(aSchedProf.RepeatInterval);
                                    break;

                                case eScheduleRepeatIntervalType.Hours:
                                    repeatDateTime = lastCompDateTime.AddHours(aSchedProf.RepeatInterval);
                                    break;
                            }

                            if (untilDateTime == DateTime.MinValue || repeatDateTime < untilDateTime)
                            {
                                newDateTime = repeatDateTime;
                            }
                        }
                    }
                }

                if (newDateTime == DateTime.MinValue)
                {
                    switch ((eScheduleByType)aSchedProf.ScheduleByType)
                    {
                        case eScheduleByType.Day:

                            newDateTime = lastRunDateTime;
                            newDateTime = lastRunDateTime.AddDays(aSchedProf.ScheduleByInterval);
                            break;

                        case eScheduleByType.Week:

                            // Begin TT#2522 - JSmith - Scheduled Task not adhering to frequency
                            //newDateTime = CalculateNextWeekDate(aSchedProf, lastRunDateTime);
                            newDateTime = CalculateNextWeekDate(aSchedProf, lastRunDateTime, false);
                            // End TT#2522
                            break;

                        case eScheduleByType.Month:

                            newDateTime = CalculateNextMonthDate(aSchedProf, lastRunDateTime);
                            break;

                        default:

                            if (aSchedProf.StartDateRange > lastCompDateTime)
                            {
                                newDateTime = aSchedProf.StartDateRange;
                            }
                            break;
                    }

                    if (newDateTime != DateTime.MinValue)
                    {
                        newDateTime = new DateTime(newDateTime.Year, newDateTime.Month, newDateTime.Day, aSchedProf.StartTime.Hour, aSchedProf.StartTime.Minute, aSchedProf.StartTime.Second);
                    }

                    untilDateTime = DateTime.MinValue;
                }

                if (newDateTime != DateTime.MinValue)
                {
                    if (aSchedProf.EndDate && newDateTime.Date >= aSchedProf.EndDateRange.Date)
                    {
                        newDateTime = DateTime.MinValue;
                        untilDateTime = DateTime.MinValue;
                    }
                }

                if (newDateTime == DateTime.MinValue)
                {
                    aRow["NEXT_RUN_DATETIME"] = DBNull.Value;
                }
                else
                {
                    aRow["NEXT_RUN_DATETIME"] = newDateTime;
                }

                if (untilDateTime == DateTime.MinValue)
                {
                    aRow["REPEAT_UNTIL_DATETIME"] = DBNull.Value;
                }
                else
                {
                    aRow["REPEAT_UNTIL_DATETIME"] = untilDateTime;
                }

                SetExecutionStatus(aRow, Include.AdministratorUserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CalculateNextRunStatus", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);

                aRow["EXECUTION_STATUS"] = eProcessExecutionStatus.InError;
            }
        }

        static private void CalculateInitialRunStatus(ScheduleProfile aSchedProf, DataRow aRow)
        {
            DateTime nextRunDateTime;

            try
            {
                if (aRow["NEXT_RUN_DATETIME"] != DBNull.Value)
                {
                    nextRunDateTime = Convert.ToDateTime(aRow["NEXT_RUN_DATETIME"]);
                }
                else
                {
                    nextRunDateTime = DateTime.MinValue;
                }

                switch ((eScheduleByType)aSchedProf.ScheduleByType)
                {
                    case eScheduleByType.Day:

                        nextRunDateTime = aSchedProf.StartDateRange;
                        while (nextRunDateTime.Date < DateTime.Now.Date)
                        {
                            nextRunDateTime = nextRunDateTime.AddDays(aSchedProf.ScheduleByInterval);
                        }
                        break;

                    case eScheduleByType.Week:

                        if (aSchedProf.StartDateRange.Date < DateTime.Now.Date)
                        {
                            // Begin TT#2522 - JSmith - Scheduled Task not adhering to frequency
                            //nextRunDateTime = CalculateNextWeekDate(aSchedProf, DateTime.Now.Date.AddDays(-1));
                            nextRunDateTime = CalculateNextWeekDate(aSchedProf, DateTime.Now.Date.AddDays(-1), true);
                            // End TT#2522
                        }
                        else
                        {
                            // Begin TT#2522 - JSmith - Scheduled Task not adhering to frequency
                            //nextRunDateTime = CalculateNextWeekDate(aSchedProf, aSchedProf.StartDateRange.AddDays(-1));
                            nextRunDateTime = CalculateNextWeekDate(aSchedProf, aSchedProf.StartDateRange.AddDays(-1), true);
                            // End TT#2522
                        }
                        break;

                    case eScheduleByType.Month:

                        if (aSchedProf.StartDateRange.Date < DateTime.Now.Date)
                        {
                            nextRunDateTime = CalculateNextMonthDate(aSchedProf, DateTime.Now.Date.AddDays(-1));
                        }
                        else
                        {
                            nextRunDateTime = CalculateNextMonthDate(aSchedProf, aSchedProf.StartDateRange.AddDays(-1));
                        }
                        break;

                    default:

                        nextRunDateTime = aSchedProf.StartDateRange;
                        break;
                }

                nextRunDateTime = new DateTime(nextRunDateTime.Year, nextRunDateTime.Month, nextRunDateTime.Day, aSchedProf.StartTime.Hour, aSchedProf.StartTime.Minute, aSchedProf.StartTime.Second);

                if (aSchedProf.EndDate && nextRunDateTime.Date >= aSchedProf.EndDateRange.Date)
                {
                    nextRunDateTime = DateTime.MinValue;
                }

                if (nextRunDateTime == DateTime.MinValue)
                {
                    aRow["NEXT_RUN_DATETIME"] = DBNull.Value;
                }
                else
                {
                    aRow["NEXT_RUN_DATETIME"] = nextRunDateTime;
                }

                aRow["REPEAT_UNTIL_DATETIME"] = DBNull.Value;

                SetExecutionStatus(aRow, Include.AdministratorUserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CalculateNextRunDate", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);

                aRow["EXECUTION_STATUS"] = eProcessExecutionStatus.InError;
            }
        }

        static private void SetExecutionStatus(DataRow aRow, int aUserRID)	// TT#1386-MD - stodd - Scheduler Job Manager
        {
            try
            {
                if (aRow["NEXT_RUN_DATETIME"] == DBNull.Value)
                {
                    aRow["EXECUTION_STATUS"] = eProcessExecutionStatus.Completed;
                }
                else
                {
                    aRow["EXECUTION_STATUS"] = eProcessExecutionStatus.Waiting;
                }
                // Begin TT#1386-MD - stodd - Scheduler Job Manager
                aRow["RELEASED_BY_DATETIME"] = DateTime.Now;
                aRow["RELEASED_BY_USER_RID"] = aUserRID;
                aRow["HOLD_BY_DATETIME"] = DBNull.Value;
                aRow["HOLD_BY_USER_RID"] = DBNull.Value;
                // EndTT#1386-MD - stodd - Scheduler Job Manager
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in SetExecutionStatus", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        // Begin TT#2522 - JSmith - Scheduled Task not adhering to frequency
        //static private DateTime CalculateNextWeekDate(ScheduleProfile aSchedProf, DateTime aDate)
        static private DateTime CalculateNextWeekDate(ScheduleProfile aSchedProf, DateTime aDate, bool aInitialStatus)
        // End TT#2522
        {
            DateTime newDate;

            try
            {
                newDate = GetNextWeekDay(aSchedProf, aDate);

                // Begin TT#2522 - JSmith - Scheduled Task not adhering to frequency
                //if (aDate.DayOfWeek > newDate.DayOfWeek)
                if ((!aInitialStatus && aDate.DayOfWeek == newDate.DayOfWeek) ||
                    aDate.DayOfWeek > newDate.DayOfWeek)
                // End TT#2522
                {
                    newDate = newDate.AddDays((aSchedProf.ScheduleByInterval - 1) * 7);
                }

                return newDate;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CalculateNextWeekDate", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static private DateTime CalculateNextMonthDate(ScheduleProfile aSchedProf, DateTime aDate)
        {
            DateTime newDate;
            DateTime BOWDate;
            DateTime tempDate;

            try
            {
                if (aSchedProf.ScheduleByMonthWeekType == eScheduleByMonthWeekType.Every)
                {
                    newDate = GetNextWeekDay(aSchedProf, aDate);

                    if (aDate.DayOfWeek < newDate.DayOfWeek)
                    {
                        newDate = newDate.AddDays(7);
                    }
                }
                else
                {
                    newDate = GetNextWeekDay(aSchedProf, aDate);
                    BOWDate = GetFirstDayOfWeekInMonth(aSchedProf, newDate);

                    if (newDate.Date < BOWDate.Date)
                    {
                        newDate = GetNextWeekDay(aSchedProf, BOWDate.AddDays(-1));
                    }
                    else if (newDate.Date > BOWDate.AddDays(6).Date)
                    {
                        tempDate = aDate.AddMonths(aSchedProf.ScheduleByInterval);
                        BOWDate = GetFirstDayOfWeekInMonth(aSchedProf, new DateTime(tempDate.Year, tempDate.Month, 1));
                        newDate = GetNextWeekDay(aSchedProf, BOWDate.AddDays(-1));
                    }
                }

                return newDate;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CalculateNextMonthDate", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static private DateTime GetNextWeekDay(ScheduleProfile aSchedProf, DateTime aDate)
        {
            DateTime newDate;
            DayOfWeek dayOfWeek;

            try
            {
                newDate = aDate.AddDays(1);
                dayOfWeek = newDate.DayOfWeek;

                while (!CheckDayOfWeek(aSchedProf, dayOfWeek))
                {
                    newDate = newDate.AddDays(1);
                    dayOfWeek = newDate.DayOfWeek;
                }

                return newDate;
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in GetNextWeekDay", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static private DateTime GetFirstDayOfWeekInMonth(ScheduleProfile aSchedProf, DateTime aDate)
        {
            DateTime newDate;

            try
            {
                switch (aSchedProf.ScheduleByMonthWeekType)
                {
                    case eScheduleByMonthWeekType.First:
                        newDate = new DateTime(aDate.Year, aDate.Month, 1);
                        return newDate;

                    case eScheduleByMonthWeekType.Second:
                        newDate = new DateTime(aDate.Year, aDate.Month, 1);
                        return newDate.AddDays(7);

                    case eScheduleByMonthWeekType.Third:
                        newDate = new DateTime(aDate.Year, aDate.Month, 1);
                        return newDate.AddDays(14);

                    case eScheduleByMonthWeekType.Fourth:
                        newDate = new DateTime(aDate.Year, aDate.Month, 1);
                        return newDate.AddDays(21);

                    default:
                        newDate = new DateTime(aDate.Year, aDate.Month, DateTime.DaysInMonth(aDate.Year, aDate.Month));
                        return newDate.AddDays(-6);
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in GetFirstDayOfWeekInMonth", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static private bool CheckDayOfWeek(ScheduleProfile aSchedProf, DayOfWeek aDayOfWeek)
        {
            try
            {
                switch (aDayOfWeek)
                {
                    case DayOfWeek.Sunday:
                        return aSchedProf.ScheduleByDaysInWeek.Sunday;

                    case DayOfWeek.Monday:
                        return aSchedProf.ScheduleByDaysInWeek.Monday;

                    case DayOfWeek.Tuesday:
                        return aSchedProf.ScheduleByDaysInWeek.Tuesday;

                    case DayOfWeek.Wednesday:
                        return aSchedProf.ScheduleByDaysInWeek.Wednesday;

                    case DayOfWeek.Thursday:
                        return aSchedProf.ScheduleByDaysInWeek.Thursday;

                    case DayOfWeek.Friday:
                        return aSchedProf.ScheduleByDaysInWeek.Friday;

                    default:
                        return aSchedProf.ScheduleByDaysInWeek.Saturday;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CheckDayOfWeek", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static private void LockForRead()
        {
            try
            {
                _scheduleLock.AcquireReaderLock(ReaderLockTimeOut);
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in LockForRead", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static private void UnlockForRead()
        {
            try
            {
                _scheduleLock.ReleaseReaderLock();
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in UnlockForRead", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static private void LockForWrite()
        {
            try
            {
                _scheduleLock.AcquireWriterLock(WriterLockTimeOut);

                try
                {
                    _dlSchedule.OpenUpdateConnection();
                }
                catch (Exception exc)
                {
                    _scheduleLock.ReleaseWriterLock();

                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in LockForWrite:1", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
            }
            catch (Exception exc)
            {
                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in LockForWrite:2", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }

        static private void UnlockForWrite()
        {
            try
            {
                _dlSchedule.CloseUpdateConnection();

                try
                {
                    _scheduleLock.ReleaseWriterLock();
                }
                catch (Exception exc)
                {
                    Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in UnlockForWrite:1", "SchedulerServerSessionGlobal");
                    Audit.Log_Exception(exc);
                    throw;
                }
            }
            catch (Exception exc)
            {
                _scheduleLock.ReleaseWriterLock();

                Audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in UnlockForWrite:2", "SchedulerServerSessionGlobal");
                Audit.Log_Exception(exc);
                throw;
            }
        }
    }

    public class CommonScheduleRoutines
    {
        static CommonScheduleRoutines()
        {
        }

        //Begin TT#1067 - JScott - Add option to allow return codes be consistent with other processes
        //static internal eMIDMessageLevel DetermineMessageLevelFromCompletionCode(eProcessCompletionStatus aProcCompStat)
        static public eMIDMessageLevel DetermineMessageLevelFromCompletionCode(eProcessCompletionStatus aProcCompStat)
        //End TT#1067 - JScott - Add option to allow return codes be consistent with other processes
        {
            try
            {
                switch (aProcCompStat)
                {
                    case eProcessCompletionStatus.Successful:
                        return eMIDMessageLevel.Information;

                    case eProcessCompletionStatus.Failed:
                        return eMIDMessageLevel.Error;

                    case eProcessCompletionStatus.ConditionFailed:
                    case eProcessCompletionStatus.Cancelled:
                        return eMIDMessageLevel.Warning;

                    default:
                        return eMIDMessageLevel.Information;
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        static internal string FormatExtension(string aExtension)
        {
            try
            {
                if (aExtension[0] != '.')
                {
                    aExtension = "." + aExtension;
                }

                return "*" + aExtension;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
    }

    [Serializable]
    public class SchedulerConfigInfo
    {
        const int cScheduleScanInterval = 10000;
        const int cCheckForTerminateInterval = 2000;
        //Begin Track #6468 -- Limit number of concurrent processes submitted by the Scheduler
        const int cMaximumConcurrentProcessesStr = 0;
        //End Track #6468 -- Limit number of concurrent processes submitted by the Scheduler
        const ProcessWindowStyle cStartedProcessWindowStyle = ProcessWindowStyle.Normal;

        private int _scheduleScanInterval;
        private int _checkForTerminateInterval;
        //Begin Track #6468 -- Limit number of concurrent processes submitted by the Scheduler
        private int _maximumConcurrentProcesses;
        //End Track #6468 -- Limit number of concurrent processes submitted by the Scheduler
        private ProcessWindowStyle _startedProcessWindowStyle;
        private string _forecastCommand;
        private string _allocateCommand;
        private string _rollupCommand;
        //Begin TT#155 - JScott - Size Curve Method
        private string _sizeCurveGenerateCommand;
        //End TT#155 - JScott - Size Curve Method
        private string _purgeCommand;
        private string _relieveIntransitCommand;
        //Begin MOD - JScott - Build Pack Criteria Load
        private string _buildPackCriteriaLoadCommand;
        //End MOD - JScott - Build Pack Criteria Load
        private string _colorCodesLoadCommand;
        private string _headerReconcileCommand;
        private string _headerLoadCommand;
        private string _hierarchyLoadCommand;
        private string _historyPlanLoadCommand;
        private string _sizeCodesLoadCommand;
        private string _sizeCurveLoadCommand;
        private string _sizeConstraintsLoadCommand;
        private string _storeLoadCommand;
        //Begin TT#391 - stodd - size day to week summary
        private string _sizeDayToWeekSummaryCommand;
        //End TT#391 - stodd - size day to week summary
        private string _computationDriverCommand;
        private string _chainSetPercentCriteriaLoadCommand;  // TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
        private string _pushToBackStockLoadCommand;  // TT#1401 - AGallagher - VSW
        private string _dailyPercentagesCriteriaLoadCommand; // TT#43 - MD - DOConnell - Projected Sales Enhancement
        private string _StoreEligibilityCriteriaLoadCommand;
        private string _VSWCriteriaLoadCommand;
        private string _batchCompCommand;   // TT#1613-MD - stodd - batch comp API


        // BEGIN TT#1766 - stodd - FIFO processing
        private eAPIFileProcessingDirection _FileProcessingDirection;
        // END TT#1766 - stodd - FIFO processing

        public SchedulerConfigInfo()
        {
            string scheduleScanIntervalStr;
            string checkForTerminateIntervalStr;
            //Begin Track #6468 -- Limit number of concurrent processes submitted by the Scheduler
            string maximumConcurrentProcessesStr;
            //End Track #6468 -- Limit number of concurrent processes submitted by the Scheduler
            string startedProcessWindowStyleStr;
            string errMessage;

            try
            {
                scheduleScanIntervalStr = MIDConfigurationManager.AppSettings["ScheduleScanInterval"];

                if (scheduleScanIntervalStr == null)
                {
                    _scheduleScanInterval = cScheduleScanInterval;
                }
                else
                {
                    _scheduleScanInterval = Convert.ToInt32(scheduleScanIntervalStr);
                }

                checkForTerminateIntervalStr = MIDConfigurationManager.AppSettings["CheckForTerminateInterval"];

                if (checkForTerminateIntervalStr == null)
                {
                    _checkForTerminateInterval = cCheckForTerminateInterval;
                }
                else
                {
                    _checkForTerminateInterval = Convert.ToInt32(checkForTerminateIntervalStr);
                }

                if (_scheduleScanInterval < _checkForTerminateInterval)
                {
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ScanIntervalMustBeLarger, MIDText.GetText(eMIDTextCode.msg_ScanIntervalMustBeLarger));
                }

                // BEGIN TT#1766 - stodd - FIFO processing
                string fileProcessingDirection = MIDConfigurationManager.AppSettings["APIFileProcessingDirection"];
                if (fileProcessingDirection == null)
                {
                    _FileProcessingDirection = eAPIFileProcessingDirection.Default;
                }
                else
                {
                    switch (fileProcessingDirection.ToUpper().Trim())
                    {
                        case "FIFO":
                            _FileProcessingDirection = eAPIFileProcessingDirection.FIFO;
                            break;
                        case "FILO":
                            _FileProcessingDirection = eAPIFileProcessingDirection.FILO;
                            break;
                        default:
                            _FileProcessingDirection = eAPIFileProcessingDirection.Default;
                            break;
                    }
                }
                // END TT#1766 - stodd - FIFO processing


                //Begin Track #6468 -- Limit number of concurrent processes submitted by the Scheduler
                maximumConcurrentProcessesStr = MIDConfigurationManager.AppSettings["MaximumConcurrentProcesses"];

                if (maximumConcurrentProcessesStr == null)
                {
                    _maximumConcurrentProcesses = cMaximumConcurrentProcessesStr;
                }
                else
                {
                    _maximumConcurrentProcesses = Convert.ToInt32(maximumConcurrentProcessesStr);
                }

                //End Track #6468 -- Limit number of concurrent processes submitted by the Scheduler
                startedProcessWindowStyleStr = MIDConfigurationManager.AppSettings["StartedProcessWindowStyle"];

                if (startedProcessWindowStyleStr == null)
                {
                    _startedProcessWindowStyle = cStartedProcessWindowStyle;
                }
                else
                {
                    switch (startedProcessWindowStyleStr.ToLower())
                    {
                        case "normal":
                            _startedProcessWindowStyle = ProcessWindowStyle.Normal;
                            break;
                        case "hidden":
                            _startedProcessWindowStyle = ProcessWindowStyle.Hidden;
                            break;
                        case "maximized":
                            _startedProcessWindowStyle = ProcessWindowStyle.Maximized;
                            break;
                        case "minimized":
                            _startedProcessWindowStyle = ProcessWindowStyle.Minimized;
                            break;
                        default:
                            _startedProcessWindowStyle = cStartedProcessWindowStyle;
                            break;
                    }
                }

                _forecastCommand = MIDConfigurationManager.AppSettings["ForecastExecutablePath"];

                if (_forecastCommand == null || !System.IO.File.Exists(_forecastCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "ForecastExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                _allocateCommand = MIDConfigurationManager.AppSettings["AllocateExecutablePath"];

                if (_allocateCommand == null || !System.IO.File.Exists(_allocateCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "AllocateExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                _rollupCommand = MIDConfigurationManager.AppSettings["RollupExecutablePath"];

                if (_rollupCommand == null || !System.IO.File.Exists(_rollupCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "RollupExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                //Begin TT#155 - JScott - Size Curve Method
                _sizeCurveGenerateCommand = MIDConfigurationManager.AppSettings["SizeCurveGenerateExecutablePath"];

                if (_sizeCurveGenerateCommand == null || !System.IO.File.Exists(_sizeCurveGenerateCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "SizeCurveGenerateExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                //End TT#155 - JScott - Size Curve Method
                _purgeCommand = MIDConfigurationManager.AppSettings["PurgeExecutablePath"];

                if (_purgeCommand == null || !System.IO.File.Exists(_purgeCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "PurgeExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                _relieveIntransitCommand = MIDConfigurationManager.AppSettings["RelieveIntransitExecutablePath"];

                if (_relieveIntransitCommand == null || !System.IO.File.Exists(_relieveIntransitCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "RelieveIntransitExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                //Begin MOD - JScott - Build Pack Criteria Load
                _buildPackCriteriaLoadCommand = MIDConfigurationManager.AppSettings["BuildPackCriteriaLoadExecutablePath"];

                if (_buildPackCriteriaLoadCommand == null || !System.IO.File.Exists(_buildPackCriteriaLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "BuildPackCriteriaLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                //End MOD - JScott - Build Pack Criteria Load
                _colorCodesLoadCommand = MIDConfigurationManager.AppSettings["ColorCodesLoadExecutablePath"];

                if (_colorCodesLoadCommand == null || !System.IO.File.Exists(_colorCodesLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "ColorCodesLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                // Begin TT#1581-MD - stodd - Header Reconcile API
                _headerReconcileCommand = MIDConfigurationManager.AppSettings["HeaderReconcileExecutablePath"];

                if (_headerReconcileCommand == null || !System.IO.File.Exists(_headerReconcileCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "HeaderReconcileExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }
                // End TT#1581-MD - stodd - Header Reconcile API

                // Begin TT#1613-MD - stodd - batch comp API
                _batchCompCommand = MIDConfigurationManager.AppSettings["BatchCompExecutablePath"];

                if (_batchCompCommand == null || !System.IO.File.Exists(_batchCompCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "BatchCompExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }
                // End TT#1613-MD - stodd - batch comp API


                _headerLoadCommand = MIDConfigurationManager.AppSettings["HeaderLoadExecutablePath"];

                if (_headerLoadCommand == null || !System.IO.File.Exists(_headerLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "HeaderLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                _hierarchyLoadCommand = MIDConfigurationManager.AppSettings["HierarchyLoadExecutablePath"];

                if (_hierarchyLoadCommand == null || !System.IO.File.Exists(_hierarchyLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "HierarchyLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                // BEGIN TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
                _chainSetPercentCriteriaLoadCommand = MIDConfigurationManager.AppSettings["ChainSetPercentCriteriaLoadExecutablePath"];

                if (_chainSetPercentCriteriaLoadCommand == null || !System.IO.File.Exists(_chainSetPercentCriteriaLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "ChainSetPercentCriteriaLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }
                // END TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4

                // BEGIN TT#1401 - AGallagher - VSW
                _pushToBackStockLoadCommand = MIDConfigurationManager.AppSettings["PushToBackStockLoadExecutablePath"];

                if (_pushToBackStockLoadCommand == null || !System.IO.File.Exists(_pushToBackStockLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "PushToBackStcokLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }
                // END TT#1401 - AGallagher - VSW

                // BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                _dailyPercentagesCriteriaLoadCommand = MIDConfigurationManager.AppSettings["DailyPercentagesCriteriaLoadExecutablePath"];

                if (_dailyPercentagesCriteriaLoadCommand == null || !System.IO.File.Exists(_dailyPercentagesCriteriaLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "DailyPercentagesCriteriaLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }
                // END TT#43 - MD - DOConnell - Projected Sales Enhancement

                //BEGIN TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
                _StoreEligibilityCriteriaLoadCommand = MIDConfigurationManager.AppSettings["StoreEligibilityCriteriaLoadExecutablePath"];

                if (_StoreEligibilityCriteriaLoadCommand == null || !System.IO.File.Exists(_StoreEligibilityCriteriaLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "StoreEligibilityCriteriaLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                _VSWCriteriaLoadCommand = MIDConfigurationManager.AppSettings["VSWCriteriaLoadExecutablePath"];

                if (_VSWCriteriaLoadCommand == null || !System.IO.File.Exists(_VSWCriteriaLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "VSWCriteriaLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }
                //END TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API

                _historyPlanLoadCommand = MIDConfigurationManager.AppSettings["HistoryPlanLoadExecutablePath"];

                if (_historyPlanLoadCommand == null || !System.IO.File.Exists(_historyPlanLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "HistoryPlanLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                _sizeCodesLoadCommand = MIDConfigurationManager.AppSettings["SizeCodesLoadExecutablePath"];

                if (_sizeCodesLoadCommand == null || !System.IO.File.Exists(_sizeCodesLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "SizeCodesLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                _sizeCurveLoadCommand = MIDConfigurationManager.AppSettings["SizeCurveLoadExecutablePath"];

                if (_sizeCurveLoadCommand == null || !System.IO.File.Exists(_sizeCurveLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "SizeCurveLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                _sizeConstraintsLoadCommand = MIDConfigurationManager.AppSettings["SizeConstraintsLoadExecutablePath"];

                if (_sizeConstraintsLoadCommand == null || !System.IO.File.Exists(_sizeConstraintsLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "SizeConstraintsLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                _storeLoadCommand = MIDConfigurationManager.AppSettings["StoreLoadExecutablePath"];

                if (_storeLoadCommand == null || !System.IO.File.Exists(_storeLoadCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "StoreLoadExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }

                // Begin TT#391 - stodd - size day to week summary
                _sizeDayToWeekSummaryCommand = MIDConfigurationManager.AppSettings["SizeDayToWeekSummaryExecutablePath"];

                if (_sizeDayToWeekSummaryCommand == null || !System.IO.File.Exists(_sizeDayToWeekSummaryCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "SizeDayToWeekSummaryExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }
                // End TT#391 - stodd - size day to week summary

                _computationDriverCommand = MIDConfigurationManager.AppSettings["ComputationDriverExecutablePath"];

                if (_computationDriverCommand == null || !System.IO.File.Exists(_computationDriverCommand))
                {
                    errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), "ComputationDriverExecutablePath");
                    throw new MIDException(eErrorLevel.severe, (int)eMIDTextCode.msg_ExecutableNotFound, errMessage);
                }
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public int ScheduleScanInterval
        {
            get
            {
                return _scheduleScanInterval;
            }
        }

        public int CheckForTerminateInterval
        {
            get
            {
                return _checkForTerminateInterval;
            }
        }

        // BEGIN TT#1766 - stodd - FIFO processing
        public eAPIFileProcessingDirection APIFileProcessingDirection
        {
            get
            {
                return _FileProcessingDirection;
            }
        }
        // BEGIN TT#1766 - stodd - FIFO processing

        //Begin Track #6468 -- Limit number of concurrent processes submitted by the Scheduler
        public int MaximumConcurrentProcesses
        {
            get
            {
                return _maximumConcurrentProcesses;
            }
        }

        //End Track #6468 -- Limit number of concurrent processes submitted by the Scheduler
        public ProcessWindowStyle StartedProcessWindowStyle
        {
            get
            {
                return _startedProcessWindowStyle;
            }
        }

        public string ForecastCommand
        {
            get
            {
                return _forecastCommand;
            }
        }

        public string AllocateCommand
        {
            get
            {
                return _allocateCommand;
            }
        }

        public string RollupCommand
        {
            get
            {
                return _rollupCommand;
            }
        }

        //Begin TT#155 - JScott - Size Curve Method
        public string SizeCurveGenerateCommand
        {
            get
            {
                return _sizeCurveGenerateCommand;
            }
        }

        //End TT#155 - JScott - Size Curve Method
        public string PurgeCommand
        {
            get
            {
                return _purgeCommand;
            }
        }

        public string RelieveIntransitCommand
        {
            get
            {
                return _relieveIntransitCommand;
            }
        }

        //Begin MOD - JScott - Build Pack Criteria Load
        public string BuildPackCriteriaLoadCommand
        {
            get
            {
                return _buildPackCriteriaLoadCommand;
            }
        }

        //End MOD - JScott - Build Pack Criteria Load
        public string ColorCodesLoadCommand
        {
            get
            {
                return _colorCodesLoadCommand;
            }
        }

        public string HeaderReconcileCommand
        {
            get
            {
                return _headerReconcileCommand;
            }
        }

        // Begin TT#1613-MD - stodd - batch comp API
        public string BatchCompCommand
        {
            get
            {
                return _batchCompCommand;
            }
        }
        // End TT#1613-MD - stodd - batch comp API

        public string HeaderLoadCommand
        {
            get
            {
                return _headerLoadCommand;
            }
        }

        public string HierarchyLoadCommand
        {
            get
            {
                return _hierarchyLoadCommand;
            }
        }

        // BEGIN TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
        public string ChainSetPercentCriteriaLoadCommand
        {
            get
            {
                return _chainSetPercentCriteriaLoadCommand;
            }
        }
        // END TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4

        // BEGIN TT#1401 - AGallagher - VSW
        public string PushToBackStockLoadCommand
        {
            get
            {
                return _pushToBackStockLoadCommand;
            }
        }
        // END TT#1401 - AGallagher - VSW

        // BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
        public string DailyPercentagesCriteriaLoadCommand
        {
            get
            {
                return _dailyPercentagesCriteriaLoadCommand;
            }
        }
        // END TT#43 - MD - DOConnell - Projected Sales Enhancement

        // BEGIN TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
        public string StoreEligibilityCriteriaLoadCommand
        {
            get
            {
                return _StoreEligibilityCriteriaLoadCommand;
            }
        }
        // END TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API

        // BEGIN TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
        public string VSWCriteriaLoadCommand
        {
            get
            {
                return _VSWCriteriaLoadCommand;
            }
        }
        // END TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API

        public string HistoryPlanLoadCommand
        {
            get
            {
                return _historyPlanLoadCommand;
            }
        }

        public string SizeCodesLoadCommand
        {
            get
            {
                return _sizeCodesLoadCommand;
            }
        }

        public string SizeCurveLoadCommand
        {
            get
            {
                return _sizeCurveLoadCommand;
            }
        }

        public string SizeConstraintsLoadCommand
        {
            get
            {
                return _sizeConstraintsLoadCommand;
            }
        }

        public string StoreLoadCommand
        {
            get
            {
                return _storeLoadCommand;
            }
        }

        //Begin TT#391 - stodd - size day to week summary
        public string SizeDayToWeekSummaryCommand
        {
            get
            {
                return _sizeDayToWeekSummaryCommand;
            }
        }
        //End TT#391 - stodd - size day to week summary

        public string ComputationDriverCommand
        {
            get
            {
                return _computationDriverCommand;
            }
        }
    }

    /// <summary>
    /// JobProcessor is a class that contains functionality to process a Job.
    /// </summary>

    public class JobProcessor
    {
        //=======
        // FIELDS
        //=======

        private SchedulerConfigInfo _schedConfigInfo;
        private Audit _audit;
        private int _jobRID;
        private ScheduleData _dlSchedule;
        private bool _isRunning;
        private eProcessCompletionStatus _completionStatus;
        private eMIDMessageLevel _highestTaskMsgLevel = eMIDMessageLevel.None;  // TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
        private DateTime _completionDateTime;
        private Thread _thread;

        //=============
        // CONSTRUCTORS
        //=============

        public JobProcessor(SchedulerConfigInfo aSchedConfigInfo, Audit aAudit, int aJobRID)
        {
            try
            {
                _schedConfigInfo = aSchedConfigInfo;
                _audit = aAudit;
                _jobRID = aJobRID;
                _dlSchedule = new ScheduleData();
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "JobProcessor");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========

        public bool isRunning
        {
            get
            {
                try
                {
                    return _isRunning;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CompletionStatus:Get", "JobProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }

        public DateTime CompletionDateTime
        {
            get
            {
                try
                {
                    return _completionDateTime;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CompletionStatus:Get", "JobProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }

        public eProcessCompletionStatus CompletionStatus
        {
            get
            {
                try
                {
                    return _completionStatus;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CompletionStatus:Get", "JobProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }

        // Begin TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
        public eMIDMessageLevel HighestTaskMsgLevel
        {
            get
            {
                try
                {
                    return _highestTaskMsgLevel;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in HighestTaskMsgLevel:Get", "JobProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }
        // End TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job

        public int JobRID
        {
            get
            {
                try
                {
                    return _jobRID;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in JobRID:Get", "JobProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }

        //========
        // METHODS
        //========

        public void AbortThread()
        {
            try
            {
                _thread.Abort();
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in AbortThread", "JobProcessor");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        public void WaitForThreadExit()
        {
            try
            {
                _thread.Join();
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in WaitForThreadExit", "JobProcessor");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        public void ExecuteJobInThread()
        {
            try
            {
                _thread = new Thread(new ThreadStart(ExecuteJob));
                _thread.Start();
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteJobInThread", "JobProcessor");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        public void ExecuteJob()
        {
            JobProfile jobProf = null;   // TT#4291 - JSnith - Update scheduler messages to be consistent.
            DataTable dtTaskLists;
            TaskListProcessor taskListProc;
            eProcessCompletionStatus taskListStatus = eProcessCompletionStatus.Successful;

            try
            {
                _isRunning = true;

                jobProf = new JobProfile(_dlSchedule.Job_Read(_jobRID));
                dtTaskLists = _dlSchedule.TaskList_ReadByJob(_jobRID);

                _audit.Add_Msg(eMIDMessageLevel.Information, "Starting Job #" + _jobRID + " (" + jobProf.Name + ")", "JobProcessor");

                foreach (DataRow row in dtTaskLists.Rows)
                {
                    taskListProc = new TaskListProcessor(_schedConfigInfo, _audit, Convert.ToInt32(row["TASKLIST_RID"]));
                    taskListProc.ExecuteTaskList();
                    taskListStatus = taskListProc.CompletionStatus;

                    // Begin TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
                    if (taskListProc.HighestTaskMsgLevel > _highestTaskMsgLevel)
                    {
                        _highestTaskMsgLevel = taskListProc.HighestTaskMsgLevel;
                    }
                    // End TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job

                    if (taskListStatus == eProcessCompletionStatus.Failed)
                    {
                        return;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                _audit.Add_Msg(eMIDMessageLevel.Warning, "Job was cancelled by User", "CommandProcessor");

                taskListStatus = eProcessCompletionStatus.Cancelled;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteJob", "JobProcessor");
                _audit.Log_Exception(exc);

                taskListStatus = eProcessCompletionStatus.Failed;
            }
            finally
            {
                _isRunning = false;
                _completionDateTime = DateTime.Now;
                _completionStatus = taskListStatus;

                // Begin TT#4291 - JSnith - Update scheduler messages to be consistent.
                //_audit.Add_Msg(
                //    CommonScheduleRoutines.DetermineMessageLevelFromCompletionCode(taskListStatus),
                //    "Completed Job #" + _jobRID + " with status " + MIDText.GetTextOnly((int)taskListStatus),
                //    "JobProcessor");
                if (jobProf == null)
                {
                    _audit.Add_Msg(
                        CommonScheduleRoutines.DetermineMessageLevelFromCompletionCode(taskListStatus),
                        "Completed Job #" + _jobRID + " with status " + MIDText.GetTextOnly((int)taskListStatus),
                        "JobProcessor");
                }
                else
                {
                    _audit.Add_Msg(
                        CommonScheduleRoutines.DetermineMessageLevelFromCompletionCode(taskListStatus),
                        "Completed Job #" + _jobRID + " (" + jobProf.Name + ")" + " with status " + MIDText.GetTextOnly((int)taskListStatus),
                        "JobProcessor");
                }
                // End TT#4291 - JSnith - Update scheduler messages to be consistent.
            }
        }
    }

    /// <summary>
    /// TaskListProcessor is a class that contains functionality to process a TaskList.
    /// </summary>

    public class TaskListProcessor
    {
        //=======
        // FIELDS
        //=======

        private SchedulerConfigInfo _schedConfigInfo;
        private Audit _audit;
        private int _taskListRID;
        private ScheduleData _dlSchedule;
        private bool _isRunning;
        private eMIDMessageLevel _taskMsgLevel;
        private eMIDMessageLevel _highestTaskMsgLevel = eMIDMessageLevel.None;  // TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
        private DateTime _completionDateTime;
        private eProcessCompletionStatus _completionStatus;

        //=============
        // CONSTRUCTORS
        //=============

        public TaskListProcessor(SchedulerConfigInfo aSchedConfigInfo, Audit aAudit, int aTaskListRID)
        {
            try
            {
                _schedConfigInfo = aSchedConfigInfo;
                _audit = aAudit;
                _taskListRID = aTaskListRID;
                _dlSchedule = new ScheduleData();
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "TaskListProcessor");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========

        public bool isRunning
        {
            get
            {
                try
                {
                    return _isRunning;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CompletionDateTime:Get", "TaskListProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }

        public DateTime CompletionDateTime
        {
            get
            {
                try
                {
                    return _completionDateTime;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CompletionDateTime:Get", "TaskListProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }

        public eProcessCompletionStatus CompletionStatus
        {
            get
            {
                try
                {
                    return _completionStatus;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CompletionStatus:Get", "TaskListProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }

        // Begin TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
        public eMIDMessageLevel HighestTaskMsgLevel
        {
            get
            {
                try
                {
                    return _highestTaskMsgLevel;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CompletionStatus:Get", "TaskListProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }
        // End TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job


        //========
        // METHODS
        //========

        public void ExecuteTaskList()
        {
            TaskListProfile taskListProf = null;  // TT#4291 - JSnith - Update scheduler messages to be consistent.
            DataTable dtTasks;
            int taskSequence;
            //Begin TT#859 - JScott - Max Return set to Error - task did not abort
            int maxMsgLevel;
            //End TT#859 - JScott - Max Return set to Error - task did not abort
            eProcessCompletionStatus taskStatus = eProcessCompletionStatus.Successful;
            CommandProcessor commandProc;
            DataRow dtExternalProg;
            string extProgPath;
            string extProgParms;

            try
            {
                _isRunning = true;

                taskListProf = new TaskListProfile(_dlSchedule.TaskList_Read(_taskListRID));
                dtTasks = _dlSchedule.Task_ReadByTaskList(_taskListRID);

                _audit.Add_Msg(eMIDMessageLevel.Information, "Starting TaskList #" + _taskListRID + " (" + taskListProf.Name + ")", "TaskListProcessor");
                _taskMsgLevel = eMIDMessageLevel.None;

                foreach (DataRow row in dtTasks.Rows)
                {
                    taskSequence = Convert.ToInt32(row["TASK_SEQUENCE"]);
                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                    maxMsgLevel = Convert.ToInt32(row["MAX_MESSAGE_LEVEL"]);
                    //End TT#859 - JScott - Max Return set to Error - task did not abort

                    switch ((eTaskType)Convert.ToInt32(row["TASK_TYPE"]))
                    {
                        case eTaskType.Allocate:
                            //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                            //							commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.AllocateCommand, _taskListRID + " " + taskSequence);
                            commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.AllocateCommand, _taskListRID + " " + taskSequence, true, true);
                            //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                            commandProc.ExecuteCommand();
                            _taskMsgLevel = commandProc.MessageLevel;
                            break;

                        case eTaskType.Forecasting:
                            //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                            //							commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.ForecastCommand, _taskListRID + " " + taskSequence);
                            commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.ForecastCommand, _taskListRID + " " + taskSequence, true, true);
                            //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                            commandProc.ExecuteCommand();
                            _taskMsgLevel = commandProc.MessageLevel;
                            break;

                        case eTaskType.computationDriver:
                            //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                            //							commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.ComputationDriverCommand, _taskListRID + " " + taskSequence);
                            commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.ComputationDriverCommand, _taskListRID + " " + taskSequence, true, true);
                            //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                            commandProc.ExecuteCommand();
                            _taskMsgLevel = commandProc.MessageLevel;
                            break;

                        // Begin TT#1581-MD - stodd - Header Reconcile API
                        case eTaskType.HeaderReconcile:
                            commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.HeaderReconcileCommand, _taskListRID + " " + taskSequence, true, true);
                            commandProc.ExecuteCommand();
                            _taskMsgLevel = commandProc.MessageLevel;
                            break;
                        // End TT#1581-MD - stodd - Header Reconcile API

                        // Begin TT#1612-MD - stodd - batch comp API
                        case eTaskType.BatchComp:
                            commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.BatchCompCommand, _taskListRID + " " + taskSequence, true, true);
                            commandProc.ExecuteCommand();
                            _taskMsgLevel = commandProc.MessageLevel;
                            break;
                        // End TT#1612-MD - stodd - batch comp API


                        case eTaskType.Purge:
                            //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                            //							commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.PurgeCommand, String.Empty);
                            commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.PurgeCommand, String.Empty, true, true);
                            //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                            commandProc.ExecuteCommand();
                            _taskMsgLevel = commandProc.MessageLevel;
                            break;

                        // BEGIN TT#1401 - AGallagher - VSW
                        case eTaskType.PushToBackStockLoad:
                            commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.PushToBackStockLoadCommand, String.Empty, true, true);
                            commandProc.ExecuteCommand();
                            _taskMsgLevel = commandProc.MessageLevel;
                            break;
                        // END TT#1401 - AGallagher - VSW

                        case eTaskType.Rollup:
                            //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                            //							commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.RollupCommand, _taskListRID + " " + taskSequence);
                            commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.RollupCommand, _taskListRID + " " + taskSequence, true, true);
                            //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                            commandProc.ExecuteCommand();
                            _taskMsgLevel = commandProc.MessageLevel;
                            break;

                        //Begin TT#155 - JScott - Size Curve Method
                        case eTaskType.SizeCurveMethod:
                        //Begin TT#155 - JScott - Add Size Curve info to Node Properties
                        //case eTaskType.SizeCurve:
                        case eTaskType.SizeCurves:
                            //End TT#155 - JScott - Add Size Curve info to Node Properties
                            commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.SizeCurveGenerateCommand, _taskListRID + " " + taskSequence, true, true);
                            commandProc.ExecuteCommand();
                            _taskMsgLevel = commandProc.MessageLevel;
                            break;

                        //End TT#155 - JScott - Size Curve Method

                        // Begin TT#391 - stodd - size day to week summary
                        case eTaskType.SizeDayToWeekSummary:
                            commandProc = new CommandProcessor(_schedConfigInfo, _audit, _schedConfigInfo.SizeDayToWeekSummaryCommand, _taskListRID + " " + taskSequence, true, true);
                            commandProc.ExecuteCommand();
                            _taskMsgLevel = commandProc.MessageLevel;
                            break;
                        // End TT#391 - stodd - size day to week summary

                        case eTaskType.ExternalProgram:
                            dtExternalProg = _dlSchedule.TaskProgram_Read(_taskListRID, taskSequence);

                            extProgPath = Convert.ToString(dtExternalProg["PROGRAM_PATH"]);

                            if (dtExternalProg["PROGRAM_PARMS"] != System.DBNull.Value)
                            {
                                extProgParms = Convert.ToString(dtExternalProg["PROGRAM_PARMS"]);
                            }
                            else
                            {
                                extProgParms = String.Empty;
                            }

                            if (System.IO.File.Exists(extProgPath))
                            {
                                //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                                //								commandProc = new CommandProcessor(_schedConfigInfo, _audit, extProgPath, extProgParms);
                                // Begin Track #6442 - stodd - fof external programs, the SchedulerID prefix should not be applied.
                                commandProc = new CommandProcessor(_schedConfigInfo, _audit, extProgPath, extProgParms, false, false);
                                // End Track #6442 - stodd - fof external programs, the SchedulerID prefix should not be applied.
                                //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                                commandProc.ExecuteCommand();
                                _taskMsgLevel = commandProc.MessageLevel;
                            }
                            else
                            {
                                _audit.Add_Msg(eMIDMessageLevel.Severe, String.Format(MIDText.GetText(eMIDTextCode.msg_ExecutableNotFound), extProgPath), "CommandProcessor");
                                _taskMsgLevel = eMIDMessageLevel.Severe;
                            }
                            break;

                        case eTaskType.RelieveIntransit:
                        //Begin MOD - JScott - Build Pack Criteria Load
                        case eTaskType.BuildPackCriteriaLoad:
                        //End MOD - JScott - Build Pack Criteria Load
                        case eTaskType.ColorCodeLoad:
                        case eTaskType.HeaderLoad:
                        case eTaskType.HierarchyLoad:
                        case eTaskType.HistoryPlanLoad:
                        case eTaskType.SizeCodeLoad:
                        case eTaskType.SizeCurveLoad:
                        case eTaskType.SizeConstraintsLoad:
                        case eTaskType.StoreLoad:
                        case eTaskType.ChainSetPercentCriteriaLoad:  // TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
                        case eTaskType.DailyPercentagesCriteriaLoad: // TT#43 - MD - DOConnell - Projected Sales Enhancement //TT#816 - MD - DOConnell - corrected misspelling
                        case eTaskType.StoreEligibilityCriteriaLoad: // TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
                        case eTaskType.VSWCriteriaLoad: //TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API

                            switch ((eTaskType)Convert.ToInt32(row["TASK_TYPE"]))
                            {
                                case eTaskType.RelieveIntransit:
                                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                                    //_taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.RelieveIntransitCommand);
                                    // BEGIN TT#1766 - stodd - FIFO/FILO processing
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.RelieveIntransitCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#1766 - stodd - FIFO/FILO processing
                                    //End TT#859 - JScott - Max Return set to Error - task did not abort
                                    break;
                                //Begin MOD - JScott - Build Pack Criteria Load
                                case eTaskType.BuildPackCriteriaLoad:
                                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                                    //_taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.BuildPackCriteriaLoadCommand);
                                    // BEGIN TT#1766 - stodd - FIFO/FILO processing
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.BuildPackCriteriaLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#1766 - stodd - FIFO/FILO processing
                                    //End TT#859 - JScott - Max Return set to Error - task did not abort
                                    break;
                                //End MOD - JScott - Build Pack Criteria Load
                                case eTaskType.ColorCodeLoad:
                                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                                    //_taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.ColorCodesLoadCommand);
                                    // BEGIN TT#1766 - stodd - FIFO/FILO processing
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.ColorCodesLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#1766 - stodd - FIFO/FILO processing
                                    //End TT#859 - JScott - Max Return set to Error - task did not abort
                                    break;
                                case eTaskType.HeaderLoad:
                                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                                    //_taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.HeaderLoadCommand);
                                    // BEGIN TT#1766 - stodd - FIFO/FILO processing
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.HeaderLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#1766 - stodd - FIFO/FILO processing
                                    //End TT#859 - JScott - Max Return set to Error - task did not abort
                                    break;
                                case eTaskType.HierarchyLoad:
                                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                                    //_taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.HierarchyLoadCommand);
                                    // BEGIN TT#1766 - stodd - FIFO/FILO processing
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.HierarchyLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#1766 - stodd - FIFO/FILO processing
                                    //End TT#859 - JScott - Max Return set to Error - task did not abort
                                    break;
                                // BEGIN TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
                                case eTaskType.ChainSetPercentCriteriaLoad:
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.ChainSetPercentCriteriaLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    break;
                                // END TT#1501 - AGallagher - Chain Plan - Set Percentages - Phase 4
                                case eTaskType.HistoryPlanLoad:
                                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                                    //_taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.HistoryPlanLoadCommand);
                                    // BEGIN TT#1766 - stodd - FIFO/FILO processing
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.HistoryPlanLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#1766 - stodd - FIFO/FILO processing
                                    //End TT#859 - JScott - Max Return set to Error - task did not abort
                                    break;
                                case eTaskType.SizeCodeLoad:
                                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                                    //_taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.SizeCodesLoadCommand);
                                    // BEGIN TT#1766 - stodd - FIFO/FILO processing
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.SizeCodesLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#1766 - stodd - FIFO/FILO processing
                                    //End TT#859 - JScott - Max Return set to Error - task did not abort
                                    break;
                                case eTaskType.SizeCurveLoad:
                                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                                    //_taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.SizeCurveLoadCommand);
                                    // BEGIN TT#1766 - stodd - FIFO/FILO processing
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.SizeCurveLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#1766 - stodd - FIFO/FILO processing
                                    //End TT#859 - JScott - Max Return set to Error - task did not abort
                                    break;
                                case eTaskType.SizeConstraintsLoad:
                                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                                    //_taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.SizeConstraintsLoadCommand);
                                    // BEGIN TT#1766 - stodd - FIFO/FILO processing
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.SizeConstraintsLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#1766 - stodd - FIFO/FILO processing
                                    //End TT#859 - JScott - Max Return set to Error - task did not abort
                                    break;
                                case eTaskType.StoreLoad:
                                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                                    //_taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.StoreLoadCommand);
                                    // BEGIN TT#1766 - stodd - FIFO/FILO processing
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.StoreLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#1766 - stodd - FIFO/FILO processing
                                    //End TT#859 - JScott - Max Return set to Error - task did not abort
                                    break;
                                case eTaskType.DailyPercentagesCriteriaLoad: //TT#816 - MD - DOConnell - corrected misspelling
                                    // BEGIN TT#43 - MD - DOConnell - Projected Sales Enhancement
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.DailyPercentagesCriteriaLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#43 - MD - DOConnell - Projected Sales Enhancement
                                    break;
                                case eTaskType.StoreEligibilityCriteriaLoad:
                                    // BEGIN TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.StoreEligibilityCriteriaLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
                                    break;
                                case eTaskType.VSWCriteriaLoad:
                                    // BEGIN TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
                                    _taskMsgLevel = ProcessPostingTask(_taskListRID, Convert.ToInt32(row["TASK_SEQUENCE"]), _schedConfigInfo.VSWCriteriaLoadCommand, maxMsgLevel, _schedConfigInfo.APIFileProcessingDirection);
                                    // END TT#816 - MD - DOConnell - Node Properties Maintenance Enhancement - Phase 1 - Store Eligibility Load API
                                    break;
                            }

                            break;
                    }


                    // Begin TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job
                    if (_taskMsgLevel > _highestTaskMsgLevel)
                    {
                        _highestTaskMsgLevel = _taskMsgLevel;
                    }
                    // End TT#4073 - JSmith - Modify Schedule Interface to return actual highest message level across all tasks in all task lists for the job

                    //Begin TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                    if (_taskMsgLevel == eMIDMessageLevel.Cancelled)
                    {
                        //Do not email canceled tasks.
                    }
                    else
                    {
                        //this task was "successful"
                        _audit.EmailScheduledTasks(row, _taskMsgLevel, taskListProf.Name);
                    }
                    //End TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application



                    if (_taskMsgLevel == eMIDMessageLevel.Cancelled)
                    {
                        taskStatus = eProcessCompletionStatus.Cancelled;
                        return;
                    }




                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                    //else if ((int)_taskMsgLevel > Convert.ToInt32(row["MAX_MESSAGE_LEVEL"]))
                    // Begin TT#1127 - JSmith - Severe error didn't roll up to Scheduler Service
                    //else if ((int)_taskMsgLevel > maxMsgLevel)
                    else if ((int)_taskMsgLevel >= maxMsgLevel)
                    //End TT#859 - JScott - Max Return set to Error - task did not abort
                    {
                        taskStatus = eProcessCompletionStatus.Failed;
                        return;
                    }
                }
            }
            catch (ThreadAbortException)
            {
                _audit.Add_Msg(eMIDMessageLevel.Warning, "TaskList was cancelled by User", "CommandProcessor");

                taskStatus = eProcessCompletionStatus.Cancelled;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteTaskList", "TaskListProcessor");
                _audit.Log_Exception(exc);

                taskStatus = eProcessCompletionStatus.Failed;
            }
            finally
            {
                _isRunning = false;
                _completionDateTime = DateTime.Now;
                _completionStatus = taskStatus;

                // Begin TT#4291 - JSnith - Update scheduler messages to be consistent.
                //_audit.Add_Msg(
                //    CommonScheduleRoutines.DetermineMessageLevelFromCompletionCode(taskStatus),
                //    "Completed TaskList #" + _taskListRID + " with status " + MIDText.GetTextOnly((int)taskStatus) + " (" + MIDText.GetTextOnly((int)_taskMsgLevel) + ")",
                //    "TaskListProcessor");
                if (taskListProf == null)
                {
                    _audit.Add_Msg(
                    CommonScheduleRoutines.DetermineMessageLevelFromCompletionCode(taskStatus),
                    "Completed TaskList #" + _taskListRID + " with status " + MIDText.GetTextOnly((int)taskStatus) + " (" + MIDText.GetTextOnly((int)_taskMsgLevel) + ")",
                    "TaskListProcessor");
                }
                else
                {
                    _audit.Add_Msg(
                    CommonScheduleRoutines.DetermineMessageLevelFromCompletionCode(taskStatus),
                    "Completed TaskList #" + _taskListRID + " (" + taskListProf.Name + ")" + " with status " + MIDText.GetTextOnly((int)taskStatus) + " (" + MIDText.GetTextOnly((int)_taskMsgLevel) + ")",
                    "TaskListProcessor");
                }
                // End TT#4291 - JSnith - Update scheduler messages to be consistent.
            }
        }

        // BEGIN TT#1766 - stodd - FIFO file processing 
        private eMIDMessageLevel ProcessPostingTask(int aTaskListRID, int aTaskSequence, string aCommand, int aMaxMsgLevel)
        {
            return ProcessPostingTask(aTaskListRID, aTaskSequence, aCommand, aMaxMsgLevel, eAPIFileProcessingDirection.Default);
        }
        // END TT#1766 - stodd - FIFO file processing 

        //Begin TT#859 - JScott - Max Return set to Error - task did not abort
        //private eMIDMessageLevel ProcessPostingTask(int aTaskListRID, int aTaskSequence, string aCommand)
        // BEGIN TT#1766 - stodd - FIFO file processing 
        private eMIDMessageLevel ProcessPostingTask(int aTaskListRID, int aTaskSequence, string aCommand, int aMaxMsgLevel, eAPIFileProcessingDirection direction)
        // END TT#1766 - stodd - FIFO file processing 
        //End TT#859 - JScott - Max Return set to Error - task did not abort
        {
            DataRow postingRow;
            CommandProcessor[] commandProcArray = null;
            int concurrentFiles;
            string inputDir;
            string fileMask;
            bool runUntil;
            string runUntilMask = null;
            string[] runUntilFileList = null;
            eMIDMessageLevel maxMessageLevel;
            Stack inputFileStack;
            Hashtable flagFileHash;
            int inputFilesProcessed;
            bool loop;
            int i;
            int lastIndex;
            string flagFile;
            string inputFile;
            string errMessage;


            try
            {
                postingRow = _dlSchedule.TaskPosting_Read(_taskListRID, aTaskSequence);

                concurrentFiles = Convert.ToInt32(postingRow["CONCURRENT_FILES"]);
                inputDir = Convert.ToString(postingRow["INPUT_DIRECTORY"]);

                fileMask = Convert.ToString(postingRow["FILE_MASK"]);
                if (fileMask[0] != '.')
                {
                    fileMask = "." + fileMask;
                }

                runUntil = postingRow["RUN_UNTIL_FILE_PRESENT_IND"].ToString() == "1";
                if (runUntil)
                {
                    runUntilMask = CommonScheduleRoutines.FormatExtension(Convert.ToString(postingRow["RUN_UNTIL_FILE_MASK"]));
                }

                // Begin TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files
                eAPIFileProcessingDirection DBdirection = (eAPIFileProcessingDirection)Convert.ToInt32(postingRow["FILE_PROCESSING_DIRECTION"]);
                if (DBdirection != eAPIFileProcessingDirection.Config)
                {
                    direction = DBdirection;
                }
                // End TT#645-MD - JSmith - Add File Processing Direction Parameter to Tasks on Task Lists that Process Input Files

                commandProcArray = new CommandProcessor[concurrentFiles];
                inputFileStack = new Stack();
                flagFileHash = new Hashtable();
                maxMessageLevel = eMIDMessageLevel.None;
                loop = true;
                inputFilesProcessed = 0;

                //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                try
                {
                    //End TT#859 - JScott - Max Return set to Error - task did not abort
                    while (loop)
                    {
                        if (runUntil)
                        {
                            //Begin TT#1281 - JScott - WUB header load failed
                            //if (System.IO.Directory.GetFiles(inputDir, runUntilMask).Length > 0)
                            if (Include.GetFiles(inputDir, runUntilMask).Length > 0)
                            //End TT#1281 - JScott - WUB header load failed
                            {
                                loop = false;
                            }
                        }
                        else
                        {
                            loop = false;
                        }

                        // Begin TT#3521 - JSmith - Header Load delay
                        // if no processes are running, clear flag hash to removed orphaned file flags.
                        bool isProcessRunning = false;
						// Begin TT#5308 - JSmith - Issue with Task List using the Run Until File Present Feature
                        //for (i = 0; i < concurrentFiles; i++)
                        //{
                        //    if (commandProcArray[i] != null && commandProcArray[i].isRunning)
                        //    {
                        //        isProcessRunning = true;
                        //    }
                        //}
                        //if (!isProcessRunning)
                        //{
                        //    lock (flagFileHash.SyncRoot)
                        //    {
                        //        flagFileHash.Clear();
                        //    }
                        //}
                        lock (flagFileHash.SyncRoot)
                        {
                            for (i = 0; i < concurrentFiles; i++)
                            {
                                if (commandProcArray[i] != null && commandProcArray[i].isRunning)
                                {
                                    isProcessRunning = true;
                                }
                            }
                            if (!isProcessRunning)
                            {
                                flagFileHash.Clear();
                            }
                        }
						// End TT#5308 - JSmith - Issue with Task List using the Run Until File Present Feature
                        // End TT#3521 - JSmith - Header Load delay

                        // BEGIN TT#1766 - stodd - FIFO file processing 
                        ReadInputFileList(inputFileStack, flagFileHash, inputDir, fileMask, direction);
                        // END TT#1766 - stodd - FIFO file processing 

                        if (inputFileStack.Count > 0)
                        {
                            while (inputFileStack.Count > 0)
                            {
                                for (i = 0; i < concurrentFiles && inputFileStack.Count > 0; i++)
                                {
                                    if (commandProcArray[i] == null || !commandProcArray[i].isRunning)
                                    {
                                        if (commandProcArray[i] != null)
                                        {
                                            maxMessageLevel = (eMIDMessageLevel)Math.Max((int)maxMessageLevel, (int)commandProcArray[i].MessageLevel);
                                            // Begin TT#1833 - JSmith - the scheduler is not running the Header Load is not executing even though the run until is specified.
                                            ClearFlagFileHash(flagFileHash, commandProcArray[i]);
                                            // End TT#1833
                                            commandProcArray[i] = null;
                                            //Begin TT#859 - JScott - Max Return set to Error - task did not abort

                                            // Begin TT#1127 - JSmith - Severe error didn't roll up to Scheduler Service
                                            //if ((int)maxMessageLevel > aMaxMsgLevel)
                                            if ((int)maxMessageLevel >= aMaxMsgLevel)
                                            // End TT#1127
                                            {
                                                throw new MaxMsgLevelExceeded();
                                            }
                                            //End TT#859 - JScott - Max Return set to Error - task did not abort
                                        }

                                        flagFile = (string)inputFileStack.Pop();
                                        // Begin TT#4595 - JSmith - Input files and trigger files should not be case sensitive
                                        //lastIndex = flagFile.LastIndexOf(fileMask);
                                        lastIndex = flagFile.LastIndexOf(fileMask, StringComparison.CurrentCultureIgnoreCase);
                                        // End TT#4595 - JSmith - Input files and trigger files should not be case sensitive

                                        if (lastIndex >= 0)
                                        {
                                            inputFile = flagFile.Remove(lastIndex, fileMask.Length);

                                            if (System.IO.File.Exists(inputFile))
                                            {
                                                //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                                                //											commandProcArray[i] = new CommandProcessor(_schedConfigInfo, _audit, aCommand, "\"" + inputFile + "\"", flagFile);
												// Begin TT#5308 - JSmith - Issue with Task List using the Run Until File Present Feature
                                                lock (flagFileHash.SyncRoot)
                                                {
												// End TT#5308 - JSmith - Issue with Task List using the Run Until File Present Feature
                                                    commandProcArray[i] = new CommandProcessor(_schedConfigInfo, _audit, aCommand, "\"" + inputFile + "\"", true, flagFile, true);
                                                    //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                                                    commandProcArray[i].ExecuteCommandInThread();
												// Begin TT#5308 - JSmith - Issue with Task List using the Run Until File Present Feature
                                                }
												// End TT#5308 - JSmith - Issue with Task List using the Run Until File Present Feature
                                                inputFilesProcessed++;
                                            }
                                            else
                                            {
                                                errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_DataFileNotFound), flagFile, inputFile);
                                                _audit.Add_Msg(eMIDMessageLevel.Error, errMessage, "TaskListProcessor");
                                                maxMessageLevel = (eMIDMessageLevel)Math.Max((int)maxMessageLevel, (int)eMIDMessageLevel.NothingToDo);
                                                //Begin TT#859 - JScott - Max Return set to Error - task did not abort

                                                // Begin TT#1127 - JSmith - Severe error didn't roll up to Scheduler Service
                                                //if ((int)maxMessageLevel > aMaxMsgLevel)
                                                if ((int)maxMessageLevel >= aMaxMsgLevel)
                                                // End TT#1127
                                                {
                                                    throw new MaxMsgLevelExceeded();
                                                }
                                                //End TT#859 - JScott - Max Return set to Error - task did not abort
                                            }
                                        }
                                        else
                                        {
                                            errMessage = String.Format(MIDText.GetText(eMIDTextCode.msg_SuffixIsInvalid), fileMask);
                                            _audit.Add_Msg(eMIDMessageLevel.Error, errMessage, "TaskListProcessor");
                                            maxMessageLevel = (eMIDMessageLevel)Math.Max((int)maxMessageLevel, (int)eMIDMessageLevel.NothingToDo);
                                            //Begin TT#859 - JScott - Max Return set to Error - task did not abort

                                            // Begin TT#1127 - JSmith - Severe error didn't roll up to Scheduler Service
                                            //if ((int)maxMessageLevel > aMaxMsgLevel)
                                            if ((int)maxMessageLevel >= aMaxMsgLevel)
                                            // End TT#1127
                                            {
                                                throw new MaxMsgLevelExceeded();
                                            }
                                            //End TT#859 - JScott - Max Return set to Error - task did not abort
                                        }
                                    }
                                }

                                if (inputFileStack.Count > 0)
                                {
                                    System.Threading.Thread.Sleep(_schedConfigInfo.CheckForTerminateInterval);
                                }
                            }
                        }
                        else
                        {
                            //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                            for (i = 0; i < concurrentFiles; i++)
                            {
                                if (commandProcArray[i] != null && !commandProcArray[i].isRunning)
                                {
                                    maxMessageLevel = (eMIDMessageLevel)Math.Max((int)maxMessageLevel, (int)commandProcArray[i].MessageLevel);
                                    // Begin TT#1833 - JSmith - the scheduler is not running the Header Load is not executing even though the run until is specified.
                                    ClearFlagFileHash(flagFileHash, commandProcArray[i]);
                                    // End TT#1833
                                    commandProcArray[i] = null;

                                    // Begin TT#1127 - JSmith - Severe error didn't roll up to Scheduler Service
                                    //if ((int)maxMessageLevel > aMaxMsgLevel)
                                    if ((int)maxMessageLevel >= aMaxMsgLevel)
                                    // End TT#1127
                                    {
                                        throw new MaxMsgLevelExceeded();
                                    }
                                }
                            }

                            //End TT#859 - JScott - Max Return set to Error - task did not abort
                            if (loop)
                            {
                                System.Threading.Thread.Sleep(_schedConfigInfo.CheckForTerminateInterval);
                            }
                        }
                    }
                    //Begin TT#859 - JScott - Max Return set to Error - task did not abort
                }
                catch (MaxMsgLevelExceeded)
                {
                }
                catch (Exception)
                {
                    throw;
                }
                //End TT#859 - JScott - Max Return set to Error - task did not abort

                if (inputFilesProcessed > 0)
                {
                    if (commandProcArray != null)
                    {
                        for (i = 0; i < concurrentFiles; i++)
                        {
                            if (commandProcArray[i] != null)
                            {
                                commandProcArray[i].WaitForThreadExit();
                                maxMessageLevel = (eMIDMessageLevel)Math.Max((int)maxMessageLevel, (int)commandProcArray[i].MessageLevel);
                                // Begin TT#1833 - JSmith - the scheduler is not running the Header Load is not executing even though the run until is specified.
                                ClearFlagFileHash(flagFileHash, commandProcArray[i]);
                                // End TT#1833
                                commandProcArray[i] = null;
                            }
                        }
                    }
                }
                else
                {
                    maxMessageLevel = eMIDMessageLevel.NothingToDo;
                }

                if (runUntil)
                {
                    //Begin TT#1281 - JScott - WUB header load failed
                    //runUntilFileList = System.IO.Directory.GetFiles(inputDir, runUntilMask);
                    runUntilFileList = Include.GetFiles(inputDir, runUntilMask);
                    //End TT#1281 - JScott - WUB header load failed

                    foreach (string file in runUntilFileList)
                    {
                        System.IO.File.Delete(file);
                    }
                }

                return maxMessageLevel;
            }
            catch (ThreadAbortException)
            {
                if (commandProcArray != null)
                {
                    foreach (CommandProcessor commandProc in commandProcArray)
                    {
                        if (commandProc != null)
                        {
                            commandProc.AbortThread();
                            commandProc.WaitForThreadExit();
                        }
                    }
                }

                _audit.Add_Msg(eMIDMessageLevel.Warning, "TaskList was cancelled by User", "CommandProcessor");

                return eMIDMessageLevel.Cancelled;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ProcessPostingTask", "TaskListProcessor");
                _audit.Log_Exception(exc);

                return eMIDMessageLevel.Severe;
            }
        }

        // Begin TT#1833 - JSmith - the scheduler is not running the Header Load is not executing even though the run until is specified.
        private void ClearFlagFileHash(Hashtable flagFileHash, CommandProcessor commandProcessor)
        {
            lock (flagFileHash.SyncRoot)
            {
                flagFileHash.Remove(commandProcessor.FlagFile);
            }
        }
        // End TT#1833

        // BEGIN TT#1766 - stodd - FIFO file processing 
        private void ReadInputFileList(Stack aFileStack, Hashtable aFlagFileHash, string aInputDir, string aFileMask)
        {
            ReadInputFileList(aFileStack, aFlagFileHash, aInputDir, aFileMask, eAPIFileProcessingDirection.Default);
        }
        // END TT#1766 - stodd - FIFO file processing 

        // BEGIN TT#1766 - stodd - FIFO file processing 
        private void ReadInputFileList(Stack aFileStack, Hashtable aFlagFileHash, string aInputDir, string aFileMask, eAPIFileProcessingDirection direction)
        // END TT#1766 - stodd - FIFO file processing 
        {
            string[] fileList;

            try
            {
                //Begin TT#1281 - JScott - WUB header load failed
                //fileList = System.IO.Directory.GetFiles(aInputDir, CommonScheduleRoutines.FormatExtension(aFileMask));
                // BEGIN TT#1766 - stodd - FIFO file processing 
                fileList = Include.GetFiles(aInputDir, CommonScheduleRoutines.FormatExtension(aFileMask), direction);
                // END TT#1766 - stodd - FIFO file processing 
                //End TT#1281 - JScott - WUB header load failed

                // Begin TT#1833 - JSmith - the scheduler is not running the Header Load is not executing even though the run until is specified.
                //foreach (string file in fileList)
                //{
                //    if (!aFlagFileHash.Contains(file))
                //    {
                //        aFlagFileHash.Add(file, null);
                //        aFileStack.Push(file);
                //    }
                //}
                lock (aFlagFileHash.SyncRoot)
                {
                    foreach (string file in fileList)
                    {
                        if (!aFlagFileHash.Contains(file))
                        {
                            aFlagFileHash.Add(file, null);
                            aFileStack.Push(file);
                        }
                    }
                }
                // End TT#1833
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ReadInputFileList", "JobProcessor");
                _audit.Log_Exception(exc);
                throw;
            }
        }
        //Begin TT#859 - JScott - Max Return set to Error - task did not abort

        public class MaxMsgLevelExceeded : Exception
        {
        }
        //Begin TT#859 - JScott - Max Return set to Error - task did not abort
    }

    /// <summary>
    /// CommandProcessor is a class that contains functionality to process a command-line call.
    /// </summary>

    public class CommandProcessor
    {
        //=======
        // FIELDS
        //=======

        private SchedulerConfigInfo _schedConfigInfo;
        private Audit _audit;
        private string _command;
        private string _parms;
        //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
        private bool _dequeueAfterCancel;
        //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
        private string _flagFile;
        //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
        private int _processId;
        //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
        private Process _process;
        private bool _isRunning;
        private bool _isStarted = false;    // TT#5308 - JSmith - Issue with Task List using the Run Until File Present Feature
        private eMIDMessageLevel _commandMsgLevel;
        private DateTime _completionDateTime;
        private Thread _thread;

        //=============
        // CONSTRUCTORS
        //=============

        //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
        //		public CommandProcessor(SchedulerConfigInfo aSchedConfigInfo, Audit aAudit, string aCommand, string aParms)
        public CommandProcessor(SchedulerConfigInfo aSchedConfigInfo, Audit aAudit, string aCommand, string aParms, bool aDequeueAfterCancel, bool prefixSchedulerID)
        //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
        {
            //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
            ScheduleData dlSchedule;

            //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
            try
            {
                _schedConfigInfo = aSchedConfigInfo;
                _audit = aAudit;
                _command = aCommand;
                //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                //				_parms = aParms;
                // Begin Track #6442 - stodd - fof external programs, the SchedulerID prefix should not be applied.
                if (prefixSchedulerID)
                    _parms = Include.SchedulerID + " " + aParms;
                // End Track #6442 - stodd - fof external programs, the SchedulerID prefix should not be applied.
                // Begin TT#595 - JSmith - Running External .EXE with Parameters no longer works
                else
                {
                    _parms = aParms;
                }
                // End TT#595
                _dequeueAfterCancel = aDequeueAfterCancel;
                //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                _flagFile = string.Empty;
                //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

                if (aDequeueAfterCancel)
                {
                    dlSchedule = new ScheduleData();
                    dlSchedule.OpenUpdateConnection();

                    try
                    {
                        _processId = dlSchedule.GetNextScheduleProcessId();
                        dlSchedule.CommitData();
                    }
                    catch (Exception err)
                    {
                        dlSchedule.Rollback();
                        string message = err.ToString();
                        throw;
                    }
                    finally
                    {
                        dlSchedule.CloseUpdateConnection();
                    }

                    _parms += " " + _processId;
                }
                //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandProcessor");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
        //		public CommandProcessor(SchedulerConfigInfo aSchedConfigInfo, Audit aAudit, string aCommand, string aParms, string aFlagFile)
        public CommandProcessor(SchedulerConfigInfo aSchedConfigInfo, Audit aAudit, string aCommand, string aParms, bool aDequeueAfterCancel, string aFlagFile, bool prefixSchedulerID)
        //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
        {
            //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
            ScheduleData dlSchedule;

            //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
            try
            {
                _schedConfigInfo = aSchedConfigInfo;
                _audit = aAudit;
                _command = aCommand;
                //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                //				_parms = aParms;
                // Begin Track #6442 - stodd - fof external programs, the SchedulerID prefix should not be applied.
                if (prefixSchedulerID)
                    _parms = Include.SchedulerID + " " + aParms;
                // End Track #6442 - stodd - fof external programs, the SchedulerID prefix should not be applied.
                // Begin TT#595 - JSmith - Running External .EXE with Parameters no longer works
                else
                {
                    _parms = aParms;
                }
                // End TT#595
                _dequeueAfterCancel = aDequeueAfterCancel;
                //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                _flagFile = aFlagFile;
                //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

                if (aDequeueAfterCancel)
                {
                    dlSchedule = new ScheduleData();
                    dlSchedule.OpenUpdateConnection();

                    try
                    {
                        _processId = dlSchedule.GetNextScheduleProcessId();
                        dlSchedule.CommitData();
                    }
                    catch (Exception err)
                    {
                        dlSchedule.Rollback();
                        string message = err.ToString();
                        throw;
                    }
                    finally
                    {
                        dlSchedule.CloseUpdateConnection();
                    }

                    _parms += " " + _processId;
                }
                //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "CommandProcessor");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========

        public bool isRunning
        {
            get
            {
                try
                {
                    return _isRunning;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in isRunning:Get", "CommandProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }

        // Begin TT#5308 - JSmith - Issue with Task List using the Run Until File Present Feature
        public bool isStarted
        {
            get
            {
                try
                {
                    return _isStarted;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in isRunning:Get", "CommandProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }
		// End TT#5308 - JSmith - Issue with Task List using the Run Until File Present Feature

        public DateTime CompletionDateTime
        {
            get
            {
                try
                {
                    return _completionDateTime;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in CompletionTime:Get", "CommandProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }

        public eMIDMessageLevel MessageLevel
        {
            get
            {
                try
                {
                    return _commandMsgLevel;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in MessageLevel:Get", "CommandProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }

        // Begin TT#1833 - JSmith - the scheduler is not running the Header Load is not executing even though the run until is specified.
        public string FlagFile
        {
            get
            {
                try
                {
                    return _flagFile;
                }
                catch (ThreadAbortException exc)
                {
                    string message = exc.ToString();
                    throw;
                }
                catch (Exception exc)
                {
                    _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in MessageLevel:Get", "CommandProcessor");
                    _audit.Log_Exception(exc);
                    throw;
                }
            }
        }
        // End TT#1833

        //========
        // METHODS
        //========

        public void AbortThread()
        {
            try
            {
                _thread.Abort();
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in AbortThread", "CommandProcessor");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        public void WaitForThreadExit()
        {
            try
            {
                _thread.Join();
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in WaitForThreadExit", "CommandProcessor");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        public void ExecuteCommandInThread()
        {
            try
            {
                _thread = new Thread(new ThreadStart(ExecuteCommand));
                _thread.Start();

                // Begin TT#5308 - JSmith - Issue with Task List using the Run Until File Present Feature
                // wait to make sure thread has started
                while (!isStarted)
                {
                    System.Threading.Thread.Sleep(1000);
                }
				// End TT#5308 - JSmith - Issue with Task List using the Run Until File Present Feature
            }
            catch (ThreadAbortException exc)
            {
                string message = exc.ToString();
                throw;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteCommandInThread", "CommandProcessor");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        public void ExecuteCommand()
        {
            eMIDMessageLevel messageLevel = eMIDMessageLevel.None;
            //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
            MIDEnqueue midNQ;
            //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

            try
            {
                _isRunning = true;
                _isStarted = true;    // TT#5308 - JSmith - Issue with Task List using the Run Until File Present Feature

                _audit.Add_Msg(eMIDMessageLevel.Information, "Executing Command '" + _command + " " + _parms + "'", "JobProcessor");

                _process = new Process();
                _process.StartInfo.FileName = _command;
                _process.StartInfo.Arguments = _parms;
                _process.StartInfo.WindowStyle = _schedConfigInfo.StartedProcessWindowStyle;
                _process.Start();

                while (!_process.HasExited)
                {
                    System.Threading.Thread.Sleep(_schedConfigInfo.CheckForTerminateInterval);
                }

                messageLevel = ConvertExitCodeToMessageLevel(_process.ExitCode);

                if (_flagFile != string.Empty)
                {
                    System.IO.File.Delete(_flagFile);
                }
            }
            catch (ThreadAbortException)
            {
                if (_process != null)
                {
                    try
                    {
                        _process.Kill();
                        //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
                        _process.WaitForExit();

                        if (_dequeueAfterCancel)
                        {
                            midNQ = new MIDEnqueue();
                            midNQ.OpenUpdateConnection();

                            try
                            {
                                midNQ.Enqueue_DeleteAll_ByProcess(_processId);
                                midNQ.CommitData();
                            }
                            catch (Exception err)
                            {
                                midNQ.Rollback();
                                string message = err.ToString();
                                throw;
                            }
                            finally
                            {
                                midNQ.CloseUpdateConnection();
                            }
                        }
                        //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"

                        _audit.Add_Msg(eMIDMessageLevel.Warning, "Command was cancelled by User", "CommandProcessor");

                        messageLevel = eMIDMessageLevel.Cancelled;
                    }
                    catch (InvalidOperationException)
                    {
                        messageLevel = ConvertExitCodeToMessageLevel(_process.ExitCode);

                        if (_flagFile != string.Empty)
                        {
                            System.IO.File.Delete(_flagFile);
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ExecuteCommand", "CommandProcessor");
                _audit.Log_Exception(exc);

                messageLevel = eMIDMessageLevel.Severe;
            }
            finally
            {
                _isRunning = false;
                _completionDateTime = DateTime.Now;
                _commandMsgLevel = messageLevel;

                _audit.Add_Msg(eMIDMessageLevel.Information, "Completed command '" + _command + " " + _parms + "' with Message Level " + MIDText.GetTextOnly((int)_commandMsgLevel), "CommandProcessor");
            }
        }

        private eMIDMessageLevel ConvertExitCodeToMessageLevel(int aExitCode)
        {
            try
            {
                if (aExitCode <= (int)eMIDMessageLevel.Severe && aExitCode >= (int)eMIDMessageLevel.None)
                {
                    return (eMIDMessageLevel)aExitCode;
                }
                else
                {
                    return eMIDMessageLevel.Severe;
                }
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ConvertExitCodeToMessageLevel", "CommandProcessor");
                _audit.Log_Exception(exc);
                throw;
            }
        }
    }

    //Begin Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
    /// <summary>
    /// QueuedJobEntry is a class that contains information queue job.
    /// </summary>

    public class QueuedJobEntry
    {
        //=======
        // FIELDS
        //=======

        private JobProcessor _jobProc;
        private DataRow _jobRow;

        //=============
        // CONSTRUCTORS
        //=============

        public QueuedJobEntry(JobProcessor aJobProc, DataRow aJobRow)
        {
            _jobProc = aJobProc;
            _jobRow = aJobRow;
        }

        //===========
        // PROPERTIES
        //===========

        public JobProcessor JobProcessor
        {
            get
            {
                return _jobProc;
            }
        }

        public DataRow JobRow
        {
            get
            {
                return _jobRow;
            }
        }

        //========
        // METHODS
        //========
    }

    //End Track #6468 - JScott - Limit number of concurrent processes submitted by the Scheduler
    /// <summary>
    /// ScheduleProcessInfo is a class that contains information about all schedule processes.
    /// </summary>

    public class ScheduleProcessInfo
    {
        //=======
        // FIELDS
        //=======

        private Audit _audit;
        private ScheduleData _dlSchedule;
        private Hashtable _hashTable;

        //=============
        // CONSTRUCTORS
        //=============

        public ScheduleProcessInfo(Audit aAudit, ScheduleData aScheduleData)
        {
            try
            {
                _audit = aAudit;
                _dlSchedule = aScheduleData;
                _hashTable = new Hashtable();
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in Constructor", "ScheduleProcessInfo");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        //===========
        // PROPERTIES
        //===========

        //========
        // METHODS
        //========

        public JobProcessor AddJobProcessor(SchedulerConfigInfo aSchedConfigInfo, int aSchedRID, int aJobRID)
        {
            ScheduleProcessEntry schedProcEnt;
            JobProcessor jobProc;

            try
            {
                schedProcEnt = GetScheduleProcessEntry(aSchedRID, aJobRID);
                jobProc = new JobProcessor(aSchedConfigInfo, _audit, aJobRID);
                schedProcEnt.JobProcessor = jobProc;

                return jobProc;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in AddJobProcessor", "ScheduleProcessInfo");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        public JobProcessor GetJobProcessor(int aSchedRID, int aJobRID)
        {
            ScheduleProcessEntry schedProcEnt;

            try
            {
                schedProcEnt = GetScheduleProcessEntry(aSchedRID, aJobRID);

                return schedProcEnt.JobProcessor;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in GetJobProcessor", "ScheduleProcessInfo");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        public ScheduleProfile GetScheduleProfile(int aSchedRID, int aJobRID)
        {
            ScheduleProcessEntry schedProcEnt;
            DataRow schedRow;
            ScheduleProfile schedProf;

            try
            {
                schedProcEnt = GetScheduleProcessEntry(aSchedRID, aJobRID);

                if (schedProcEnt.ScheduleProfile == null)
                {
                    schedRow = _dlSchedule.Schedule_Read(aSchedRID);

                    if (schedRow != null)
                    {
                        schedProf = new ScheduleProfile(schedRow);
                    }
                    else
                    {
                        schedProf = null;
                    }

                    schedProcEnt.ScheduleProfile = schedProf;
                }

                return schedProcEnt.ScheduleProfile;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in GetScheduleProfile", "ScheduleProcessInfo");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        public void ClearJobProcessor(int aSchedRID, int aJobRID)
        {
            ScheduleProcessEntry schedProcEnt;

            try
            {
                schedProcEnt = GetScheduleProcessEntry(aSchedRID, aJobRID);

                if (schedProcEnt != null)
                {
                    schedProcEnt.JobProcessor = null;
                }
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ClearJobProcessor", "ScheduleProcessInfo");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        public void ClearScheduleProfile(int aSchedRID, int aJobRID)
        {
            ScheduleProcessEntry schedProcEnt;

            try
            {
                schedProcEnt = GetScheduleProcessEntry(aSchedRID, aJobRID);

                if (schedProcEnt != null)
                {
                    schedProcEnt.ScheduleProfile = null;
                }
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in ClearScheduleProfile", "ScheduleProcessInfo");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        private ScheduleProcessEntry GetScheduleProcessEntry(int aSchedRID, int aJobRID)
        {
            int hashKey;
            ScheduleProcessEntry schedProcEnt;

            try
            {
                hashKey = GetScheduleJobHashKey(aSchedRID, aJobRID);
                schedProcEnt = (ScheduleProcessEntry)_hashTable[hashKey];

                if (schedProcEnt == null)
                {
                    schedProcEnt = new ScheduleProcessEntry();
                    _hashTable.Add(hashKey, schedProcEnt);
                }

                return schedProcEnt;
            }
            catch (Exception exc)
            {
                _audit.Add_Msg(eMIDMessageLevel.Severe, "Exception encountered in GetScheduleProcessEntry", "ScheduleProcessInfo");
                _audit.Log_Exception(exc);
                throw;
            }
        }

        private int GetScheduleJobHashKey(int aSchedRID, int aJobRID)
        {
            return ((aSchedRID & 0xFFFF) << 16) | (aJobRID & 0xFFFF);
        }
    }

    /// <summary>
    /// ScheduleProcessEntry is a class that contains information about a schedule process.
    /// </summary>

    public class ScheduleProcessEntry
    {
        //=======
        // FIELDS
        //=======

        private ScheduleProfile _schedProf;
        private JobProcessor _jobProc;

        //=============
        // CONSTRUCTORS
        //=============

        public ScheduleProcessEntry()
        {
        }

        //===========
        // PROPERTIES
        //===========

        public ScheduleProfile ScheduleProfile
        {
            get
            {
                return _schedProf;
            }
            set
            {
                _schedProf = value;
            }
        }

        public JobProcessor JobProcessor
        {
            get
            {
                return _jobProc;
            }
            set
            {
                _jobProc = value;
            }
        }

        //========
        // METHODS
        //========
    }

    /// <summary>
    /// ScheduleProcessEntry is a class that contains information about a schedule process.
    /// </summary>

    [Serializable]
    public class ScheduleKey
    {
        //=======
        // FIELDS
        //=======

        private int _schedRID;
        private int _jobRID;

        //=============
        // CONSTRUCTORS
        //=============

        public ScheduleKey(int aSchedRID, int aJobRID)
        {
            _schedRID = aSchedRID;
            _jobRID = aJobRID;
        }

        //===========
        // PROPERTIES
        //===========

        public int SchedRID
        {
            get
            {
                return _schedRID;
            }
        }

        public int JobRID
        {
            get
            {
                return _jobRID;
            }
        }

        //========
        // METHODS
        //========
    }

    /// <summary>
    /// SchedulerServerSession is a class that contains fields, properties, and methods that are available to other sessions
    /// of the system.
    /// </summary>
    /// <remarks>
    /// The SchedulerServerSession class is the interface to the SchedulerServer functionality.  All requests for functionality
    /// or information in the SchedulerServer should be made through methods and properties in this class.
    /// </remarks>

    //Begin TT#708 - JScott - Services need a Retry availalbe.
    //public class SchedulerServerSession : Session
    public class SchedulerServerSessionRemote : SessionRemote
    //End TT#708 - JScott - Services need a Retry availalbe.
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        /// <summary>
        /// Creates a new instance of SchedulerSessionGlobal as either local or remote, depending on the value of aLocal
        /// </summary>
        /// <param name="aLocal">
        /// A boolean that indicates whether this class is being instantiated in the Client or in a remote service.
        /// </param>

        //Begin TT#708 - JScott - Services need a Retry availalbe.
        //public SchedulerServerSession(bool aLocal)
        public SchedulerServerSessionRemote(bool aLocal)
            //Begin TT#708 - JScott - Services need a Retry availalbe.
            : base(aLocal)
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

        public SchedulerConfigInfo SchedConfigInfo
        {
            get
            {
                return SchedulerServerGlobal.SchedConfigInfo;
            }
        }

        //========
        // METHODS
        //========

        public override void Initialize()
        {
            try
            {
                Calendar = SchedulerServerGlobal.Calendar;
                // Begin TT#1808-MD - JSmith - Store Load Error
                ExceptionHandler.Initialize(SessionAddressBlock.SchedulerServerSession, false);
                // End TT#1808-MD - JSmith - Store Load Error
            }
            catch
            {
                throw;
            }
        }

        // Begin TT#195 MD - JSmith - Add environment authentication
        public void VerifyEnvironment(ClientProfile aClientProfile)
        {
            try
            {
                SchedulerServerGlobal.VerifyEnvironment(aClientProfile);
            }
            catch
            {
                throw;
            }
        }
        // End TT#195 MD

        public void CleanUpGlobal()
        {
            try
            {
                SchedulerServerGlobal.EndProcessScheduleThread();
                SchedulerServerGlobal.CleanUp();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        protected override void ExpiredCleanup()
        {
            // Begin TT#1243 - JSmith - Audit Performance
            base.ExpiredCleanup();
            // End TT#1243
        }

        // Begin TT#1243 - JSmith - Audit Performance
        /// <summary>
        /// Clean up the global resources
        /// </summary>
        public override void CloseSession()
        {
            try
            {
                base.CloseSession();
            }
            catch
            {
                throw;
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
                SchedulerServerGlobal.CloseAudit();
            }
            catch
            {
                throw;
            }
        }
        // End TT#1243

        public DataTable GetSchedule()
        {
            try
            {
                return SchedulerServerGlobal.Schedule;
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin Alert Events Code -- DO NOT REMOVE
        public void ScheduleExistingJob(ScheduleProfile aSchedProf, int aJobRID, int aUserRID)
        //		public void ScheduleExistingJob(ScheduleProfile aSchedProf, int aJobRID, int aUserRID, JobFinishAlertEvent aJobFinishAlertEvent)
        // End Alert Events Code -- DO NOT REMOVE
        {
            try
            {
                // Begin Alert Events Code -- DO NOT REMOVE
                SchedulerServerGlobal.ScheduleExistingJob(aSchedProf, aJobRID, aUserRID);
                //				SchedulerServerGlobal.ScheduleExistingJob(aSchedProf, aJobRID, aUserRID, aJobFinishAlertEvent);
                // End Alert Events Code -- DO NOT REMOVE
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin Alert Events Code -- DO NOT REMOVE
        public void ScheduleNewJob(ScheduleProfile aSchedProf, JobProfile aJobProf, int aTaskListRID, int aUserRID)
        //		public void ScheduleNewJob(ScheduleProfile aSchedProf, JobProfile aJobProf, int aTaskListRID, int aUserRID, JobFinishAlertEvent aJobFinishAlertEvent)
        // End Alert Events Code -- DO NOT REMOVE
        {
            try
            {
                // Begin Alert Events Code -- DO NOT REMOVE
                SchedulerServerGlobal.ScheduleNewJob(aSchedProf, aJobProf, aTaskListRID, aUserRID);
                //				SchedulerServerGlobal.ScheduleNewJob(aSchedProf, aJobProf, aTaskListRID, aUserRID, aJobFinishAlertEvent);
                // End Alert Events Code -- DO NOT REMOVE
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void UpdateSchedule(ScheduleProfile aSchedProf, int aJobRID, int aUserRID)
        {
            try
            {
                SchedulerServerGlobal.UpdateSchedule(aSchedProf, aJobRID, aUserRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public DateTime GetNextRunDate(ScheduleProfile aSchedProf, int aJobRID)
        {
            try
            {
                return SchedulerServerGlobal.GetNextRunDate(aSchedProf, aJobRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public eProcessExecutionStatus GetJobStatus(int aSchedRID, int aJobRID)
        {
            try
            {
                return SchedulerServerGlobal.GetJobStatus(aSchedRID, aJobRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

		// Begin TT#1386-MD - stodd - Scheduler Job Manager
        public DataTable GetScheduledJobsForJobManager(string scheduleName, string jobName, int userRID)
        {
            try
            {
                return SchedulerServerGlobal.GetScheduledJobsForJobManager(scheduleName, jobName, userRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }
		// End TT#1386-MD - stodd - Scheduler Job Manager

        public void HoldJob(int aSchedRID, int aJobRID, int aUserRID)	// TT#1386-MD - stodd - Scheduler Job Manager
        {
            try
            {
                SchedulerServerGlobal.HoldJob(aSchedRID, aJobRID, aUserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public bool isJobInCycle(ScheduleProfile aSchedProf, int aJobRID)
        {
            try
            {
                return SchedulerServerGlobal.isJobInCycle(aSchedProf, aJobRID);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public ArrayList HoldAllJobs(TaskListProfile aTaskListProf)
        {
            try
            {
                return SchedulerServerGlobal.HoldAllJobs(aTaskListProf);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public ArrayList HoldAllJobs(JobProfile aJobProf)
        {
            try
            {
                return SchedulerServerGlobal.HoldAllJobs(aJobProf);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public ArrayList HoldAllJobs(SpecialRequestProfile aSpecialRequestProf)
        {
            try
            {
                return SchedulerServerGlobal.HoldAllJobs(aSpecialRequestProf);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin Alert Events Code -- DO NOT REMOVE
        public void RunJobNow(int aSchedRID, int aJobRID)
        //		public void RunJobNow(int aSchedRID, int aJobRID, JobFinishAlertEvent aJobFinishAlertEvent)
        // End Alert Events Code -- DO NOT REMOVE
        {
            try
            {
                // Begin Alert Events Code -- DO NOT REMOVE
                SchedulerServerGlobal.RunJobNow(aSchedRID, aJobRID);
                //				SchedulerServerGlobal.RunJobNow(aSchedRID, aJobRID, aJobFinishAlertEvent);
                // End Alert Events Code -- DO NOT REMOVE
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void CancelJob(int aSchedRID, int aJobRID, eProcessExecutionStatus aStatusAtCancel)
        {
            try
            {
                SchedulerServerGlobal.CancelJob(aSchedRID, aJobRID, aStatusAtCancel);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void ResumeJob(int aSchedRID, int aJobRID, int aUserRID)	// TT#1386-MD - stodd - Scheduler Job Manager
        {
            try
            {
                SchedulerServerGlobal.ResumeJob(aSchedRID, aJobRID, aUserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void ResumeAllJobs(ArrayList aHeldSchedules, int aUserRid)	// TT#1386-MD - stodd - Scheduler Job Manager
        {
            try
            {
                SchedulerServerGlobal.ResumeAllJobs(aHeldSchedules, aUserRid);	// TT#1386-MD - stodd - Scheduler Job Manager
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public bool DeleteSchedulesFromList(DataTable aTable)
        {
            try
            {
                return SchedulerServerGlobal.DeleteSchedulesFromList(aTable);
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        public void Refresh()
        {
            try
            {
                RefreshBase();
            }
            catch (Exception exc)
            {
                string message = exc.ToString();
                throw;
            }
        }

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                return SchedulerServerGlobal.ServiceProfile;
            }
            catch
            {
                throw;
            }
        }
        // Begin TT#195 MD
    }
    //Begin TT#708 - JScott - Services need a Retry availalbe.

    [Serializable]
    public class SchedulerServerSession : Session
    {
        //=======
        // FIELDS
        //=======

        //=============
        // CONSTRUCTORS
        //=============

        public SchedulerServerSession(SchedulerServerSessionRemote aSessionRemote, int aServiceRetryCount, int aServiceRetryInterval)
            : base(aSessionRemote, eProcesses.schedulerService, aServiceRetryCount, aServiceRetryInterval)
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

        public SchedulerConfigInfo SchedConfigInfo
        {
            get
            {
                try
                {
                    for (int i = 0; i < ServiceRetryCount; i++)
                    {
                        try
                        {
                            return SchedulerServerSessionRemote.SchedConfigInfo;
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
                        SchedulerServerSessionRemote.Initialize();
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

        public void CleanUpGlobal()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        SchedulerServerSessionRemote.CleanUpGlobal();
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
        public void CloseSession()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        SchedulerServerSessionRemote.CloseSession();
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

        public void CloseAudit()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        SchedulerServerSessionRemote.CloseAudit();
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

        public DataTable GetSchedule()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return SchedulerServerSessionRemote.GetSchedule();
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

        public void ScheduleExistingJob(ScheduleProfile aSchedProf, int aJobRID, int aUserRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        SchedulerServerSessionRemote.ScheduleExistingJob(aSchedProf, aJobRID, aUserRID);
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

        public void ScheduleNewJob(ScheduleProfile aSchedProf, JobProfile aJobProf, int aTaskListRID, int aUserRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        SchedulerServerSessionRemote.ScheduleNewJob(aSchedProf, aJobProf, aTaskListRID, aUserRID);
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

        public void UpdateSchedule(ScheduleProfile aSchedProf, int aJobRID, int aUserRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        SchedulerServerSessionRemote.UpdateSchedule(aSchedProf, aJobRID, aUserRID);
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

        public DateTime GetNextRunDate(ScheduleProfile aSchedProf, int aJobRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return SchedulerServerSessionRemote.GetNextRunDate(aSchedProf, aJobRID);
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

        public eProcessExecutionStatus GetJobStatus(int aSchedRID, int aJobRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return SchedulerServerSessionRemote.GetJobStatus(aSchedRID, aJobRID);
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

		// Begin TT#1386-MD - stodd - Scheduler Job Manager
        public DataTable GetScheduledJobsForJobManager(string scheduleName, string jobName, int userRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return SchedulerServerSessionRemote.GetScheduledJobsForJobManager(scheduleName, jobName, userRID);
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
		// End TT#1386-MD - stodd - Scheduler Job Manager

        public void HoldJob(int aSchedRID, int aJobRID, int aUserRID)	// TT#1386-MD - stodd - Scheduler Job Manager
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        SchedulerServerSessionRemote.HoldJob(aSchedRID, aJobRID, aUserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
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

        public bool isJobInCycle(ScheduleProfile aSchedProf, int aJobRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return SchedulerServerSessionRemote.isJobInCycle(aSchedProf, aJobRID);
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

        public ArrayList HoldAllJobs(TaskListProfile aTaskListProf)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return SchedulerServerSessionRemote.HoldAllJobs(aTaskListProf);
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

        public ArrayList HoldAllJobs(JobProfile aJobProf)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return SchedulerServerSessionRemote.HoldAllJobs(aJobProf);
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

        public ArrayList HoldAllJobs(SpecialRequestProfile aSpecialRequestProf)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return SchedulerServerSessionRemote.HoldAllJobs(aSpecialRequestProf);
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

        public void RunJobNow(int aSchedRID, int aJobRID)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        SchedulerServerSessionRemote.RunJobNow(aSchedRID, aJobRID);
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

        public void CancelJob(int aSchedRID, int aJobRID, eProcessExecutionStatus aStatusAtCancel)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        SchedulerServerSessionRemote.CancelJob(aSchedRID, aJobRID, aStatusAtCancel);
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

        public void ResumeJob(int aSchedRID, int aJobRID, int aUserRID)	// TT#1386-MD - stodd - Scheduler Job Manager
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        SchedulerServerSessionRemote.ResumeJob(aSchedRID, aJobRID, aUserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
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

        public void ResumeAllJobs(ArrayList aHeldSchedules, int aUserRID)	// TT#1386-MD - stodd - Scheduler Job Manager
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        SchedulerServerSessionRemote.ResumeAllJobs(aHeldSchedules, aUserRID);	// TT#1386-MD - stodd - Scheduler Job Manager
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

        public bool DeleteSchedulesFromList(DataTable aTable)
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return SchedulerServerSessionRemote.DeleteSchedulesFromList(aTable);
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

        public void Refresh()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        SchedulerServerSessionRemote.Refresh();
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

        // Begin TT#195 MD - JSmith - Add environment authentication
        public ServiceProfile GetServiceProfile()
        {
            try
            {
                for (int i = 0; i < ServiceRetryCount; i++)
                {
                    try
                    {
                        return SchedulerServerSessionRemote.GetServiceProfile();
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
                        SchedulerServerSessionRemote.VerifyEnvironment(aClientProfile);
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
    }
    //End TT#708 - JScott - Services need a Retry availalbe.
}
