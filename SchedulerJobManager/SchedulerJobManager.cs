using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MIDRetail.Business;
using MIDRetail.Common;
using MIDRetail.DataCommon;
using MIDRetail.Data;



namespace SchedulerJobManager
{
    internal enum functionEnum
    {
        Release,
        Hold,
        CommandList
    }

    class SchedulerJobManagerAPI
    {
        static string eventLogID = "SchedulerJobManagerAPI";
        private static SessionAddressBlock _SAB;

        private static ManualResetEvent clientListDone = new ManualResetEvent(false);

        static int Main(string[] args)
        {

            if (!EventLog.SourceExists(eventLogID))
            {
                EventLog.CreateEventSource(eventLogID, null);
            }

            functionEnum function = functionEnum.CommandList;
            string scheduleName = string.Empty;
            string jobName = string.Empty;
            string userName = string.Empty;
            int userRid = -1;

            if (args.Length == 0)
            {
                Console.WriteLine("No arguments specified, use /? to see list of commands.");
                return 1;
            }

            string argLine = string.Empty;
            foreach (string arg in args)
            {
                argLine += arg + " ";
            }
            Debug.WriteLine(argLine);

            if (IsCommandValid(args, ref function, ref scheduleName, ref jobName, ref userName, ref userRid) == false)
            {
                Console.WriteLine("Unknown or invalid argument(s) specified, use /? to see list of valid commands.");
                return 1;
            }
            Debug.WriteLine("Function: " + function + " JOB: " + jobName + "USER: " + userName + " " + userRid);

            if (function == functionEnum.CommandList)
            {
                WriteCommandListToConsole();
                return 0;
            }

            try
            {
                IMessageCallback _messageCallback = new BatchMessageCallback();
                SessionSponsor _sponsor = new SessionSponsor();
                //authentication = eSecurityAuthenticate.UnknownUser;
                _SAB = new SessionAddressBlock(_messageCallback, _sponsor);
                String errorMessage = string.Empty;
                System.Runtime.Remoting.Channels.IChannel channel;

                if (!EventLog.SourceExists(eventLogID))
                {
                    EventLog.CreateEventSource(eventLogID, null);
                }

                // Register callback channel
                try
                {
                    channel = _SAB.OpenCallbackChannel();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error opening port #0 " + ex.Message);
                }

                // Create Sessions: Scheduler
                try
                {
                    _SAB.CreateSessions((int)eServerType.Scheduler);
                }
                catch (Exception ex)
                {
                    Exception innerE = ex;

                    while (innerE.InnerException != null)
                    {
                        innerE = innerE.InnerException;
                    }

                    throw new Exception("Error creating sessions - " + innerE.Message);
                }

                SchedulerJobCommand jobCommand = new SchedulerJobCommand(_SAB, function, scheduleName, jobName, userRid, argLine);
                jobCommand.Process();
            }
            catch (Exception ex)
            {
                EventLog.WriteEntry(eventLogID, ex.ToString(), EventLogEntryType.Error);
                Console.WriteLine(ex.ToString());
                return 1;
            }

            return 0;
        }

        private static bool IsCommandValid(string[] args, ref functionEnum function, ref string scheduleName, ref string jobName, ref string userName, ref int userRid)
        {
            bool isValid = true;
            scheduleName = string.Empty;
            jobName = string.Empty;
            userName = string.Empty;
            userRid = -1;
            //======================
            // Argument 0: Function
            //======================
            string functionArg = args[0].ToString().ToLower();
            if (functionArg == "/?")
            {
                function = functionEnum.CommandList;
                return true;
            }
            if (functionArg.Contains("release") || functionArg.Contains("rlse"))
            {
                function = functionEnum.Release;
            }
            else if (functionArg.Contains("hold"))
            {
                function = functionEnum.Hold;
            }
            else
            {
                return false;
            }

            string argStart4;
            string argStart5;
            string argStart6;

            //==================================================
            // Aurgument 1: schedule name, Job name, User name, or "all jobs"
            //==================================================
            if (args.Length > 1)
            {
                string arg1 = (string)args[1].ToString().Trim();
                argStart4 = (string)args[1].ToString().Substring(0, 4).ToLower();
                argStart5 = (string)args[1].ToString().Substring(0, 5).ToLower();
                argStart6 = (string)args[1].ToString().Substring(0, 6).ToLower();
                if (argStart4 == "job=")
                {
                    arg1 = arg1.Replace(args[1].ToString().Substring(0, 4), "");
                    jobName = arg1.Replace('*', '%');
                }
                else if (argStart5 == "user=")
                {
                    arg1 = arg1.Replace(args[1].ToString().Substring(0, 5), "");
                    userName = arg1;
                    userRid = GetUserRid(userName);
                    if (userRid == -1)
                    {
                        string msg = MIDText.GetText(eMIDTextCode.msg_InvalidUserName);
                        msg = msg.Replace("{0}", userName);
                        Console.WriteLine(msg);
                        EventLog.WriteEntry(eventLogID, msg, EventLogEntryType.Error);
                        return false;
                    }
                }
                else if (argStart6 == "sched=")
                {
                    arg1 = arg1.Replace(args[1].ToString().Substring(0, 6), "");
                    scheduleName = arg1.Replace('*', '%');
                }
                else if (arg1 == "all jobs")
                {
                    jobName = arg1;
                }
                else
                {
                    return false;
                }

                //==================================================
                // Aurgument 2: Job name
                //==================================================
                if (args.Length > 2)
                {
                    if (userName != string.Empty)
                    {
                        string arg2 = (string)args[2].ToString().Trim();
                        argStart4 = (string)args[2].ToString().Substring(0, 4).ToLower();

                        if (argStart4 == "job=")
                        {
                            arg2 = arg2.Replace(args[2].ToString().Substring(0, 4), "");
                            jobName = arg2.Replace('*', '%'); ;
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
            }
            else
            {
                return false;
            }

            return isValid;
        }

        static private void WriteCommandListToConsole()
        {
            Console.WriteLine("Valid command parameters for SchedulerJobManager:");
            Console.WriteLine("/?  (displays this list.)");
            Console.WriteLine("func schedule");
            Console.WriteLine("func job");
            Console.WriteLine("func user");
            Console.WriteLine("func schedule job");
            Console.WriteLine("func \"job=all jobs\"");
            Console.WriteLine("func \"sched=all jobs\"");
            Console.WriteLine("");
            Console.WriteLine("Examples");
            Console.WriteLine("func=hold \"job=all jobs\"  (This places all jobs on hold)");
            Console.WriteLine("func=hold \"sched=all jobs\"  (This also places all jobs on hold)");
            Console.WriteLine("func=rlse \"sched=Header Load\"  (This releases a schedule named \"Header Load\")");
            Console.WriteLine("func=rlse \"job=Daily History Load\"  (This releases a job named \"Daily History Load\")");
            Console.WriteLine("func=hold user=administrator  (This places on hold all jobs owned by the user \"administrator\")");
            Console.WriteLine("func=rlse \"sched=AM History Load\" \"job=Daily History Load\" (This releases a schedule named \"AM History Load\" that includes the job name \"Daily History Load\")");
            Console.WriteLine("");
            Console.WriteLine("Note: Wild card character for job names and schedule names is the asterisk (*).");
            Console.WriteLine("Note: Be sure to surround the job/schedule name parameter with quotes (\") if the job/schedule name includes spaces.  EX: \"job=Daily History Load\"");
            Console.WriteLine("Note: A job can exist in multiple schedules. Keep this in mind when releasing by job name alone. It will release that job for all schedules.");
        }

        static private int GetUserRid(string userName)
        {
            int userRid = -1;
            try
            {
                MIDConnectionString.ConnectionString = MIDConfigurationManager.AppSettings["ConnectionString"];
                DatabaseAccess dba = new DatabaseAccess(MIDConnectionString.ConnectionString);
                SecurityAdmin sa = new SecurityAdmin(dba);
                DataTable dtUsers = sa.GetActiveUsers();

                DataRow[] userRows = dtUsers.Select("USER_NAME = '" + userName + "'");
                if (userRows.Length == 1)
                {
                    userRid = int.Parse(userRows[0]["USER_RID"].ToString());
                }
                else
                {
                    return -1;
                }

                return userRid;
            }
            catch
            {
                throw;
            }
        }
    }

    class SchedulerJobCommand
    {
        static string eventLogID = "SchedulerJobManagerAPI";
        private functionEnum _function;
        private string _scheduleName;
        private string _jobName;
        private int _userRid;
        private DataTable _dtJobs;
        private SessionAddressBlock _SAB;
        private string _argLine;

        public SchedulerJobCommand(SessionAddressBlock sab, functionEnum function, string scheduleName, string jobName, int userRid, string argLine)
        {
            _SAB = sab;
            _function = function;
            _scheduleName = scheduleName;
            _jobName = jobName;
            _userRid = userRid;
            _argLine = argLine;
            _dtJobs = null;
        }

        public void Process()
        {
            try
            {
                FindMatchingJobs();

                UpdateExecutionStatus();

                string msg = MIDText.GetText(eMIDTextCode.msg_SchedulerJobManagerAPISuccessful);
                msg = msg.Replace("{0}", _argLine);
                if (_SAB.SchedulerServerSession.Audit != null)
                {
                    _SAB.SchedulerServerSession.Audit.Add_Msg(eMIDMessageLevel.Information, msg, eventLogID, true);
                }
                else
                {
                    EventLog.WriteEntry(eventLogID, msg, EventLogEntryType.Information);
                }
                Console.WriteLine(msg);
            }
            catch
            {
                throw;
            }
        }


        /// <summary>
        /// Resolves the matching jobs from the jobname and the user Rid.
        /// </summary>
        /// <param name="jobName"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        private void FindMatchingJobs()
        {
            try
            {
                _dtJobs = _SAB.SchedulerServerSession.GetScheduledJobsForJobManager(_scheduleName, _jobName, _userRid);
                if (_dtJobs == null)
                {
                    string msg = MIDText.GetText(eMIDTextCode.msg_MatchingJobsTableNull);
                    msg = msg.Replace("{0}", _scheduleName);
                    msg = msg.Replace("{1}", _jobName);
                    msg = msg.Replace("{2}", _userRid.ToString());
                    EventLog.WriteEntry(eventLogID, msg, EventLogEntryType.Error);
                    Console.WriteLine(msg);
                }
                else if (_dtJobs.Rows.Count == 0)
                {
                    string msg = MIDText.GetText(eMIDTextCode.msg_NoMatchingJobsFound);
                    msg = msg.Replace("{0}", _scheduleName);
                    msg = msg.Replace("{1}", _jobName);
                    msg = msg.Replace("{2}", _userRid.ToString());
                    EventLog.WriteEntry(eventLogID, msg, EventLogEntryType.Warning);
                    Console.WriteLine(msg);
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// Updates the execution status on selected jobs.
        /// </summary>
        private void UpdateExecutionStatus()
        {
            try
            {
                foreach (DataRow aRow in _dtJobs.Rows)
                {
                    int schedRid = Include.NoRID;
                    int jobRid = Include.NoRID;
                    eProcessExecutionStatus execStatus = eProcessExecutionStatus.None;

                    if (aRow["SCHED_RID"] != DBNull.Value)
                    {
                        schedRid = int.Parse(aRow["SCHED_RID"].ToString());
                    }
                    if (aRow["JOB_RID"] != DBNull.Value)
                    {
                        jobRid = int.Parse(aRow["JOB_RID"].ToString());
                    }

                    if (aRow["EXECUTION_STATUS"] != DBNull.Value)
                    {
                        execStatus = (eProcessExecutionStatus)int.Parse(aRow["EXECUTION_STATUS"].ToString());
                    }

                    if (_function == functionEnum.Hold)
                    {
                        if (execStatus == eProcessExecutionStatus.Waiting)
                        {
                            _SAB.SchedulerServerSession.HoldJob(schedRid, jobRid, Include.AdministratorUserRID);
                        }
                    }
                    else if (_function == functionEnum.Release)
                    {
                        if (execStatus == eProcessExecutionStatus.OnHold)
                        {
                            _SAB.SchedulerServerSession.ResumeJob(schedRid, jobRid, Include.AdministratorUserRID);
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
        }


    }
}
