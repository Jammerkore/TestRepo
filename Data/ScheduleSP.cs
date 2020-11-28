using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace MIDRetail.Data
{
    public partial class ScheduleData : DataLayer
    {
        protected static class StoredProcedures
        {

            public static MID_SCHEDULE_READ_ALL_JOBS_def MID_SCHEDULE_READ_ALL_JOBS = new MID_SCHEDULE_READ_ALL_JOBS_def();
			public class MID_SCHEDULE_READ_ALL_JOBS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_READ_ALL_JOBS.SQL"

			
			    public MID_SCHEDULE_READ_ALL_JOBS_def()
			    {
			        base.procedureName = "MID_SCHEDULE_READ_ALL_JOBS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SCHEDULE");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SCHEDULE_READ_ALL_JOBS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static MID_SCHEDULE_READ_JOBS_FOR_JOB_MANAGER_def MID_SCHEDULE_READ_JOBS_FOR_JOB_MANAGER = new MID_SCHEDULE_READ_JOBS_FOR_JOB_MANAGER_def();
            public class MID_SCHEDULE_READ_JOBS_FOR_JOB_MANAGER_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_READ_JOBS_FOR_JOB_MANAGER.SQL"

                private stringParameter SCHED_NAME;
                private stringParameter JOB_NAME;
                private intParameter USER_RID;

                public MID_SCHEDULE_READ_JOBS_FOR_JOB_MANAGER_def()
                {
                    base.procedureName = "MID_SCHEDULE_READ_JOBS_FOR_JOB_MANAGER";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("SCHEDULE_JOB_JOIN");
                    SCHED_NAME = new stringParameter("@SCHED_NAME", base.inputParameterList);
                    JOB_NAME = new stringParameter("@JOB_NAME", base.inputParameterList);
                    USER_RID = new intParameter("@USER_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      string SCHED_NAME,
                    			      string JOB_NAME,
			                          int? USER_RID
			                          )
                {
                    lock (typeof(MID_SCHEDULE_READ_JOBS_FOR_JOB_MANAGER_def))
                    {
                        this.SCHED_NAME.SetValue(SCHED_NAME);
                        this.JOB_NAME.SetValue(JOB_NAME);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

			public static MID_SCHEDULE_READ_JOB_def MID_SCHEDULE_READ_JOB = new MID_SCHEDULE_READ_JOB_def();
			public class MID_SCHEDULE_READ_JOB_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_READ_JOB.SQL"

			    private intParameter SCHED_RID;
                private intParameter JOB_RID;
			
			    public MID_SCHEDULE_READ_JOB_def()
			    {
			        base.procedureName = "MID_SCHEDULE_READ_JOB";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SCHEDULE");
			        SCHED_RID = new intParameter("@SCHED_RID", base.inputParameterList);
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? SCHED_RID,
			                          int? JOB_RID
			                          )
			    {
                    lock (typeof(MID_SCHEDULE_READ_JOB_def))
                    {
                        this.SCHED_RID.SetValue(SCHED_RID);
                        this.JOB_RID.SetValue(JOB_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_READ_JOB_FROM_JOB_RID_def MID_SCHEDULE_READ_JOB_FROM_JOB_RID = new MID_SCHEDULE_READ_JOB_FROM_JOB_RID_def();
			public class MID_SCHEDULE_READ_JOB_FROM_JOB_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_READ_JOB_FROM_JOB_RID.SQL"

                private intParameter JOB_RID;
			
			    public MID_SCHEDULE_READ_JOB_FROM_JOB_RID_def()
			    {
			        base.procedureName = "MID_SCHEDULE_READ_JOB_FROM_JOB_RID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SCHEDULE");
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? JOB_RID)
			    {
                    lock (typeof(MID_SCHEDULE_READ_JOB_FROM_JOB_RID_def))
                    {
                        this.JOB_RID.SetValue(JOB_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_READ_ACTIVE_JOBS_def MID_SCHEDULE_READ_ACTIVE_JOBS = new MID_SCHEDULE_READ_ACTIVE_JOBS_def();
			public class MID_SCHEDULE_READ_ACTIVE_JOBS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_READ_ACTIVE_JOBS.SQL"

                private intParameter JOB_RID;
			
			    public MID_SCHEDULE_READ_ACTIVE_JOBS_def()
			    {
			        base.procedureName = "MID_SCHEDULE_READ_ACTIVE_JOBS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SCHEDULE");
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? JOB_RID)
			    {
                    lock (typeof(MID_SCHEDULE_READ_ACTIVE_JOBS_def))
                    {
                        this.JOB_RID.SetValue(JOB_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_READ_JOBS_OLDER_THAN_DATE_def MID_SCHEDULE_READ_JOBS_OLDER_THAN_DATE = new MID_SCHEDULE_READ_JOBS_OLDER_THAN_DATE_def();
			public class MID_SCHEDULE_READ_JOBS_OLDER_THAN_DATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_READ_JOBS_OLDER_THAN_DATE.SQL"

                private datetimeParameter LAST_COMPLETION_DATETIME;
			
			    public MID_SCHEDULE_READ_JOBS_OLDER_THAN_DATE_def()
			    {
			        base.procedureName = "MID_SCHEDULE_READ_JOBS_OLDER_THAN_DATE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SCHEDULE");
			        LAST_COMPLETION_DATETIME = new datetimeParameter("@LAST_COMPLETION_DATETIME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, DateTime? LAST_COMPLETION_DATETIME)
			    {
                    lock (typeof(MID_SCHEDULE_READ_JOBS_OLDER_THAN_DATE_def))
                    {
                        this.LAST_COMPLETION_DATETIME.SetValue(LAST_COMPLETION_DATETIME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_JOB_READ_ALL_ORPHANED_SYSTEM_JOBS_def MID_JOB_READ_ALL_ORPHANED_SYSTEM_JOBS = new MID_JOB_READ_ALL_ORPHANED_SYSTEM_JOBS_def();
			public class MID_JOB_READ_ALL_ORPHANED_SYSTEM_JOBS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_READ_ALL_ORPHANED_SYSTEM_JOBS.SQL"

			
			    public MID_JOB_READ_ALL_ORPHANED_SYSTEM_JOBS_def()
			    {
			        base.procedureName = "MID_JOB_READ_ALL_ORPHANED_SYSTEM_JOBS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("JOB");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_JOB_READ_ALL_ORPHANED_SYSTEM_JOBS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_JOB_READ_ALL_ORPHANED_SYSTEM_TASKLISTS_def MID_JOB_READ_ALL_ORPHANED_SYSTEM_TASKLISTS = new MID_JOB_READ_ALL_ORPHANED_SYSTEM_TASKLISTS_def();
			public class MID_JOB_READ_ALL_ORPHANED_SYSTEM_TASKLISTS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_READ_ALL_ORPHANED_SYSTEM_TASKLISTS.SQL"

			
			    public MID_JOB_READ_ALL_ORPHANED_SYSTEM_TASKLISTS_def()
			    {
			        base.procedureName = "MID_JOB_READ_ALL_ORPHANED_SYSTEM_TASKLISTS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("JOB");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_JOB_READ_ALL_ORPHANED_SYSTEM_TASKLISTS_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_READ_ACTIVE_JOBS_BY_TASKLIST_def MID_SCHEDULE_READ_ACTIVE_JOBS_BY_TASKLIST = new MID_SCHEDULE_READ_ACTIVE_JOBS_BY_TASKLIST_def();
			public class MID_SCHEDULE_READ_ACTIVE_JOBS_BY_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_READ_ACTIVE_JOBS_BY_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_SCHEDULE_READ_ACTIVE_JOBS_BY_TASKLIST_def()
			    {
			        base.procedureName = "MID_SCHEDULE_READ_ACTIVE_JOBS_BY_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SCHEDULE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_SCHEDULE_READ_ACTIVE_JOBS_BY_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_JOB_READ_SYSTEM_JOBS_BY_TASKLIST_def MID_JOB_READ_SYSTEM_JOBS_BY_TASKLIST = new MID_JOB_READ_SYSTEM_JOBS_BY_TASKLIST_def();
			public class MID_JOB_READ_SYSTEM_JOBS_BY_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_READ_SYSTEM_JOBS_BY_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_JOB_READ_SYSTEM_JOBS_BY_TASKLIST_def()
			    {
			        base.procedureName = "MID_JOB_READ_SYSTEM_JOBS_BY_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("JOB");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_JOB_READ_SYSTEM_JOBS_BY_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_READ_NON_SYSTEM_JOBS_BY_TASKLIST_def MID_SCHEDULE_READ_NON_SYSTEM_JOBS_BY_TASKLIST = new MID_SCHEDULE_READ_NON_SYSTEM_JOBS_BY_TASKLIST_def();
			public class MID_SCHEDULE_READ_NON_SYSTEM_JOBS_BY_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_READ_NON_SYSTEM_JOBS_BY_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_SCHEDULE_READ_NON_SYSTEM_JOBS_BY_TASKLIST_def()
			    {
			        base.procedureName = "MID_SCHEDULE_READ_NON_SYSTEM_JOBS_BY_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SCHEDULE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_SCHEDULE_READ_NON_SYSTEM_JOBS_BY_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_READ_SYSTEM_JOBS_BY_TASKLIST_def MID_SCHEDULE_READ_SYSTEM_JOBS_BY_TASKLIST = new MID_SCHEDULE_READ_SYSTEM_JOBS_BY_TASKLIST_def();
			public class MID_SCHEDULE_READ_SYSTEM_JOBS_BY_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_READ_SYSTEM_JOBS_BY_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_SCHEDULE_READ_SYSTEM_JOBS_BY_TASKLIST_def()
			    {
			        base.procedureName = "MID_SCHEDULE_READ_SYSTEM_JOBS_BY_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SCHEDULE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_SCHEDULE_READ_SYSTEM_JOBS_BY_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SPECIAL_REQUEST_JOB_READ_def MID_SPECIAL_REQUEST_JOB_READ = new MID_SPECIAL_REQUEST_JOB_READ_def();
			public class MID_SPECIAL_REQUEST_JOB_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_READ.SQL"

                private intParameter JOB_RID;
			
			    public MID_SPECIAL_REQUEST_JOB_READ_def()
			    {
			        base.procedureName = "MID_SPECIAL_REQUEST_JOB_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB");
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? JOB_RID)
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_READ_def))
                    {
                        this.JOB_RID.SetValue(JOB_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_READ_def MID_SCHEDULE_READ = new MID_SCHEDULE_READ_def();
			public class MID_SCHEDULE_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_READ.SQL"

                private intParameter SCHED_RID;
			
			    public MID_SCHEDULE_READ_def()
			    {
			        base.procedureName = "MID_SCHEDULE_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SCHEDULE");
			        SCHED_RID = new intParameter("@SCHED_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SCHED_RID)
			    {
                    lock (typeof(MID_SCHEDULE_READ_def))
                    {
                        this.SCHED_RID.SetValue(SCHED_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_READ_KEY_def MID_SCHEDULE_READ_KEY = new MID_SCHEDULE_READ_KEY_def();
			public class MID_SCHEDULE_READ_KEY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_READ_KEY.SQL"

                private stringParameter SCHED_NAME;
			
			    public MID_SCHEDULE_READ_KEY_def()
			    {
			        base.procedureName = "MID_SCHEDULE_READ_KEY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SCHEDULE");
			        SCHED_NAME = new stringParameter("@SCHED_NAME", base.inputParameterList);
			    }

                public DataTable Read(DatabaseAccess _dba, string SCHED_NAME)
			    {
                    lock (typeof(MID_SCHEDULE_READ_KEY_def))
                    {
                        this.SCHED_NAME.SetValue(SCHED_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_UPDATE_def MID_SCHEDULE_UPDATE = new MID_SCHEDULE_UPDATE_def();
			public class MID_SCHEDULE_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_UPDATE.SQL"

                private stringParameter SCHED_NAME;
                private datetimeParameter START_TIME;
                private intParameter SCHEDULE_BY_TYPE;
                private intParameter SCHEDULE_BY_INTERVAL;
                private charParameter SCHEDULE_ON_MONDAY_IND;
                private charParameter SCHEDULE_ON_TUESDAY_IND;
                private charParameter SCHEDULE_ON_WEDNESDAY_IND;
                private charParameter SCHEDULE_ON_THURSDAY_IND;
                private charParameter SCHEDULE_ON_FRIDAY_IND;
                private charParameter SCHEDULE_ON_SATURDAY_IND;
                private charParameter SCHEDULE_ON_SUNDAY_IND;
                private intParameter SCHEDULE_BY_MONTH_WEEK_TYPE;
                private datetimeParameter START_DATE_RANGE;
                private charParameter END_DATE_IND;
                private datetimeParameter END_DATE_RANGE;
                private intParameter REPEAT_INTERVAL;
                private intParameter REPEAT_INTERVAL_TYPE;
                private charParameter REPEAT_UNTIL_IND;
                private datetimeParameter REPEAT_UNTIL_TIME;
                private charParameter REPEAT_DURATION_IND;
                private intParameter REPEAT_DURATION_HOURS;
                private intParameter REPEAT_DURATION_MINUTES;
                private intParameter CONDITION_TYPE;
                private stringParameter CONDITION_TRIGGER_DIRECTORY;
                private stringParameter CONDITION_TRIGGER_SUFFIX;
                private charParameter TERMINATE_AFTER_COND_MET_IND;
                private charParameter REPEAT_UNTIL_SUCCESSFUL_IND;
                private intParameter LAST_MODIFIED_BY_USER_RID;
                private datetimeParameter LAST_MODIFIED_DATETIME;
                private intParameter SCHED_RID;
			
			    public MID_SCHEDULE_UPDATE_def()
			    {
			        base.procedureName = "MID_SCHEDULE_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("SCHEDULE");
			        SCHED_NAME = new stringParameter("@SCHED_NAME", base.inputParameterList);
			        START_TIME = new datetimeParameter("@START_TIME", base.inputParameterList);
			        SCHEDULE_BY_TYPE = new intParameter("@SCHEDULE_BY_TYPE", base.inputParameterList);
			        SCHEDULE_BY_INTERVAL = new intParameter("@SCHEDULE_BY_INTERVAL", base.inputParameterList);
			        SCHEDULE_ON_MONDAY_IND = new charParameter("@SCHEDULE_ON_MONDAY_IND", base.inputParameterList);
			        SCHEDULE_ON_TUESDAY_IND = new charParameter("@SCHEDULE_ON_TUESDAY_IND", base.inputParameterList);
			        SCHEDULE_ON_WEDNESDAY_IND = new charParameter("@SCHEDULE_ON_WEDNESDAY_IND", base.inputParameterList);
			        SCHEDULE_ON_THURSDAY_IND = new charParameter("@SCHEDULE_ON_THURSDAY_IND", base.inputParameterList);
			        SCHEDULE_ON_FRIDAY_IND = new charParameter("@SCHEDULE_ON_FRIDAY_IND", base.inputParameterList);
			        SCHEDULE_ON_SATURDAY_IND = new charParameter("@SCHEDULE_ON_SATURDAY_IND", base.inputParameterList);
			        SCHEDULE_ON_SUNDAY_IND = new charParameter("@SCHEDULE_ON_SUNDAY_IND", base.inputParameterList);
			        SCHEDULE_BY_MONTH_WEEK_TYPE = new intParameter("@SCHEDULE_BY_MONTH_WEEK_TYPE", base.inputParameterList);
			        START_DATE_RANGE = new datetimeParameter("@START_DATE_RANGE", base.inputParameterList);
			        END_DATE_IND = new charParameter("@END_DATE_IND", base.inputParameterList);
			        END_DATE_RANGE = new datetimeParameter("@END_DATE_RANGE", base.inputParameterList);
			        REPEAT_INTERVAL = new intParameter("@REPEAT_INTERVAL", base.inputParameterList);
			        REPEAT_INTERVAL_TYPE = new intParameter("@REPEAT_INTERVAL_TYPE", base.inputParameterList);
			        REPEAT_UNTIL_IND = new charParameter("@REPEAT_UNTIL_IND", base.inputParameterList);
			        REPEAT_UNTIL_TIME = new datetimeParameter("@REPEAT_UNTIL_TIME", base.inputParameterList);
			        REPEAT_DURATION_IND = new charParameter("@REPEAT_DURATION_IND", base.inputParameterList);
			        REPEAT_DURATION_HOURS = new intParameter("@REPEAT_DURATION_HOURS", base.inputParameterList);
			        REPEAT_DURATION_MINUTES = new intParameter("@REPEAT_DURATION_MINUTES", base.inputParameterList);
			        CONDITION_TYPE = new intParameter("@CONDITION_TYPE", base.inputParameterList);
			        CONDITION_TRIGGER_DIRECTORY = new stringParameter("@CONDITION_TRIGGER_DIRECTORY", base.inputParameterList);
			        CONDITION_TRIGGER_SUFFIX = new stringParameter("@CONDITION_TRIGGER_SUFFIX", base.inputParameterList);
			        TERMINATE_AFTER_COND_MET_IND = new charParameter("@TERMINATE_AFTER_COND_MET_IND", base.inputParameterList);
			        REPEAT_UNTIL_SUCCESSFUL_IND = new charParameter("@REPEAT_UNTIL_SUCCESSFUL_IND", base.inputParameterList);
			        LAST_MODIFIED_BY_USER_RID = new intParameter("@LAST_MODIFIED_BY_USER_RID", base.inputParameterList);
			        LAST_MODIFIED_DATETIME = new datetimeParameter("@LAST_MODIFIED_DATETIME", base.inputParameterList);
			        SCHED_RID = new intParameter("@SCHED_RID", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      string SCHED_NAME,
			                      DateTime? START_TIME,
			                      int? SCHEDULE_BY_TYPE,
			                      int? SCHEDULE_BY_INTERVAL,
			                      char? SCHEDULE_ON_MONDAY_IND,
			                      char? SCHEDULE_ON_TUESDAY_IND,
			                      char? SCHEDULE_ON_WEDNESDAY_IND,
			                      char? SCHEDULE_ON_THURSDAY_IND,
			                      char? SCHEDULE_ON_FRIDAY_IND,
			                      char? SCHEDULE_ON_SATURDAY_IND,
			                      char? SCHEDULE_ON_SUNDAY_IND,
			                      int? SCHEDULE_BY_MONTH_WEEK_TYPE,
			                      DateTime? START_DATE_RANGE,
			                      char? END_DATE_IND,
			                      DateTime? END_DATE_RANGE,
			                      int? REPEAT_INTERVAL,
			                      int? REPEAT_INTERVAL_TYPE,
			                      char? REPEAT_UNTIL_IND,
			                      DateTime? REPEAT_UNTIL_TIME,
			                      char? REPEAT_DURATION_IND,
			                      int? REPEAT_DURATION_HOURS,
			                      int? REPEAT_DURATION_MINUTES,
			                      int? CONDITION_TYPE,
			                      string CONDITION_TRIGGER_DIRECTORY,
			                      string CONDITION_TRIGGER_SUFFIX,
			                      char? TERMINATE_AFTER_COND_MET_IND,
			                      char? REPEAT_UNTIL_SUCCESSFUL_IND,
			                      int? LAST_MODIFIED_BY_USER_RID,
			                      DateTime? LAST_MODIFIED_DATETIME,
			                      int? SCHED_RID
			                      )
			    {
                    lock (typeof(MID_SCHEDULE_UPDATE_def))
                    {
                        this.SCHED_NAME.SetValue(SCHED_NAME);
                        this.START_TIME.SetValue(START_TIME);
                        this.SCHEDULE_BY_TYPE.SetValue(SCHEDULE_BY_TYPE);
                        this.SCHEDULE_BY_INTERVAL.SetValue(SCHEDULE_BY_INTERVAL);
                        this.SCHEDULE_ON_MONDAY_IND.SetValue(SCHEDULE_ON_MONDAY_IND);
                        this.SCHEDULE_ON_TUESDAY_IND.SetValue(SCHEDULE_ON_TUESDAY_IND);
                        this.SCHEDULE_ON_WEDNESDAY_IND.SetValue(SCHEDULE_ON_WEDNESDAY_IND);
                        this.SCHEDULE_ON_THURSDAY_IND.SetValue(SCHEDULE_ON_THURSDAY_IND);
                        this.SCHEDULE_ON_FRIDAY_IND.SetValue(SCHEDULE_ON_FRIDAY_IND);
                        this.SCHEDULE_ON_SATURDAY_IND.SetValue(SCHEDULE_ON_SATURDAY_IND);
                        this.SCHEDULE_ON_SUNDAY_IND.SetValue(SCHEDULE_ON_SUNDAY_IND);
                        this.SCHEDULE_BY_MONTH_WEEK_TYPE.SetValue(SCHEDULE_BY_MONTH_WEEK_TYPE);
                        this.START_DATE_RANGE.SetValue(START_DATE_RANGE);
                        this.END_DATE_IND.SetValue(END_DATE_IND);
                        this.END_DATE_RANGE.SetValue(END_DATE_RANGE);
                        this.REPEAT_INTERVAL.SetValue(REPEAT_INTERVAL);
                        this.REPEAT_INTERVAL_TYPE.SetValue(REPEAT_INTERVAL_TYPE);
                        this.REPEAT_UNTIL_IND.SetValue(REPEAT_UNTIL_IND);
                        this.REPEAT_UNTIL_TIME.SetValue(REPEAT_UNTIL_TIME);
                        this.REPEAT_DURATION_IND.SetValue(REPEAT_DURATION_IND);
                        this.REPEAT_DURATION_HOURS.SetValue(REPEAT_DURATION_HOURS);
                        this.REPEAT_DURATION_MINUTES.SetValue(REPEAT_DURATION_MINUTES);
                        this.CONDITION_TYPE.SetValue(CONDITION_TYPE);
                        this.CONDITION_TRIGGER_DIRECTORY.SetValue(CONDITION_TRIGGER_DIRECTORY);
                        this.CONDITION_TRIGGER_SUFFIX.SetValue(CONDITION_TRIGGER_SUFFIX);
                        this.TERMINATE_AFTER_COND_MET_IND.SetValue(TERMINATE_AFTER_COND_MET_IND);
                        this.REPEAT_UNTIL_SUCCESSFUL_IND.SetValue(REPEAT_UNTIL_SUCCESSFUL_IND);
                        this.LAST_MODIFIED_BY_USER_RID.SetValue(LAST_MODIFIED_BY_USER_RID);
                        this.LAST_MODIFIED_DATETIME.SetValue(LAST_MODIFIED_DATETIME);
                        this.SCHED_RID.SetValue(SCHED_RID);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_SCHEDULE_INSERT_def SP_MID_SCHEDULE_INSERT = new SP_MID_SCHEDULE_INSERT_def();
            public class SP_MID_SCHEDULE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SCHEDULE_INSERT.SQL"

                private stringParameter SCHED_NAME;
                private datetimeParameter START_TIME;
                private intParameter SCHEDULE_BY_TYPE;
                private intParameter SCHEDULE_BY_INTERVAL;
                private charParameter SCHEDULE_ON_MONDAY_IND;
                private charParameter SCHEDULE_ON_TUESDAY_IND;
                private charParameter SCHEDULE_ON_WEDNESDAY_IND;
                private charParameter SCHEDULE_ON_THURSDAY_IND;
                private charParameter SCHEDULE_ON_FRIDAY_IND;
                private charParameter SCHEDULE_ON_SATURDAY_IND;
                private charParameter SCHEDULE_ON_SUNDAY_IND;
                private intParameter SCHEDULE_BY_MONTH_WEEK_TYPE;
                private datetimeParameter START_DATE_RANGE;
                private charParameter END_DATE_IND;
                private datetimeParameter END_DATE_RANGE;
                private intParameter REPEAT_INTERVAL;
                private intParameter REPEAT_INTERVAL_TYPE;
                private charParameter REPEAT_UNTIL_IND;
                private datetimeParameter REPEAT_UNTIL_TIME;
                private charParameter REPEAT_DURATION_IND;
                private intParameter REPEAT_DURATION_HOURS;
                private intParameter REPEAT_DURATION_MINUTES;
                private intParameter CONDITION_TYPE;
                private stringParameter CONDITION_TRIGGER_DIRECTORY;
                private stringParameter CONDITION_TRIGGER_SUFFIX;
                private charParameter TERMINATE_AFTER_COND_MET_IND;
                private charParameter REPEAT_UNTIL_SUCCESSFUL_IND;
                private intParameter CREATED_BY_USER_RID;
                private datetimeParameter CREATED_DATETIME;
                private intParameter LAST_MODIFIED_BY_USER_RID;
                private datetimeParameter LAST_MODIFIED_DATETIME;
                private intParameter SCHED_RID; //Declare Output Parameter

                public SP_MID_SCHEDULE_INSERT_def()
                {
                    base.procedureName = "SP_MID_SCHEDULE_INSERT";
                    base.procedureType = storedProcedureTypes.InsertAndReturnRID;
                    base.tableNames.Add("SCHEDULE");
                    SCHED_NAME = new stringParameter("@SCHED_NAME", base.inputParameterList);
                    START_TIME = new datetimeParameter("@START_TIME", base.inputParameterList);
                    SCHEDULE_BY_TYPE = new intParameter("@SCHEDULE_BY_TYPE", base.inputParameterList);
                    SCHEDULE_BY_INTERVAL = new intParameter("@SCHEDULE_BY_INTERVAL", base.inputParameterList);
                    SCHEDULE_ON_MONDAY_IND = new charParameter("@SCHEDULE_ON_MONDAY_IND", base.inputParameterList);
                    SCHEDULE_ON_TUESDAY_IND = new charParameter("@SCHEDULE_ON_TUESDAY_IND", base.inputParameterList);
                    SCHEDULE_ON_WEDNESDAY_IND = new charParameter("@SCHEDULE_ON_WEDNESDAY_IND", base.inputParameterList);
                    SCHEDULE_ON_THURSDAY_IND = new charParameter("@SCHEDULE_ON_THURSDAY_IND", base.inputParameterList);
                    SCHEDULE_ON_FRIDAY_IND = new charParameter("@SCHEDULE_ON_FRIDAY_IND", base.inputParameterList);
                    SCHEDULE_ON_SATURDAY_IND = new charParameter("@SCHEDULE_ON_SATURDAY_IND", base.inputParameterList);
                    SCHEDULE_ON_SUNDAY_IND = new charParameter("@SCHEDULE_ON_SUNDAY_IND", base.inputParameterList);
                    SCHEDULE_BY_MONTH_WEEK_TYPE = new intParameter("@SCHEDULE_BY_MONTH_WEEK_TYPE", base.inputParameterList);
                    START_DATE_RANGE = new datetimeParameter("@START_DATE_RANGE", base.inputParameterList);
                    END_DATE_IND = new charParameter("@END_DATE_IND", base.inputParameterList);
                    END_DATE_RANGE = new datetimeParameter("@END_DATE_RANGE", base.inputParameterList);
                    REPEAT_INTERVAL = new intParameter("@REPEAT_INTERVAL", base.inputParameterList);
                    REPEAT_INTERVAL_TYPE = new intParameter("@REPEAT_INTERVAL_TYPE", base.inputParameterList);
                    REPEAT_UNTIL_IND = new charParameter("@REPEAT_UNTIL_IND", base.inputParameterList);
                    REPEAT_UNTIL_TIME = new datetimeParameter("@REPEAT_UNTIL_TIME", base.inputParameterList);
                    REPEAT_DURATION_IND = new charParameter("@REPEAT_DURATION_IND", base.inputParameterList);
                    REPEAT_DURATION_HOURS = new intParameter("@REPEAT_DURATION_HOURS", base.inputParameterList);
                    REPEAT_DURATION_MINUTES = new intParameter("@REPEAT_DURATION_MINUTES", base.inputParameterList);
                    CONDITION_TYPE = new intParameter("@CONDITION_TYPE", base.inputParameterList);
                    CONDITION_TRIGGER_DIRECTORY = new stringParameter("@CONDITION_TRIGGER_DIRECTORY", base.inputParameterList);
                    CONDITION_TRIGGER_SUFFIX = new stringParameter("@CONDITION_TRIGGER_SUFFIX", base.inputParameterList);
                    TERMINATE_AFTER_COND_MET_IND = new charParameter("@TERMINATE_AFTER_COND_MET_IND", base.inputParameterList);
                    REPEAT_UNTIL_SUCCESSFUL_IND = new charParameter("@REPEAT_UNTIL_SUCCESSFUL_IND", base.inputParameterList);
                    CREATED_BY_USER_RID = new intParameter("@CREATED_BY_USER_RID", base.inputParameterList);
                    CREATED_DATETIME = new datetimeParameter("@CREATED_DATETIME", base.inputParameterList);
                    LAST_MODIFIED_BY_USER_RID = new intParameter("@LAST_MODIFIED_BY_USER_RID", base.inputParameterList);
                    LAST_MODIFIED_DATETIME = new datetimeParameter("@LAST_MODIFIED_DATETIME", base.inputParameterList);
                    SCHED_RID = new intParameter("@SCHED_RID", base.outputParameterList); //Add Output Parameter
                }

                public int InsertAndReturnRID(DatabaseAccess _dba,
                                              string SCHED_NAME,
                                              DateTime? START_TIME,
                                              int? SCHEDULE_BY_TYPE,
                                              int? SCHEDULE_BY_INTERVAL,
                                              char? SCHEDULE_ON_MONDAY_IND,
                                              char? SCHEDULE_ON_TUESDAY_IND,
                                              char? SCHEDULE_ON_WEDNESDAY_IND,
                                              char? SCHEDULE_ON_THURSDAY_IND,
                                              char? SCHEDULE_ON_FRIDAY_IND,
                                              char? SCHEDULE_ON_SATURDAY_IND,
                                              char? SCHEDULE_ON_SUNDAY_IND,
                                              int? SCHEDULE_BY_MONTH_WEEK_TYPE,
                                              DateTime? START_DATE_RANGE,
                                              char? END_DATE_IND,
                                              DateTime? END_DATE_RANGE,
                                              int? REPEAT_INTERVAL,
                                              int? REPEAT_INTERVAL_TYPE,
                                              char? REPEAT_UNTIL_IND,
                                              DateTime? REPEAT_UNTIL_TIME,
                                              char? REPEAT_DURATION_IND,
                                              int? REPEAT_DURATION_HOURS,
                                              int? REPEAT_DURATION_MINUTES,
                                              int? CONDITION_TYPE,
                                              string CONDITION_TRIGGER_DIRECTORY,
                                              string CONDITION_TRIGGER_SUFFIX,
                                              char? TERMINATE_AFTER_COND_MET_IND,
                                              char? REPEAT_UNTIL_SUCCESSFUL_IND,
                                              int? CREATED_BY_USER_RID,
                                              DateTime? CREATED_DATETIME,
                                              int? LAST_MODIFIED_BY_USER_RID,
                                              DateTime? LAST_MODIFIED_DATETIME
                                              )
                {
                    lock (typeof(SP_MID_SCHEDULE_INSERT_def))
                    {
                        this.SCHED_NAME.SetValue(SCHED_NAME);
                        this.START_TIME.SetValue(START_TIME);
                        this.SCHEDULE_BY_TYPE.SetValue(SCHEDULE_BY_TYPE);
                        this.SCHEDULE_BY_INTERVAL.SetValue(SCHEDULE_BY_INTERVAL);
                        this.SCHEDULE_ON_MONDAY_IND.SetValue(SCHEDULE_ON_MONDAY_IND);
                        this.SCHEDULE_ON_TUESDAY_IND.SetValue(SCHEDULE_ON_TUESDAY_IND);
                        this.SCHEDULE_ON_WEDNESDAY_IND.SetValue(SCHEDULE_ON_WEDNESDAY_IND);
                        this.SCHEDULE_ON_THURSDAY_IND.SetValue(SCHEDULE_ON_THURSDAY_IND);
                        this.SCHEDULE_ON_FRIDAY_IND.SetValue(SCHEDULE_ON_FRIDAY_IND);
                        this.SCHEDULE_ON_SATURDAY_IND.SetValue(SCHEDULE_ON_SATURDAY_IND);
                        this.SCHEDULE_ON_SUNDAY_IND.SetValue(SCHEDULE_ON_SUNDAY_IND);
                        this.SCHEDULE_BY_MONTH_WEEK_TYPE.SetValue(SCHEDULE_BY_MONTH_WEEK_TYPE);
                        this.START_DATE_RANGE.SetValue(START_DATE_RANGE);
                        this.END_DATE_IND.SetValue(END_DATE_IND);
                        this.END_DATE_RANGE.SetValue(END_DATE_RANGE);
                        this.REPEAT_INTERVAL.SetValue(REPEAT_INTERVAL);
                        this.REPEAT_INTERVAL_TYPE.SetValue(REPEAT_INTERVAL_TYPE);
                        this.REPEAT_UNTIL_IND.SetValue(REPEAT_UNTIL_IND);
                        this.REPEAT_UNTIL_TIME.SetValue(REPEAT_UNTIL_TIME);
                        this.REPEAT_DURATION_IND.SetValue(REPEAT_DURATION_IND);
                        this.REPEAT_DURATION_HOURS.SetValue(REPEAT_DURATION_HOURS);
                        this.REPEAT_DURATION_MINUTES.SetValue(REPEAT_DURATION_MINUTES);
                        this.CONDITION_TYPE.SetValue(CONDITION_TYPE);
                        this.CONDITION_TRIGGER_DIRECTORY.SetValue(CONDITION_TRIGGER_DIRECTORY);
                        this.CONDITION_TRIGGER_SUFFIX.SetValue(CONDITION_TRIGGER_SUFFIX);
                        this.TERMINATE_AFTER_COND_MET_IND.SetValue(TERMINATE_AFTER_COND_MET_IND);
                        this.REPEAT_UNTIL_SUCCESSFUL_IND.SetValue(REPEAT_UNTIL_SUCCESSFUL_IND);
                        this.CREATED_BY_USER_RID.SetValue(CREATED_BY_USER_RID);
                        this.CREATED_DATETIME.SetValue(CREATED_DATETIME);
                        this.LAST_MODIFIED_BY_USER_RID.SetValue(LAST_MODIFIED_BY_USER_RID);
                        this.LAST_MODIFIED_DATETIME.SetValue(LAST_MODIFIED_DATETIME);
                        this.SCHED_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
                }
            }


            public static SP_MID_TASKLIST_INSERT_def SP_MID_TASKLIST_INSERT = new SP_MID_TASKLIST_INSERT_def();
            public class SP_MID_TASKLIST_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_TASKLIST_INSERT.SQL"

                private stringParameter TASKLIST_NAME;
                private intParameter USER_RID;
                private charParameter SYSTEM_GENERATED_IND;
                private intParameter CREATED_BY_USER_RID;
                private datetimeParameter CREATED_DATETIME;
                private intParameter LAST_MODIFIED_BY_USER_RID;
                private datetimeParameter LAST_MODIFIED_DATETIME;
                private  intParameter TASKLIST_RID; //Declare Output Parameter
			
			    public SP_MID_TASKLIST_INSERT_def()
			    {
                    base.procedureName = "SP_MID_TASKLIST_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("TASKLIST");
			        TASKLIST_NAME = new stringParameter("@TASKLIST_NAME", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        SYSTEM_GENERATED_IND = new charParameter("@SYSTEM_GENERATED_IND", base.inputParameterList);
			        CREATED_BY_USER_RID = new intParameter("@CREATED_BY_USER_RID", base.inputParameterList);
			        CREATED_DATETIME = new datetimeParameter("@CREATED_DATETIME", base.inputParameterList);
			        LAST_MODIFIED_BY_USER_RID = new intParameter("@LAST_MODIFIED_BY_USER_RID", base.inputParameterList);
			        LAST_MODIFIED_DATETIME = new datetimeParameter("@LAST_MODIFIED_DATETIME", base.inputParameterList);
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string TASKLIST_NAME,
			                                  int? USER_RID,
			                                  char? SYSTEM_GENERATED_IND,
			                                  int? CREATED_BY_USER_RID,
			                                  DateTime? CREATED_DATETIME,
			                                  int? LAST_MODIFIED_BY_USER_RID,
			                                  DateTime? LAST_MODIFIED_DATETIME
			                                  )
			    {
                    lock (typeof(SP_MID_TASKLIST_INSERT_def))
                    {
                        this.TASKLIST_NAME.SetValue(TASKLIST_NAME);
                        this.USER_RID.SetValue(USER_RID);
                        this.SYSTEM_GENERATED_IND.SetValue(SYSTEM_GENERATED_IND);
                        this.CREATED_BY_USER_RID.SetValue(CREATED_BY_USER_RID);
                        this.CREATED_DATETIME.SetValue(CREATED_DATETIME);
                        this.LAST_MODIFIED_BY_USER_RID.SetValue(LAST_MODIFIED_BY_USER_RID);
                        this.LAST_MODIFIED_DATETIME.SetValue(LAST_MODIFIED_DATETIME);
                        this.TASKLIST_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_TASKLIST_UPDATE_def MID_TASKLIST_UPDATE = new MID_TASKLIST_UPDATE_def();
			public class MID_TASKLIST_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASKLIST_UPDATE.SQL"

                private intParameter TASKLIST_RID;
                private stringParameter TASKLIST_NAME;
                private intParameter USER_RID;
                private intParameter LAST_MODIFIED_BY_USER_RID;
                private datetimeParameter LAST_MODIFIED_DATETIME;
			
			    public MID_TASKLIST_UPDATE_def()
			    {
			        base.procedureName = "MID_TASKLIST_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("TASKLIST");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASKLIST_NAME = new stringParameter("@TASKLIST_NAME", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        LAST_MODIFIED_BY_USER_RID = new intParameter("@LAST_MODIFIED_BY_USER_RID", base.inputParameterList);
			        LAST_MODIFIED_DATETIME = new datetimeParameter("@LAST_MODIFIED_DATETIME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      string TASKLIST_NAME,
			                      int? USER_RID,
			                      int? LAST_MODIFIED_BY_USER_RID,
			                      DateTime? LAST_MODIFIED_DATETIME
			                      )
			    {
                    lock (typeof(MID_TASKLIST_UPDATE_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASKLIST_NAME.SetValue(TASKLIST_NAME);
                        this.USER_RID.SetValue(USER_RID);
                        this.LAST_MODIFIED_BY_USER_RID.SetValue(LAST_MODIFIED_BY_USER_RID);
                        this.LAST_MODIFIED_DATETIME.SetValue(LAST_MODIFIED_DATETIME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

            public static SP_MID_SCHEDULE_PROCESS_INSERT_def SP_MID_SCHEDULE_PROCESS_INSERT = new SP_MID_SCHEDULE_PROCESS_INSERT_def();
            public class SP_MID_SCHEDULE_PROCESS_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SCHEDULE_PROCESS_INSERT.SQL"

                private intParameter SCHEDULE_PROCESS_ID; //Declare Output Parameter
			
			    public SP_MID_SCHEDULE_PROCESS_INSERT_def()
			    {
                    base.procedureName = "SP_MID_SCHEDULE_PROCESS_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("SCHEDULE_PROCESS");
			        SCHEDULE_PROCESS_ID = new intParameter("@SCHEDULE_PROCESS_ID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba)
			    {
                    lock (typeof(SP_MID_SCHEDULE_PROCESS_INSERT_def))
                    {
                        this.SCHEDULE_PROCESS_ID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_DELETE_def MID_SCHEDULE_DELETE = new MID_SCHEDULE_DELETE_def();
			public class MID_SCHEDULE_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_DELETE.SQL"

                private intParameter SCHED_RID;
			
			    public MID_SCHEDULE_DELETE_def()
			    {
			        base.procedureName = "MID_SCHEDULE_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SCHEDULE");
			        SCHED_RID = new intParameter("@SCHED_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SCHED_RID)
			    {
                    lock (typeof(MID_SCHEDULE_DELETE_def))
                    {
                        this.SCHED_RID.SetValue(SCHED_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_JOB_READ_def MID_JOB_READ = new MID_JOB_READ_def();
			public class MID_JOB_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_READ.SQL"

                private intParameter JOB_RID;
			
			    public MID_JOB_READ_def()
			    {
			        base.procedureName = "MID_JOB_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("JOB");
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? JOB_RID)
			    {
                    lock (typeof(MID_JOB_READ_def))
                    {
                        this.JOB_RID.SetValue(JOB_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_JOB_READ_NON_SYSTEM_PARENT_def MID_JOB_READ_NON_SYSTEM_PARENT = new MID_JOB_READ_NON_SYSTEM_PARENT_def();
			public class MID_JOB_READ_NON_SYSTEM_PARENT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_READ_NON_SYSTEM_PARENT.SQL"

                private intParameter CHILD_ITEM_TYPE;
			
			    public MID_JOB_READ_NON_SYSTEM_PARENT_def()
			    {
			        base.procedureName = "MID_JOB_READ_NON_SYSTEM_PARENT";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("JOB");
			        CHILD_ITEM_TYPE = new intParameter("@CHILD_ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? CHILD_ITEM_TYPE)
			    {
                    lock (typeof(MID_JOB_READ_NON_SYSTEM_PARENT_def))
                    {
                        this.CHILD_ITEM_TYPE.SetValue(CHILD_ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_JOB_READ_FROM_NAME_def MID_JOB_READ_FROM_NAME = new MID_JOB_READ_FROM_NAME_def();
			public class MID_JOB_READ_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_READ_FROM_NAME.SQL"

                private stringParameter JOB_NAME;
			
			    public MID_JOB_READ_FROM_NAME_def()
			    {
			        base.procedureName = "MID_JOB_READ_FROM_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("JOB");
			        JOB_NAME = new stringParameter("@JOB_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string JOB_NAME)
			    {
                    lock (typeof(MID_JOB_READ_FROM_NAME_def))
                    {
                        this.JOB_NAME.SetValue(JOB_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_JOB_READ_FROM_USER_def MID_JOB_READ_FROM_USER = new MID_JOB_READ_FROM_USER_def();
			public class MID_JOB_READ_FROM_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_READ_FROM_USER.SQL"

                private intParameter USER_RID;
			
			    public MID_JOB_READ_FROM_USER_def()
			    {
			        base.procedureName = "MID_JOB_READ_FROM_USER";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("JOB");
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? USER_RID)
			    {
                    lock (typeof(MID_JOB_READ_FROM_USER_def))
                    {
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_JOB_INSERT_def SP_MID_JOB_INSERT = new SP_MID_JOB_INSERT_def();
            public class SP_MID_JOB_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_JOB_INSERT.SQL"

                private stringParameter JOB_NAME;
                private charParameter SYSTEM_GENERATED_IND;
                private intParameter CREATED_BY_USER_RID;
                private datetimeParameter CREATED_DATETIME;
                private intParameter LAST_MODIFIED_BY_USER_RID;
                private datetimeParameter LAST_MODIFIED_DATETIME;
                private intParameter JOB_RID; //Declare Output Parameter
			
			    public SP_MID_JOB_INSERT_def()
			    {
                    base.procedureName = "SP_MID_JOB_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("JOB");
			        JOB_NAME = new stringParameter("@JOB_NAME", base.inputParameterList);
			        SYSTEM_GENERATED_IND = new charParameter("@SYSTEM_GENERATED_IND", base.inputParameterList);
			        CREATED_BY_USER_RID = new intParameter("@CREATED_BY_USER_RID", base.inputParameterList);
			        CREATED_DATETIME = new datetimeParameter("@CREATED_DATETIME", base.inputParameterList);
			        LAST_MODIFIED_BY_USER_RID = new intParameter("@LAST_MODIFIED_BY_USER_RID", base.inputParameterList);
			        LAST_MODIFIED_DATETIME = new datetimeParameter("@LAST_MODIFIED_DATETIME", base.inputParameterList);
			        JOB_RID = new intParameter("@JOB_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string JOB_NAME,
			                                  char? SYSTEM_GENERATED_IND,
			                                  int? CREATED_BY_USER_RID,
			                                  DateTime? CREATED_DATETIME,
			                                  int? LAST_MODIFIED_BY_USER_RID,
			                                  DateTime? LAST_MODIFIED_DATETIME
			                                  )
			    {
                    lock (typeof(SP_MID_JOB_INSERT_def))
                    {
                        this.JOB_NAME.SetValue(JOB_NAME);
                        this.SYSTEM_GENERATED_IND.SetValue(SYSTEM_GENERATED_IND);
                        this.CREATED_BY_USER_RID.SetValue(CREATED_BY_USER_RID);
                        this.CREATED_DATETIME.SetValue(CREATED_DATETIME);
                        this.LAST_MODIFIED_BY_USER_RID.SetValue(LAST_MODIFIED_BY_USER_RID);
                        this.LAST_MODIFIED_DATETIME.SetValue(LAST_MODIFIED_DATETIME);
                        this.JOB_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

			public static MID_JOB_UPDATE_def MID_JOB_UPDATE = new MID_JOB_UPDATE_def();
			public class MID_JOB_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_UPDATE.SQL"

                private intParameter JOB_RID;
                private stringParameter JOB_NAME;
                private intParameter LAST_MODIFIED_BY_USER_RID;
                private datetimeParameter LAST_MODIFIED_DATETIME;
			
			    public MID_JOB_UPDATE_def()
			    {
			        base.procedureName = "MID_JOB_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("JOB");
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			        JOB_NAME = new stringParameter("@JOB_NAME", base.inputParameterList);
			        LAST_MODIFIED_BY_USER_RID = new intParameter("@LAST_MODIFIED_BY_USER_RID", base.inputParameterList);
			        LAST_MODIFIED_DATETIME = new datetimeParameter("@LAST_MODIFIED_DATETIME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? JOB_RID,
			                      string JOB_NAME,
			                      int? LAST_MODIFIED_BY_USER_RID,
			                      DateTime? LAST_MODIFIED_DATETIME
			                      )
			    {
                    lock (typeof(MID_JOB_UPDATE_def))
                    {
                        this.JOB_RID.SetValue(JOB_RID);
                        this.JOB_NAME.SetValue(JOB_NAME);
                        this.LAST_MODIFIED_BY_USER_RID.SetValue(LAST_MODIFIED_BY_USER_RID);
                        this.LAST_MODIFIED_DATETIME.SetValue(LAST_MODIFIED_DATETIME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_JOB_DELETE_def MID_JOB_DELETE = new MID_JOB_DELETE_def();
			public class MID_JOB_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_DELETE.SQL"

                private intParameter JOB_RID;
			
			    public MID_JOB_DELETE_def()
			    {
			        base.procedureName = "MID_JOB_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("JOB");
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? JOB_RID)
			    {
                    lock (typeof(MID_JOB_DELETE_def))
                    {
                        this.JOB_RID.SetValue(JOB_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_JOB_DELETE_SYSTEM_JOBS_def MID_JOB_DELETE_SYSTEM_JOBS = new MID_JOB_DELETE_SYSTEM_JOBS_def();
			public class MID_JOB_DELETE_SYSTEM_JOBS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_DELETE_SYSTEM_JOBS.SQL"

                private intParameter JOB_RID;
			
			    public MID_JOB_DELETE_SYSTEM_JOBS_def()
			    {
			        base.procedureName = "MID_JOB_DELETE_SYSTEM_JOBS";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("JOB");
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? JOB_RID)
			    {
                    lock (typeof(MID_JOB_DELETE_SYSTEM_JOBS_def))
                    {
                        this.JOB_RID.SetValue(JOB_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SPECIAL_REQUEST_JOB_READ_SPECIAL_REQUESTS_def MID_SPECIAL_REQUEST_JOB_READ_SPECIAL_REQUESTS = new MID_SPECIAL_REQUEST_JOB_READ_SPECIAL_REQUESTS_def();
			public class MID_SPECIAL_REQUEST_JOB_READ_SPECIAL_REQUESTS_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_READ_SPECIAL_REQUESTS.SQL"

                private intParameter CHILD_ITEM_TYPE;
			
			    public MID_SPECIAL_REQUEST_JOB_READ_SPECIAL_REQUESTS_def()
			    {
			        base.procedureName = "MID_SPECIAL_REQUEST_JOB_READ_SPECIAL_REQUESTS";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB");
			        CHILD_ITEM_TYPE = new intParameter("@CHILD_ITEM_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? CHILD_ITEM_TYPE)
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_READ_SPECIAL_REQUESTS_def))
                    {
                        this.CHILD_ITEM_TYPE.SetValue(CHILD_ITEM_TYPE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SPECIAL_REQUEST_JOB_READ_FROM_RID_def MID_SPECIAL_REQUEST_JOB_READ_FROM_RID = new MID_SPECIAL_REQUEST_JOB_READ_FROM_RID_def();
			public class MID_SPECIAL_REQUEST_JOB_READ_FROM_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_READ_FROM_RID.SQL"

                private intParameter SPECIAL_REQ_RID;
			
			    public MID_SPECIAL_REQUEST_JOB_READ_FROM_RID_def()
			    {
			        base.procedureName = "MID_SPECIAL_REQUEST_JOB_READ_FROM_RID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB");
			        SPECIAL_REQ_RID = new intParameter("@SPECIAL_REQ_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SPECIAL_REQ_RID)
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_READ_FROM_RID_def))
                    {
                        this.SPECIAL_REQ_RID.SetValue(SPECIAL_REQ_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SPECIAL_REQUEST_JOB_JOIN_READ_def MID_SPECIAL_REQUEST_JOB_JOIN_READ = new MID_SPECIAL_REQUEST_JOB_JOIN_READ_def();
			public class MID_SPECIAL_REQUEST_JOB_JOIN_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_JOIN_READ.SQL"

                private intParameter SPECIAL_REQ_RID;
			
			    public MID_SPECIAL_REQUEST_JOB_JOIN_READ_def()
			    {
			        base.procedureName = "MID_SPECIAL_REQUEST_JOB_JOIN_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB_JOIN");
			        SPECIAL_REQ_RID = new intParameter("@SPECIAL_REQ_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SPECIAL_REQ_RID)
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_JOIN_READ_def))
                    {
                        this.SPECIAL_REQ_RID.SetValue(SPECIAL_REQ_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SPECIAL_REQUEST_JOB_READ_FROM_NAME_def MID_SPECIAL_REQUEST_JOB_READ_FROM_NAME = new MID_SPECIAL_REQUEST_JOB_READ_FROM_NAME_def();
			public class MID_SPECIAL_REQUEST_JOB_READ_FROM_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_READ_FROM_NAME.SQL"

                private stringParameter SPECIAL_REQ_NAME;
			
			    public MID_SPECIAL_REQUEST_JOB_READ_FROM_NAME_def()
			    {
			        base.procedureName = "MID_SPECIAL_REQUEST_JOB_READ_FROM_NAME";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB");
			        SPECIAL_REQ_NAME = new stringParameter("@SPECIAL_REQ_NAME", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, string SPECIAL_REQ_NAME)
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_READ_FROM_NAME_def))
                    {
                        this.SPECIAL_REQ_NAME.SetValue(SPECIAL_REQ_NAME);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

            public static SP_MID_SPECIAL_REQ_JOB_INSERT_def SP_MID_SPECIAL_REQ_JOB_INSERT = new SP_MID_SPECIAL_REQ_JOB_INSERT_def();
            public class SP_MID_SPECIAL_REQ_JOB_INSERT_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\SP_MID_SPECIAL_REQ_JOB_INSERT.SQL"

			    private stringParameter SPECIAL_REQ_NAME;
			    private intParameter CONCURRENT_PROCESSES;
			    private intParameter SPECIAL_REQ_RID; //Declare Output Parameter
			
			    public SP_MID_SPECIAL_REQ_JOB_INSERT_def()
			    {
                    base.procedureName = "SP_MID_SPECIAL_REQ_JOB_INSERT";
			        base.procedureType = storedProcedureTypes.InsertAndReturnRID;
			        base.tableNames.Add("SPECIAL_REQ_JOB");
			        SPECIAL_REQ_NAME = new stringParameter("@SPECIAL_REQ_NAME", base.inputParameterList);
			        CONCURRENT_PROCESSES = new intParameter("@CONCURRENT_PROCESSES", base.inputParameterList);
			        SPECIAL_REQ_RID = new intParameter("@SPECIAL_REQ_RID", base.outputParameterList); //Add Output Parameter
			    }
			
			    public int InsertAndReturnRID(DatabaseAccess _dba, 
			                                  string SPECIAL_REQ_NAME,
			                                  int? CONCURRENT_PROCESSES
			                                  )
			    {
                    lock (typeof(SP_MID_SPECIAL_REQ_JOB_INSERT_def))
                    {
                        this.SPECIAL_REQ_NAME.SetValue(SPECIAL_REQ_NAME);
                        this.CONCURRENT_PROCESSES.SetValue(CONCURRENT_PROCESSES);
                        this.SPECIAL_REQ_RID.SetValue(null); //Initialize Output Parameter
                        return ExecuteStoredProcedureForInsertAndReturnRID(_dba);
                    }
			    }
			}

            public static MID_SPECIAL_REQUEST_JOB_UPDATE_NAME_def MID_SPECIAL_REQUEST_JOB_UPDATE_NAME = new MID_SPECIAL_REQUEST_JOB_UPDATE_NAME_def();
            public class MID_SPECIAL_REQUEST_JOB_UPDATE_NAME_def : baseStoredProcedure
			{
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_UPDATE_NAME.SQL"

                private intParameter SPECIAL_REQ_RID;
                private stringParameter SPECIAL_REQ_NAME;
			
			    public MID_SPECIAL_REQUEST_JOB_UPDATE_NAME_def()
			    {
                    base.procedureName = "MID_SPECIAL_REQUEST_JOB_UPDATE_NAME";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB");
			        SPECIAL_REQ_RID = new intParameter("@SPECIAL_REQ_RID", base.inputParameterList);
			        SPECIAL_REQ_NAME = new stringParameter("@SPECIAL_REQ_NAME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? SPECIAL_REQ_RID,
			                      string SPECIAL_REQ_NAME
			                      )
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_UPDATE_NAME_def))
                    {
                        this.SPECIAL_REQ_RID.SetValue(SPECIAL_REQ_RID);
                        this.SPECIAL_REQ_NAME.SetValue(SPECIAL_REQ_NAME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_SPECIAL_REQUEST_JOB_UPDATE_def MID_SPECIAL_REQUEST_JOB_UPDATE = new MID_SPECIAL_REQUEST_JOB_UPDATE_def();
			public class MID_SPECIAL_REQUEST_JOB_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_UPDATE.SQL"

			    private intParameter SPECIAL_REQ_RID;
			    private stringParameter SPECIAL_REQ_NAME;
			    private intParameter CONCURRENT_PROCESSES;
			
			    public MID_SPECIAL_REQUEST_JOB_UPDATE_def()
			    {
			        base.procedureName = "MID_SPECIAL_REQUEST_JOB_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB");
			        SPECIAL_REQ_RID = new intParameter("@SPECIAL_REQ_RID", base.inputParameterList);
			        SPECIAL_REQ_NAME = new stringParameter("@SPECIAL_REQ_NAME", base.inputParameterList);
			        CONCURRENT_PROCESSES = new intParameter("@CONCURRENT_PROCESSES", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? SPECIAL_REQ_RID,
			                      string SPECIAL_REQ_NAME,
			                      int? CONCURRENT_PROCESSES
			                      )
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_UPDATE_def))
                    {
                        this.SPECIAL_REQ_RID.SetValue(SPECIAL_REQ_RID);
                        this.SPECIAL_REQ_NAME.SetValue(SPECIAL_REQ_NAME);
                        this.CONCURRENT_PROCESSES.SetValue(CONCURRENT_PROCESSES);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_SPECIAL_REQUEST_JOB_DELETE_def MID_SPECIAL_REQUEST_JOB_DELETE = new MID_SPECIAL_REQUEST_JOB_DELETE_def();
			public class MID_SPECIAL_REQUEST_JOB_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_DELETE.SQL"

                private intParameter SPECIAL_REQ_RID;
			
			    public MID_SPECIAL_REQUEST_JOB_DELETE_def()
			    {
			        base.procedureName = "MID_SPECIAL_REQUEST_JOB_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB");
			        SPECIAL_REQ_RID = new intParameter("@SPECIAL_REQ_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SPECIAL_REQ_RID)
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_DELETE_def))
                    {
                        this.SPECIAL_REQ_RID.SetValue(SPECIAL_REQ_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SPECIAL_REQUEST_JOB_JOIN_READ_FROM_REQUEST_RID_def MID_SPECIAL_REQUEST_JOB_JOIN_READ_FROM_REQUEST_RID = new MID_SPECIAL_REQUEST_JOB_JOIN_READ_FROM_REQUEST_RID_def();
			public class MID_SPECIAL_REQUEST_JOB_JOIN_READ_FROM_REQUEST_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_JOIN_READ_FROM_REQUEST_RID.SQL"

                private intParameter SPECIAL_REQ_RID;
			
			    public MID_SPECIAL_REQUEST_JOB_JOIN_READ_FROM_REQUEST_RID_def()
			    {
			        base.procedureName = "MID_SPECIAL_REQUEST_JOB_JOIN_READ_FROM_REQUEST_RID";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB_JOIN");
			        SPECIAL_REQ_RID = new intParameter("@SPECIAL_REQ_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? SPECIAL_REQ_RID)
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_JOIN_READ_FROM_REQUEST_RID_def))
                    {
                        this.SPECIAL_REQ_RID.SetValue(SPECIAL_REQ_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_FROM_REQUEST_RID_def MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_FROM_REQUEST_RID = new MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_FROM_REQUEST_RID_def();
			public class MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_FROM_REQUEST_RID_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_FROM_REQUEST_RID.SQL"

                private intParameter SPECIAL_REQ_RID;
			
			    public MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_FROM_REQUEST_RID_def()
			    {
			        base.procedureName = "MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_FROM_REQUEST_RID";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB_JOIN");
			        SPECIAL_REQ_RID = new intParameter("@SPECIAL_REQ_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? SPECIAL_REQ_RID)
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_FROM_REQUEST_RID_def))
                    {
                        this.SPECIAL_REQ_RID.SetValue(SPECIAL_REQ_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_def MID_SPECIAL_REQUEST_JOB_JOIN_DELETE = new MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_def();
			public class MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_JOIN_DELETE.SQL"

                private intParameter SPECIAL_REQ_RID;
                private intParameter JOB_RID;
			
			    public MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_def()
			    {
			        base.procedureName = "MID_SPECIAL_REQUEST_JOB_JOIN_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB_JOIN");
			        SPECIAL_REQ_RID = new intParameter("@SPECIAL_REQ_RID", base.inputParameterList);
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? SPECIAL_REQ_RID,
			                      int? JOB_RID
			                      )
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_def))
                    {
                        this.SPECIAL_REQ_RID.SetValue(SPECIAL_REQ_RID);
                        this.JOB_RID.SetValue(JOB_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_SPECIAL_REQUEST_JOB_JOIN_INSERT_def MID_SPECIAL_REQUEST_JOB_JOIN_INSERT = new MID_SPECIAL_REQUEST_JOB_JOIN_INSERT_def();
			public class MID_SPECIAL_REQUEST_JOB_JOIN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_JOIN_INSERT.SQL"

                private intParameter SPECIAL_REQ_RID;
                private intParameter JOB_RID;
                private intParameter JOB_SEQUENCE;
			
			    public MID_SPECIAL_REQUEST_JOB_JOIN_INSERT_def()
			    {
			        base.procedureName = "MID_SPECIAL_REQUEST_JOB_JOIN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB_JOIN");
			        SPECIAL_REQ_RID = new intParameter("@SPECIAL_REQ_RID", base.inputParameterList);
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			        JOB_SEQUENCE = new intParameter("@JOB_SEQUENCE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? SPECIAL_REQ_RID,
			                      int? JOB_RID,
			                      int? JOB_SEQUENCE
			                      )
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_JOIN_INSERT_def))
                    {
                        this.SPECIAL_REQ_RID.SetValue(SPECIAL_REQ_RID);
                        this.JOB_RID.SetValue(JOB_RID);
                        this.JOB_SEQUENCE.SetValue(JOB_SEQUENCE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_JOB_JOIN_INSERT_def MID_SCHEDULE_JOB_JOIN_INSERT = new MID_SCHEDULE_JOB_JOIN_INSERT_def();
			public class MID_SCHEDULE_JOB_JOIN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_JOB_JOIN_INSERT.SQL"

                private intParameter SCHED_RID;
                private intParameter JOB_RID;
                private intParameter USER_RID;
                private intParameter EXECUTION_STATUS;
                private intParameter LAST_COMPLETION_STATUS;
                private datetimeParameter LAST_RUN_DATETIME;
                private datetimeParameter LAST_COMPLETION_DATETIME;
                private datetimeParameter NEXT_RUN_DATETIME;
                private datetimeParameter REPEAT_UNTIL_DATETIME;
			
			    public MID_SCHEDULE_JOB_JOIN_INSERT_def()
			    {
			        base.procedureName = "MID_SCHEDULE_JOB_JOIN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("SCHEDULE_JOB_JOIN");
			        SCHED_RID = new intParameter("@SCHED_RID", base.inputParameterList);
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        EXECUTION_STATUS = new intParameter("@EXECUTION_STATUS", base.inputParameterList);
			        LAST_COMPLETION_STATUS = new intParameter("@LAST_COMPLETION_STATUS", base.inputParameterList);
			        LAST_RUN_DATETIME = new datetimeParameter("@LAST_RUN_DATETIME", base.inputParameterList);
			        LAST_COMPLETION_DATETIME = new datetimeParameter("@LAST_COMPLETION_DATETIME", base.inputParameterList);
			        NEXT_RUN_DATETIME = new datetimeParameter("@NEXT_RUN_DATETIME", base.inputParameterList);
			        REPEAT_UNTIL_DATETIME = new datetimeParameter("@REPEAT_UNTIL_DATETIME", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? SCHED_RID,
			                      int? JOB_RID,
			                      int? USER_RID,
			                      int? EXECUTION_STATUS,
			                      int? LAST_COMPLETION_STATUS,
			                      DateTime? LAST_RUN_DATETIME,
			                      DateTime? LAST_COMPLETION_DATETIME,
			                      DateTime? NEXT_RUN_DATETIME,
			                      DateTime? REPEAT_UNTIL_DATETIME
			                      )
			    {
                    lock (typeof(MID_SCHEDULE_JOB_JOIN_INSERT_def))
                    {
                        this.SCHED_RID.SetValue(SCHED_RID);
                        this.JOB_RID.SetValue(JOB_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.EXECUTION_STATUS.SetValue(EXECUTION_STATUS);
                        this.LAST_COMPLETION_STATUS.SetValue(LAST_COMPLETION_STATUS);
                        this.LAST_RUN_DATETIME.SetValue(LAST_RUN_DATETIME);
                        this.LAST_COMPLETION_DATETIME.SetValue(LAST_COMPLETION_DATETIME);
                        this.NEXT_RUN_DATETIME.SetValue(NEXT_RUN_DATETIME);
                        this.REPEAT_UNTIL_DATETIME.SetValue(REPEAT_UNTIL_DATETIME);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_JOB_JOIN_UPDATE_def MID_SCHEDULE_JOB_JOIN_UPDATE = new MID_SCHEDULE_JOB_JOIN_UPDATE_def();
			public class MID_SCHEDULE_JOB_JOIN_UPDATE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_JOB_JOIN_UPDATE.SQL"

                private intParameter SCHED_RID;
                private intParameter JOB_RID;
                private intParameter USER_RID;
                private intParameter EXECUTION_STATUS;
                private intParameter LAST_COMPLETION_STATUS;
                private datetimeParameter LAST_RUN_DATETIME;
                private datetimeParameter LAST_COMPLETION_DATETIME;
                private datetimeParameter NEXT_RUN_DATETIME;
                private datetimeParameter REPEAT_UNTIL_DATETIME;
                private intParameter HOLD_BY_USER_RID;
                private datetimeParameter HOLD_BY_DATETIME;
                private intParameter RELEASED_BY_USER_RID;
                private datetimeParameter RELEASED_BY_DATETIME;


			    public MID_SCHEDULE_JOB_JOIN_UPDATE_def()
			    {
			        base.procedureName = "MID_SCHEDULE_JOB_JOIN_UPDATE";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("SCHEDULE_JOB_JOIN");
			        SCHED_RID = new intParameter("@SCHED_RID", base.inputParameterList);
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        EXECUTION_STATUS = new intParameter("@EXECUTION_STATUS", base.inputParameterList);
			        LAST_COMPLETION_STATUS = new intParameter("@LAST_COMPLETION_STATUS", base.inputParameterList);
			        LAST_RUN_DATETIME = new datetimeParameter("@LAST_RUN_DATETIME", base.inputParameterList);
			        LAST_COMPLETION_DATETIME = new datetimeParameter("@LAST_COMPLETION_DATETIME", base.inputParameterList);
			        NEXT_RUN_DATETIME = new datetimeParameter("@NEXT_RUN_DATETIME", base.inputParameterList);
			        REPEAT_UNTIL_DATETIME = new datetimeParameter("@REPEAT_UNTIL_DATETIME", base.inputParameterList);
                    HOLD_BY_USER_RID = new intParameter("@HOLD_BY_USER_RID", base.inputParameterList);
                    HOLD_BY_DATETIME = new datetimeParameter("@HOLD_BY_DATETIME", base.inputParameterList);
                    RELEASED_BY_USER_RID = new intParameter("@RELEASED_BY_USER_RID", base.inputParameterList);
                    RELEASED_BY_DATETIME = new datetimeParameter("@RELEASED_BY_DATETIME", base.inputParameterList);
                }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? SCHED_RID,
			                      int? JOB_RID,
			                      int? USER_RID,
			                      int? EXECUTION_STATUS,
			                      int? LAST_COMPLETION_STATUS,
			                      DateTime? LAST_RUN_DATETIME,
			                      DateTime? LAST_COMPLETION_DATETIME,
			                      DateTime? NEXT_RUN_DATETIME,
			                      DateTime? REPEAT_UNTIL_DATETIME,
                                  int? HOLD_BY_USER_RID,
                                  DateTime? HOLD_BY_DATETIME,
                                  int? RELEASED_BY_USER_RID,
                                  DateTime? RELEASED_BY_DATETIME
			                      )
			    {
                    lock (typeof(MID_SCHEDULE_JOB_JOIN_UPDATE_def))
                    {
                        this.SCHED_RID.SetValue(SCHED_RID);
                        this.JOB_RID.SetValue(JOB_RID);
                        this.USER_RID.SetValue(USER_RID);
                        this.EXECUTION_STATUS.SetValue(EXECUTION_STATUS);
                        this.LAST_COMPLETION_STATUS.SetValue(LAST_COMPLETION_STATUS);
                        this.LAST_RUN_DATETIME.SetValue(LAST_RUN_DATETIME);
                        this.LAST_COMPLETION_DATETIME.SetValue(LAST_COMPLETION_DATETIME);
                        this.NEXT_RUN_DATETIME.SetValue(NEXT_RUN_DATETIME);
                        this.REPEAT_UNTIL_DATETIME.SetValue(REPEAT_UNTIL_DATETIME);
                        this.HOLD_BY_USER_RID.SetValue(HOLD_BY_USER_RID);
                        this.HOLD_BY_DATETIME.SetValue(HOLD_BY_DATETIME);
                        this.RELEASED_BY_USER_RID.SetValue(RELEASED_BY_USER_RID);
                        this.RELEASED_BY_DATETIME.SetValue(RELEASED_BY_DATETIME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_JOB_JOIN_DELETE_def MID_SCHEDULE_JOB_JOIN_DELETE = new MID_SCHEDULE_JOB_JOIN_DELETE_def();
			public class MID_SCHEDULE_JOB_JOIN_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_JOB_JOIN_DELETE.SQL"

                private intParameter SCHED_RID;
                private intParameter JOB_RID;
			
			    public MID_SCHEDULE_JOB_JOIN_DELETE_def()
			    {
			        base.procedureName = "MID_SCHEDULE_JOB_JOIN_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("SCHEDULE_JOB_JOIN");
			        SCHED_RID = new intParameter("@SCHED_RID", base.inputParameterList);
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? SCHED_RID,
			                      int? JOB_RID
			                      )
			    {
                    lock (typeof(MID_SCHEDULE_JOB_JOIN_DELETE_def))
                    {
                        this.SCHED_RID.SetValue(SCHED_RID);
                        this.JOB_RID.SetValue(JOB_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASKLIST_READ_KEY_def MID_TASKLIST_READ_KEY = new MID_TASKLIST_READ_KEY_def();
			public class MID_TASKLIST_READ_KEY_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASKLIST_READ_KEY.SQL"

                private stringParameter TASKLIST_NAME;
                private intParameter USER_RID;
			
			    public MID_TASKLIST_READ_KEY_def()
			    {
			        base.procedureName = "MID_TASKLIST_READ_KEY";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASKLIST");
			        TASKLIST_NAME = new stringParameter("@TASKLIST_NAME", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          string TASKLIST_NAME,
			                          int? USER_RID
			                          )
			    {
                    lock (typeof(MID_TASKLIST_READ_KEY_def))
                    {
                        this.TASKLIST_NAME.SetValue(TASKLIST_NAME);
                        this.USER_RID.SetValue(USER_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASKLIST_READ_ALL_def MID_TASKLIST_READ_ALL = new MID_TASKLIST_READ_ALL_def();
			public class MID_TASKLIST_READ_ALL_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASKLIST_READ_ALL.SQL"

			
			    public MID_TASKLIST_READ_ALL_def()
			    {
			        base.procedureName = "MID_TASKLIST_READ_ALL";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASKLIST");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_TASKLIST_READ_ALL_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASKLIST_READ_FOR_ASSIGNED_AND_OWNED_def MID_TASKLIST_READ_FOR_ASSIGNED_AND_OWNED = new MID_TASKLIST_READ_FOR_ASSIGNED_AND_OWNED_def();
			public class MID_TASKLIST_READ_FOR_ASSIGNED_AND_OWNED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASKLIST_READ_FOR_ASSIGNED_AND_OWNED.SQL"

                private tableParameter USER_LIST;
			
			    public MID_TASKLIST_READ_FOR_ASSIGNED_AND_OWNED_def()
			    {
			        base.procedureName = "MID_TASKLIST_READ_FOR_ASSIGNED_AND_OWNED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASKLIST");
			        USER_LIST = new tableParameter("@USER_LIST", "USER_RID_TYPE",  base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, DataTable USER_LIST)
			    {
                    lock (typeof(MID_TASKLIST_READ_FOR_ASSIGNED_AND_OWNED_def))
                    {
                        this.USER_LIST.SetValue(USER_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASKLIST_READ_FOR_ASSIGNED_def MID_TASKLIST_READ_FOR_ASSIGNED = new MID_TASKLIST_READ_FOR_ASSIGNED_def();
			public class MID_TASKLIST_READ_FOR_ASSIGNED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASKLIST_READ_FOR_ASSIGNED.SQL"

                private tableParameter USER_LIST;
			
			    public MID_TASKLIST_READ_FOR_ASSIGNED_def()
			    {
			        base.procedureName = "MID_TASKLIST_READ_FOR_ASSIGNED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASKLIST");
			        USER_LIST = new tableParameter("@USER_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, DataTable USER_LIST)
			    {
                    lock (typeof(MID_TASKLIST_READ_FOR_ASSIGNED_def))
                    {
                        this.USER_LIST.SetValue(USER_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASKLIST_READ_FOR_OWNED_def MID_TASKLIST_READ_FOR_OWNED = new MID_TASKLIST_READ_FOR_OWNED_def();
			public class MID_TASKLIST_READ_FOR_OWNED_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASKLIST_READ_FOR_OWNED.SQL"

                private tableParameter USER_LIST;
			
			    public MID_TASKLIST_READ_FOR_OWNED_def()
			    {
			        base.procedureName = "MID_TASKLIST_READ_FOR_OWNED";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASKLIST");
			        USER_LIST = new tableParameter("@USER_LIST", "USER_RID_TYPE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, DataTable USER_LIST)
			    {
                    lock (typeof(MID_TASKLIST_READ_FOR_OWNED_def))
                    {
                        this.USER_LIST.SetValue(USER_LIST);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASKLIST_READ_def MID_TASKLIST_READ = new MID_TASKLIST_READ_def();
			public class MID_TASKLIST_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASKLIST_READ.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASKLIST_READ_def()
			    {
			        base.procedureName = "MID_TASKLIST_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASKLIST");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASKLIST_READ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_JOB_TASKLIST_JOIN_READ_def MID_JOB_TASKLIST_JOIN_READ = new MID_JOB_TASKLIST_JOIN_READ_def();
			public class MID_JOB_TASKLIST_JOIN_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_TASKLIST_JOIN_READ.SQL"

                private intParameter JOB_RID;
			
			    public MID_JOB_TASKLIST_JOIN_READ_def()
			    {
			        base.procedureName = "MID_JOB_TASKLIST_JOIN_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("JOB_TASKLIST_JOIN");
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? JOB_RID)
			    {
                    lock (typeof(MID_JOB_TASKLIST_JOIN_READ_def))
                    {
                        this.JOB_RID.SetValue(JOB_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASKLIST_UPDATE_NAME_def MID_TASKLIST_UPDATE_NAME = new MID_TASKLIST_UPDATE_NAME_def();
			public class MID_TASKLIST_UPDATE_NAME_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASKLIST_UPDATE_NAME.SQL"

                private intParameter TASKLIST_RID;
                private stringParameter TASKLIST_NAME;
                private intParameter LAST_MODIFIED_BY_USER_RID;
                private datetimeParameter LAST_MODIFIED_DATETIME;
			
			    public MID_TASKLIST_UPDATE_NAME_def()
			    {
			        base.procedureName = "MID_TASKLIST_UPDATE_NAME";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("TASKLIST");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASKLIST_NAME = new stringParameter("@TASKLIST_NAME", base.inputParameterList);
			        LAST_MODIFIED_BY_USER_RID = new intParameter("@LAST_MODIFIED_BY_USER_RID", base.inputParameterList);
			        LAST_MODIFIED_DATETIME = new datetimeParameter("@LAST_MODIFIED_DATETIME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      string TASKLIST_NAME,
			                      int? LAST_MODIFIED_BY_USER_RID,
			                      DateTime? LAST_MODIFIED_DATETIME
			                      )
			    {
                    lock (typeof(MID_TASKLIST_UPDATE_NAME_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASKLIST_NAME.SetValue(TASKLIST_NAME);
                        this.LAST_MODIFIED_BY_USER_RID.SetValue(LAST_MODIFIED_BY_USER_RID);
                        this.LAST_MODIFIED_DATETIME.SetValue(LAST_MODIFIED_DATETIME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_TASKLIST_UPDATE_NAME_AND_USER_def MID_TASKLIST_UPDATE_NAME_AND_USER = new MID_TASKLIST_UPDATE_NAME_AND_USER_def();
			public class MID_TASKLIST_UPDATE_NAME_AND_USER_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASKLIST_UPDATE_NAME_AND_USER.SQL"

                private intParameter TASKLIST_RID;
                private stringParameter TASKLIST_NAME;
                private intParameter USER_RID;
                private intParameter LAST_MODIFIED_BY_USER_RID;
                private datetimeParameter LAST_MODIFIED_DATETIME;
			
			    public MID_TASKLIST_UPDATE_NAME_AND_USER_def()
			    {
			        base.procedureName = "MID_TASKLIST_UPDATE_NAME_AND_USER";
			        base.procedureType = storedProcedureTypes.Update;
			        base.tableNames.Add("TASKLIST");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASKLIST_NAME = new stringParameter("@TASKLIST_NAME", base.inputParameterList);
			        USER_RID = new intParameter("@USER_RID", base.inputParameterList);
			        LAST_MODIFIED_BY_USER_RID = new intParameter("@LAST_MODIFIED_BY_USER_RID", base.inputParameterList);
			        LAST_MODIFIED_DATETIME = new datetimeParameter("@LAST_MODIFIED_DATETIME", base.inputParameterList);
			    }
			
			    public int Update(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      string TASKLIST_NAME,
			                      int? USER_RID,
			                      int? LAST_MODIFIED_BY_USER_RID,
			                      DateTime? LAST_MODIFIED_DATETIME
			                      )
			    {
                    lock (typeof(MID_TASKLIST_UPDATE_NAME_AND_USER_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASKLIST_NAME.SetValue(TASKLIST_NAME);
                        this.USER_RID.SetValue(USER_RID);
                        this.LAST_MODIFIED_BY_USER_RID.SetValue(LAST_MODIFIED_BY_USER_RID);
                        this.LAST_MODIFIED_DATETIME.SetValue(LAST_MODIFIED_DATETIME);
                        return ExecuteStoredProcedureForUpdate(_dba);
                    }
			    }
			}

			public static MID_TASKLIST_DELETE_def MID_TASKLIST_DELETE = new MID_TASKLIST_DELETE_def();
			public class MID_TASKLIST_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASKLIST_DELETE.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASKLIST_DELETE_def()
			    {
			        base.procedureName = "MID_TASKLIST_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASKLIST");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASKLIST_DELETE_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_JOB_TASKLIST_JOIN_READ_FROM_JOB_def MID_JOB_TASKLIST_JOIN_READ_FROM_JOB = new MID_JOB_TASKLIST_JOIN_READ_FROM_JOB_def();
			public class MID_JOB_TASKLIST_JOIN_READ_FROM_JOB_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_TASKLIST_JOIN_READ_FROM_JOB.SQL"

                private intParameter JOB_RID;
			
			    public MID_JOB_TASKLIST_JOIN_READ_FROM_JOB_def()
			    {
			        base.procedureName = "MID_JOB_TASKLIST_JOIN_READ_FROM_JOB";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("JOB_TASKLIST_JOIN");
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? JOB_RID)
			    {
                    lock (typeof(MID_JOB_TASKLIST_JOIN_READ_FROM_JOB_def))
                    {
                        this.JOB_RID.SetValue(JOB_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_JOB_TASKLIST_JOIN_DELETE_def MID_JOB_TASKLIST_JOIN_DELETE = new MID_JOB_TASKLIST_JOIN_DELETE_def();
			public class MID_JOB_TASKLIST_JOIN_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_TASKLIST_JOIN_DELETE.SQL"

                private intParameter JOB_RID;
                private intParameter TASKLIST_RID;
			
			    public MID_JOB_TASKLIST_JOIN_DELETE_def()
			    {
			        base.procedureName = "MID_JOB_TASKLIST_JOIN_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("JOB_TASKLIST_JOIN");
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, 
			                      int? JOB_RID,
			                      int? TASKLIST_RID
			                      )
			    {
                    lock (typeof(MID_JOB_TASKLIST_JOIN_DELETE_def))
                    {
                        this.JOB_RID.SetValue(JOB_RID);
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_JOB_TASKLIST_JOIN_DELETE_FROM_JOB_def MID_JOB_TASKLIST_JOIN_DELETE_FROM_JOB = new MID_JOB_TASKLIST_JOIN_DELETE_FROM_JOB_def();
			public class MID_JOB_TASKLIST_JOIN_DELETE_FROM_JOB_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_TASKLIST_JOIN_DELETE_FROM_JOB.SQL"

                private intParameter JOB_RID;
			
			    public MID_JOB_TASKLIST_JOIN_DELETE_FROM_JOB_def()
			    {
			        base.procedureName = "MID_JOB_TASKLIST_JOIN_DELETE_FROM_JOB";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("JOB_TASKLIST_JOIN");
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? JOB_RID)
			    {
                    lock (typeof(MID_JOB_TASKLIST_JOIN_DELETE_FROM_JOB_def))
                    {
                        this.JOB_RID.SetValue(JOB_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_JOB_TASKLIST_JOIN_INSERT_def MID_JOB_TASKLIST_JOIN_INSERT = new MID_JOB_TASKLIST_JOIN_INSERT_def();
			public class MID_JOB_TASKLIST_JOIN_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_TASKLIST_JOIN_INSERT.SQL"

                private intParameter JOB_RID;
                private intParameter TASKLIST_RID;
                private intParameter TASKLIST_SEQUENCE;
			
			    public MID_JOB_TASKLIST_JOIN_INSERT_def()
			    {
			        base.procedureName = "MID_JOB_TASKLIST_JOIN_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("JOB_TASKLIST_JOIN");
			        JOB_RID = new intParameter("@JOB_RID", base.inputParameterList);
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASKLIST_SEQUENCE = new intParameter("@TASKLIST_SEQUENCE", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? JOB_RID,
			                      int? TASKLIST_RID,
			                      int? TASKLIST_SEQUENCE
			                      )
			    {
                    lock (typeof(MID_JOB_TASKLIST_JOIN_INSERT_def))
                    {
                        this.JOB_RID.SetValue(JOB_RID);
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASKLIST_SEQUENCE.SetValue(TASKLIST_SEQUENCE);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TASK_READ_FROM_TASKLIST_def MID_TASK_READ_FROM_TASKLIST = new MID_TASK_READ_FROM_TASKLIST_def();
			public class MID_TASK_READ_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_READ_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_READ_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_DELETE_FROM_TASKLIST_def MID_TASK_DELETE_FROM_TASKLIST = new MID_TASK_DELETE_FROM_TASKLIST_def();
			public class MID_TASK_DELETE_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_DELETE_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_DELETE_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_DELETE_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASK");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_DELETE_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASK_INSERT_def MID_TASK_INSERT = new MID_TASK_INSERT_def();
			public class MID_TASK_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_INSERT.SQL"

			    private intParameter TASKLIST_RID;
			    private intParameter TASK_SEQUENCE;
			    private intParameter TASK_TYPE;
			    private intParameter MAX_MESSAGE_LEVEL;
			    private stringParameter EMAIL_SUCCESS_FROM;
			    private stringParameter EMAIL_SUCCESS_TO;
			    private stringParameter EMAIL_SUCCESS_CC;
			    private stringParameter EMAIL_SUCCESS_BCC;
			    private stringParameter EMAIL_SUCCESS_SUBJECT;
			    private stringParameter EMAIL_SUCCESS_BODY;
			    private stringParameter EMAIL_FAILURE_FROM;
			    private stringParameter EMAIL_FAILURE_TO;
			    private stringParameter EMAIL_FAILURE_CC;
			    private stringParameter EMAIL_FAILURE_BCC;
			    private stringParameter EMAIL_FAILURE_SUBJECT;
			    private stringParameter EMAIL_FAILURE_BODY;
			
			    public MID_TASK_INSERT_def()
			    {
			        base.procedureName = "MID_TASK_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TASK");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			        TASK_TYPE = new intParameter("@TASK_TYPE", base.inputParameterList);
			        MAX_MESSAGE_LEVEL = new intParameter("@MAX_MESSAGE_LEVEL", base.inputParameterList);
			        EMAIL_SUCCESS_FROM = new stringParameter("@EMAIL_SUCCESS_FROM", base.inputParameterList);
			        EMAIL_SUCCESS_TO = new stringParameter("@EMAIL_SUCCESS_TO", base.inputParameterList);
			        EMAIL_SUCCESS_CC = new stringParameter("@EMAIL_SUCCESS_CC", base.inputParameterList);
			        EMAIL_SUCCESS_BCC = new stringParameter("@EMAIL_SUCCESS_BCC", base.inputParameterList);
			        EMAIL_SUCCESS_SUBJECT = new stringParameter("@EMAIL_SUCCESS_SUBJECT", base.inputParameterList);
			        EMAIL_SUCCESS_BODY = new stringParameter("@EMAIL_SUCCESS_BODY", base.inputParameterList);
			        EMAIL_FAILURE_FROM = new stringParameter("@EMAIL_FAILURE_FROM", base.inputParameterList);
			        EMAIL_FAILURE_TO = new stringParameter("@EMAIL_FAILURE_TO", base.inputParameterList);
			        EMAIL_FAILURE_CC = new stringParameter("@EMAIL_FAILURE_CC", base.inputParameterList);
			        EMAIL_FAILURE_BCC = new stringParameter("@EMAIL_FAILURE_BCC", base.inputParameterList);
			        EMAIL_FAILURE_SUBJECT = new stringParameter("@EMAIL_FAILURE_SUBJECT", base.inputParameterList);
			        EMAIL_FAILURE_BODY = new stringParameter("@EMAIL_FAILURE_BODY", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      int? TASK_SEQUENCE,
			                      int? TASK_TYPE,
			                      int? MAX_MESSAGE_LEVEL,
			                      string EMAIL_SUCCESS_FROM,
			                      string EMAIL_SUCCESS_TO,
			                      string EMAIL_SUCCESS_CC,
			                      string EMAIL_SUCCESS_BCC,
			                      string EMAIL_SUCCESS_SUBJECT,
			                      string EMAIL_SUCCESS_BODY,
			                      string EMAIL_FAILURE_FROM,
			                      string EMAIL_FAILURE_TO,
			                      string EMAIL_FAILURE_CC,
			                      string EMAIL_FAILURE_BCC,
			                      string EMAIL_FAILURE_SUBJECT,
			                      string EMAIL_FAILURE_BODY
			                      )
			    {
                    lock (typeof(MID_TASK_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.TASK_TYPE.SetValue(TASK_TYPE);
                        this.MAX_MESSAGE_LEVEL.SetValue(MAX_MESSAGE_LEVEL);
                        this.EMAIL_SUCCESS_FROM.SetValue(EMAIL_SUCCESS_FROM);
                        this.EMAIL_SUCCESS_TO.SetValue(EMAIL_SUCCESS_TO);
                        this.EMAIL_SUCCESS_CC.SetValue(EMAIL_SUCCESS_CC);
                        this.EMAIL_SUCCESS_BCC.SetValue(EMAIL_SUCCESS_BCC);
                        this.EMAIL_SUCCESS_SUBJECT.SetValue(EMAIL_SUCCESS_SUBJECT);
                        this.EMAIL_SUCCESS_BODY.SetValue(EMAIL_SUCCESS_BODY);
                        this.EMAIL_FAILURE_FROM.SetValue(EMAIL_FAILURE_FROM);
                        this.EMAIL_FAILURE_TO.SetValue(EMAIL_FAILURE_TO);
                        this.EMAIL_FAILURE_CC.SetValue(EMAIL_FAILURE_CC);
                        this.EMAIL_FAILURE_BCC.SetValue(EMAIL_FAILURE_BCC);
                        this.EMAIL_FAILURE_SUBJECT.SetValue(EMAIL_FAILURE_SUBJECT);
                        this.EMAIL_FAILURE_BODY.SetValue(EMAIL_FAILURE_BODY);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TASK_FORECAST_READ_FROM_TASKLIST_def MID_TASK_FORECAST_READ_FROM_TASKLIST = new MID_TASK_FORECAST_READ_FROM_TASKLIST_def();
			public class MID_TASK_FORECAST_READ_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_FORECAST_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_FORECAST_READ_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_FORECAST_READ_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_FORECAST");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_FORECAST_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_FORECAST_READ_FROM_TASKLIST_AND_SEQ_def MID_TASK_FORECAST_READ_FROM_TASKLIST_AND_SEQ = new MID_TASK_FORECAST_READ_FROM_TASKLIST_AND_SEQ_def();
			public class MID_TASK_FORECAST_READ_FROM_TASKLIST_AND_SEQ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_FORECAST_READ_FROM_TASKLIST_AND_SEQ.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
			
			    public MID_TASK_FORECAST_READ_FROM_TASKLIST_AND_SEQ_def()
			    {
			        base.procedureName = "MID_TASK_FORECAST_READ_FROM_TASKLIST_AND_SEQ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_FORECAST");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TASKLIST_RID,
			                          int? TASK_SEQUENCE
			                          )
			    {
                    lock (typeof(MID_TASK_FORECAST_READ_FROM_TASKLIST_AND_SEQ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_FORECAST_DELETE_FROM_TASKLIST_def MID_TASK_FORECAST_DELETE_FROM_TASKLIST = new MID_TASK_FORECAST_DELETE_FROM_TASKLIST_def();
			public class MID_TASK_FORECAST_DELETE_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_FORECAST_DELETE_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_FORECAST_DELETE_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_FORECAST_DELETE_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASK_FORECAST");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_FORECAST_DELETE_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASK_FORECAST_INSERT_def MID_TASK_FORECAST_INSERT = new MID_TASK_FORECAST_INSERT_def();
			public class MID_TASK_FORECAST_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_FORECAST_INSERT.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private intParameter FORECAST_SEQUENCE;
                private intParameter HN_RID;
                private intParameter FV_RID;
			
			    public MID_TASK_FORECAST_INSERT_def()
			    {
			        base.procedureName = "MID_TASK_FORECAST_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TASK_FORECAST");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			        FORECAST_SEQUENCE = new intParameter("@FORECAST_SEQUENCE", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      int? TASK_SEQUENCE,
			                      int? FORECAST_SEQUENCE,
			                      int? HN_RID,
			                      int? FV_RID
			                      )
			    {
                    lock (typeof(MID_TASK_FORECAST_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.FORECAST_SEQUENCE.SetValue(FORECAST_SEQUENCE);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_def MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST = new MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_def();
			public class MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_FORECAST_DETAIL");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_AND_SEQ_def MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_AND_SEQ = new MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_AND_SEQ_def();
			public class MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_AND_SEQ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_AND_SEQ.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private intParameter FORECAST_SEQUENCE;
			
			    public MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_AND_SEQ_def()
			    {
			        base.procedureName = "MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_AND_SEQ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_FORECAST_DETAIL");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			        FORECAST_SEQUENCE = new intParameter("@FORECAST_SEQUENCE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TASKLIST_RID,
			                          int? TASK_SEQUENCE,
			                          int? FORECAST_SEQUENCE
			                          )
			    {
                    lock (typeof(MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_AND_SEQ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.FORECAST_SEQUENCE.SetValue(FORECAST_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_FORECAST_DETAIL_DELETE_FROM_TASKLIST_def MID_TASK_FORECAST_DETAIL_DELETE_FROM_TASKLIST = new MID_TASK_FORECAST_DETAIL_DELETE_FROM_TASKLIST_def();
			public class MID_TASK_FORECAST_DETAIL_DELETE_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_FORECAST_DETAIL_DELETE_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_FORECAST_DETAIL_DELETE_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_FORECAST_DETAIL_DELETE_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASK_FORECAST_DETAIL");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_FORECAST_DETAIL_DELETE_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASK_FORECAST_DETAIL_INSERT_def MID_TASK_FORECAST_DETAIL_INSERT = new MID_TASK_FORECAST_DETAIL_INSERT_def();
			public class MID_TASK_FORECAST_DETAIL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_FORECAST_DETAIL_INSERT.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private intParameter FORECAST_SEQUENCE;
                private intParameter DETAIL_SEQUENCE;
                private intParameter WORKFLOW_METHOD_IND;
                private intParameter METHOD_RID;
                private intParameter WORKFLOW_RID;
                private intParameter EXECUTE_CDR_RID;
			
			    public MID_TASK_FORECAST_DETAIL_INSERT_def()
			    {
			        base.procedureName = "MID_TASK_FORECAST_DETAIL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TASK_FORECAST_DETAIL");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			        FORECAST_SEQUENCE = new intParameter("@FORECAST_SEQUENCE", base.inputParameterList);
			        DETAIL_SEQUENCE = new intParameter("@DETAIL_SEQUENCE", base.inputParameterList);
			        WORKFLOW_METHOD_IND = new intParameter("@WORKFLOW_METHOD_IND", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			        EXECUTE_CDR_RID = new intParameter("@EXECUTE_CDR_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      int? TASK_SEQUENCE,
			                      int? FORECAST_SEQUENCE,
			                      int? DETAIL_SEQUENCE,
			                      int? WORKFLOW_METHOD_IND,
			                      int? METHOD_RID,
			                      int? WORKFLOW_RID,
			                      int? EXECUTE_CDR_RID
			                      )
			    {
                    lock (typeof(MID_TASK_FORECAST_DETAIL_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.FORECAST_SEQUENCE.SetValue(FORECAST_SEQUENCE);
                        this.DETAIL_SEQUENCE.SetValue(DETAIL_SEQUENCE);
                        this.WORKFLOW_METHOD_IND.SetValue(WORKFLOW_METHOD_IND);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.EXECUTE_CDR_RID.SetValue(EXECUTE_CDR_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TASK_FORECAST_DETAIL_READ_FROM_METHOD_def MID_TASK_FORECAST_DETAIL_READ_FROM_METHOD = new MID_TASK_FORECAST_DETAIL_READ_FROM_METHOD_def();
			public class MID_TASK_FORECAST_DETAIL_READ_FROM_METHOD_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_FORECAST_DETAIL_READ_FROM_METHOD.SQL"

                private intParameter METHOD_RID;
			
			    public MID_TASK_FORECAST_DETAIL_READ_FROM_METHOD_def()
			    {
			        base.procedureName = "MID_TASK_FORECAST_DETAIL_READ_FROM_METHOD";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_FORECAST_DETAIL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_TASK_FORECAST_DETAIL_READ_FROM_METHOD_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_AND_SEQ_def MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_AND_SEQ = new MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_AND_SEQ_def();
			public class MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_AND_SEQ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_AND_SEQ.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
			
			    public MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_AND_SEQ_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_AND_SEQ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_SIZE_CURVE_GENERATE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TASKLIST_RID,
			                          int? TASK_SEQUENCE
			                          )
			    {
                    lock (typeof(MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_AND_SEQ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_def MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST = new MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_def();
			public class MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_SIZE_CURVE_GENERATE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_def MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST = new MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_def();
			public class MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASK_SIZE_CURVE_GENERATE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_FOR_NODES_def MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_FOR_NODES = new MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_FOR_NODES_def();
			public class MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_FOR_NODES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_FOR_NODES.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_FOR_NODES_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_FOR_NODES";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_SIZE_CURVE_GENERATE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_FOR_NODES_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_FOR_NODES_def MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_FOR_NODES = new MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_FOR_NODES_def();
			public class MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_FOR_NODES_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_FOR_NODES.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_FOR_NODES_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_FOR_NODES";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASK_SIZE_CURVE_GENERATE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_FOR_NODES_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_CURVE_GENERATE_INSERT_def MID_TASK_SIZE_CURVE_GENERATE_INSERT = new MID_TASK_SIZE_CURVE_GENERATE_INSERT_def();
			public class MID_TASK_SIZE_CURVE_GENERATE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_CURVE_GENERATE_INSERT.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private intParameter GENERATE_SEQUENCE;
                private intParameter METHOD_NODE_TYPE;
                private intParameter HN_RID;
                private intParameter METHOD_RID;
                private intParameter EXECUTE_CDR_RID;
			
			    public MID_TASK_SIZE_CURVE_GENERATE_INSERT_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_CURVE_GENERATE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TASK_SIZE_CURVE_GENERATE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			        GENERATE_SEQUENCE = new intParameter("@GENERATE_SEQUENCE", base.inputParameterList);
			        METHOD_NODE_TYPE = new intParameter("@METHOD_NODE_TYPE", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        EXECUTE_CDR_RID = new intParameter("@EXECUTE_CDR_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      int? TASK_SEQUENCE,
			                      int? GENERATE_SEQUENCE,
			                      int? METHOD_NODE_TYPE,
			                      int? HN_RID,
			                      int? METHOD_RID,
			                      int? EXECUTE_CDR_RID
			                      )
			    {
                    lock (typeof(MID_TASK_SIZE_CURVE_GENERATE_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.GENERATE_SEQUENCE.SetValue(GENERATE_SEQUENCE);
                        this.METHOD_NODE_TYPE.SetValue(METHOD_NODE_TYPE);
                        this.HN_RID.SetValue(HN_RID);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.EXECUTE_CDR_RID.SetValue(EXECUTE_CDR_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_AND_SEQ_def MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_AND_SEQ = new MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_AND_SEQ_def();
			public class MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_AND_SEQ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_AND_SEQ.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
			
			    public MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_AND_SEQ_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_AND_SEQ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_SIZE_CURVE_GENERATE_NODE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TASKLIST_RID,
			                          int? TASK_SEQUENCE
			                          )
			    {
                    lock (typeof(MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_AND_SEQ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_def MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST = new MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_def();
			public class MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_SIZE_CURVE_GENERATE_NODE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_CURVE_GENERATE_NODE_DELETE_FROM_TASKLIST_def MID_TASK_SIZE_CURVE_GENERATE_NODE_DELETE_FROM_TASKLIST = new MID_TASK_SIZE_CURVE_GENERATE_NODE_DELETE_FROM_TASKLIST_def();
			public class MID_TASK_SIZE_CURVE_GENERATE_NODE_DELETE_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_CURVE_GENERATE_NODE_DELETE_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_SIZE_CURVE_GENERATE_NODE_DELETE_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_CURVE_GENERATE_NODE_DELETE_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASK_SIZE_CURVE_GENERATE_NODE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_SIZE_CURVE_GENERATE_NODE_DELETE_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_CURVE_GENERATE_NODE_INSERT_def MID_TASK_SIZE_CURVE_GENERATE_NODE_INSERT = new MID_TASK_SIZE_CURVE_GENERATE_NODE_INSERT_def();
			public class MID_TASK_SIZE_CURVE_GENERATE_NODE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_CURVE_GENERATE_NODE_INSERT.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private intParameter CONCURRENT_PROCESSES;
			
			    public MID_TASK_SIZE_CURVE_GENERATE_NODE_INSERT_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_CURVE_GENERATE_NODE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TASK_SIZE_CURVE_GENERATE_NODE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			        CONCURRENT_PROCESSES = new intParameter("@CONCURRENT_PROCESSES", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      int? TASK_SEQUENCE,
			                      int? CONCURRENT_PROCESSES
			                      )
			    {
                    lock (typeof(MID_TASK_SIZE_CURVE_GENERATE_NODE_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.CONCURRENT_PROCESSES.SetValue(CONCURRENT_PROCESSES);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_def MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST = new MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_def();
			public class MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_SIZE_DAY_TO_WEEK_SUMMARY");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_AND_SEQ_def MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_AND_SEQ = new MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_AND_SEQ_def();
			public class MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_AND_SEQ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_AND_SEQ.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
			
			    public MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_AND_SEQ_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_AND_SEQ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_SIZE_DAY_TO_WEEK_SUMMARY");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TASKLIST_RID,
			                          int? TASK_SEQUENCE
			                          )
			    {
                    lock (typeof(MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_AND_SEQ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_DELETE_FROM_TASKLIST_def MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_DELETE_FROM_TASKLIST = new MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_DELETE_FROM_TASKLIST_def();
			public class MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_DELETE_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_DELETE_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_DELETE_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_DELETE_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASK_SIZE_DAY_TO_WEEK_SUMMARY");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_DELETE_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_INSERT_def MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_INSERT = new MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_INSERT_def();
			public class MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_INSERT.SQL"

			    private intParameter TASKLIST_RID;
			    private intParameter TASK_SEQUENCE;
			    private intParameter CDR_RID;
			    private intParameter HN_RID;
			
			    public MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_INSERT_def()
			    {
			        base.procedureName = "MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TASK_SIZE_DAY_TO_WEEK_SUMMARY");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			        CDR_RID = new intParameter("@CDR_RID", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      int? TASK_SEQUENCE,
			                      int? CDR_RID,
			                      int? HN_RID
			                      )
			    {
                    lock (typeof(MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.CDR_RID.SetValue(CDR_RID);
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TASK_ALLOCATE_DETAIL_READ_FROM_METHOD_def MID_TASK_ALLOCATE_DETAIL_READ_FROM_METHOD = new MID_TASK_ALLOCATE_DETAIL_READ_FROM_METHOD_def();
			public class MID_TASK_ALLOCATE_DETAIL_READ_FROM_METHOD_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ALLOCATE_DETAIL_READ_FROM_METHOD.SQL"

                private intParameter METHOD_RID;
			
			    public MID_TASK_ALLOCATE_DETAIL_READ_FROM_METHOD_def()
			    {
			        base.procedureName = "MID_TASK_ALLOCATE_DETAIL_READ_FROM_METHOD";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_ALLOCATE_DETAIL");
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? METHOD_RID)
			    {
                    lock (typeof(MID_TASK_ALLOCATE_DETAIL_READ_FROM_METHOD_def))
                    {
                        this.METHOD_RID.SetValue(METHOD_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_ALLOCATE_READ_FROM_NODE_def MID_TASK_ALLOCATE_READ_FROM_NODE = new MID_TASK_ALLOCATE_READ_FROM_NODE_def();
			public class MID_TASK_ALLOCATE_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ALLOCATE_READ_FROM_NODE.SQL"

                private intParameter HN_RID;
			
			    public MID_TASK_ALLOCATE_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_TASK_ALLOCATE_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_ALLOCATE");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_TASK_ALLOCATE_READ_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_FORECAST_READ_FROM_NODE_def MID_TASK_FORECAST_READ_FROM_NODE = new MID_TASK_FORECAST_READ_FROM_NODE_def();
			public class MID_TASK_FORECAST_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_FORECAST_READ_FROM_NODE.SQL"

                private intParameter HN_RID;
			
			    public MID_TASK_FORECAST_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_TASK_FORECAST_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_FORECAST");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_TASK_FORECAST_READ_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_FORECAST_BALANCE_READ_FROM_NODE_def MID_TASK_FORECAST_BALANCE_READ_FROM_NODE = new MID_TASK_FORECAST_BALANCE_READ_FROM_NODE_def();
			public class MID_TASK_FORECAST_BALANCE_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_FORECAST_BALANCE_READ_FROM_NODE.SQL"

                private intParameter HN_RID;
			
			    public MID_TASK_FORECAST_BALANCE_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_TASK_FORECAST_BALANCE_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_FORECAST_BALANCE");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_TASK_FORECAST_BALANCE_READ_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_ROLLUP_READ_FROM_NODE_def MID_TASK_ROLLUP_READ_FROM_NODE = new MID_TASK_ROLLUP_READ_FROM_NODE_def();
			public class MID_TASK_ROLLUP_READ_FROM_NODE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ROLLUP_READ_FROM_NODE.SQL"

                private intParameter HN_RID;
			
			    public MID_TASK_ROLLUP_READ_FROM_NODE_def()
			    {
			        base.procedureName = "MID_TASK_ROLLUP_READ_FROM_NODE";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_ROLLUP");
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? HN_RID)
			    {
                    lock (typeof(MID_TASK_ROLLUP_READ_FROM_NODE_def))
                    {
                        this.HN_RID.SetValue(HN_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_FORECAST_DETAIL_READ_FROM_WORKFLOW_def MID_TASK_FORECAST_DETAIL_READ_FROM_WORKFLOW = new MID_TASK_FORECAST_DETAIL_READ_FROM_WORKFLOW_def();
			public class MID_TASK_FORECAST_DETAIL_READ_FROM_WORKFLOW_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_FORECAST_DETAIL_READ_FROM_WORKFLOW.SQL"

                private intParameter WORKFLOW_RID;
			
			    public MID_TASK_FORECAST_DETAIL_READ_FROM_WORKFLOW_def()
			    {
			        base.procedureName = "MID_TASK_FORECAST_DETAIL_READ_FROM_WORKFLOW";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_FORECAST_DETAIL");
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? WORKFLOW_RID)
			    {
                    lock (typeof(MID_TASK_FORECAST_DETAIL_READ_FROM_WORKFLOW_def))
                    {
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_ALLOCATE_READ_FROM_TASKLIST_def MID_TASK_ALLOCATE_READ_FROM_TASKLIST = new MID_TASK_ALLOCATE_READ_FROM_TASKLIST_def();
			public class MID_TASK_ALLOCATE_READ_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ALLOCATE_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_ALLOCATE_READ_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_ALLOCATE_READ_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_ALLOCATE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_ALLOCATE_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_ALLOCATE_READ_FROM_TASKLIST_AND_SEQ_def MID_TASK_ALLOCATE_READ_FROM_TASKLIST_AND_SEQ = new MID_TASK_ALLOCATE_READ_FROM_TASKLIST_AND_SEQ_def();
			public class MID_TASK_ALLOCATE_READ_FROM_TASKLIST_AND_SEQ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ALLOCATE_READ_FROM_TASKLIST_AND_SEQ.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
			
			    public MID_TASK_ALLOCATE_READ_FROM_TASKLIST_AND_SEQ_def()
			    {
			        base.procedureName = "MID_TASK_ALLOCATE_READ_FROM_TASKLIST_AND_SEQ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_ALLOCATE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TASKLIST_RID,
			                          int? TASK_SEQUENCE
			                          )
			    {
                    lock (typeof(MID_TASK_ALLOCATE_READ_FROM_TASKLIST_AND_SEQ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_ALLOCATE_DELETE_FROM_TASKLIST_def MID_TASK_ALLOCATE_DELETE_FROM_TASKLIST = new MID_TASK_ALLOCATE_DELETE_FROM_TASKLIST_def();
			public class MID_TASK_ALLOCATE_DELETE_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ALLOCATE_DELETE_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_ALLOCATE_DELETE_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_ALLOCATE_DELETE_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASK_ALLOCATE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_ALLOCATE_DELETE_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASK_ALLOCATE_INSERT_def MID_TASK_ALLOCATE_INSERT = new MID_TASK_ALLOCATE_INSERT_def();
			public class MID_TASK_ALLOCATE_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ALLOCATE_INSERT.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private intParameter ALLOCATE_SEQUENCE;
                private intParameter HN_RID;
                //private intParameter ALLOCATE_TYPE; //TT#1313-MD -jsobek -Header Filters
                //private stringParameter HEADER_ID; //TT#1313-MD -jsobek -Header Filters
                //private stringParameter PO_ID; //TT#1313-MD -jsobek -Header Filters
                private intParameter FILTER_RID; //TT#1313-MD -jsobek -Header Filters
			
			    public MID_TASK_ALLOCATE_INSERT_def()
			    {
			        base.procedureName = "MID_TASK_ALLOCATE_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TASK_ALLOCATE");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			        ALLOCATE_SEQUENCE = new intParameter("@ALLOCATE_SEQUENCE", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
                    //ALLOCATE_TYPE = new intParameter("@ALLOCATE_TYPE", base.inputParameterList); //TT#1313-MD -jsobek -Header Filters
                    //HEADER_ID = new stringParameter("@HEADER_ID", base.inputParameterList); //TT#1313-MD -jsobek -Header Filters
                    //PO_ID = new stringParameter("@PO_ID", base.inputParameterList); //TT#1313-MD -jsobek -Header Filters
                    FILTER_RID = new intParameter("@FILTER_RID", base.inputParameterList); //TT#1313-MD -jsobek -Header Filters
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      int? TASK_SEQUENCE,
			                      int? ALLOCATE_SEQUENCE,
			                      int? HN_RID,
                                  //int? ALLOCATE_TYPE, //TT#1313-MD -jsobek -Header Filters
                                  //string HEADER_ID, //TT#1313-MD -jsobek -Header Filters
                                  //string PO_ID //TT#1313-MD -jsobek -Header Filters
                                  int? FILTER_RID //TT#1313-MD -jsobek -Header Filters
			                      )
			    {
                    lock (typeof(MID_TASK_ALLOCATE_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.ALLOCATE_SEQUENCE.SetValue(ALLOCATE_SEQUENCE);
                        this.HN_RID.SetValue(HN_RID);
                        //this.ALLOCATE_TYPE.SetValue(ALLOCATE_TYPE); //TT#1313-MD -jsobek -Header Filters
                        //this.HEADER_ID.SetValue(HEADER_ID); //TT#1313-MD -jsobek -Header Filters
                        //this.PO_ID.SetValue(PO_ID); //TT#1313-MD -jsobek -Header Filters
                        this.FILTER_RID.SetValue(FILTER_RID); //TT#1313-MD -jsobek -Header Filters
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TASK_ALLOCATE_DETAIL_READ_FROM_TASKLIST_def MID_TASK_ALLOCATE_DETAIL_READ_FROM_TASKLIST = new MID_TASK_ALLOCATE_DETAIL_READ_FROM_TASKLIST_def();
			public class MID_TASK_ALLOCATE_DETAIL_READ_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ALLOCATE_DETAIL_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_ALLOCATE_DETAIL_READ_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_ALLOCATE_DETAIL_READ_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_ALLOCATE_DETAIL");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_ALLOCATE_DETAIL_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_ALLOCATE_DETAIL_READ_def MID_TASK_ALLOCATE_DETAIL_READ = new MID_TASK_ALLOCATE_DETAIL_READ_def();
			public class MID_TASK_ALLOCATE_DETAIL_READ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ALLOCATE_DETAIL_READ.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private intParameter ALLOCATE_SEQUENCE;
			
			    public MID_TASK_ALLOCATE_DETAIL_READ_def()
			    {
			        base.procedureName = "MID_TASK_ALLOCATE_DETAIL_READ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_ALLOCATE_DETAIL");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			        ALLOCATE_SEQUENCE = new intParameter("@ALLOCATE_SEQUENCE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TASKLIST_RID,
			                          int? TASK_SEQUENCE,
			                          int? ALLOCATE_SEQUENCE
			                          )
			    {
                    lock (typeof(MID_TASK_ALLOCATE_DETAIL_READ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.ALLOCATE_SEQUENCE.SetValue(ALLOCATE_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_ALLOCATE_DETAIL_DELETE_FROM_TASKLIST_def MID_TASK_ALLOCATE_DETAIL_DELETE_FROM_TASKLIST = new MID_TASK_ALLOCATE_DETAIL_DELETE_FROM_TASKLIST_def();
			public class MID_TASK_ALLOCATE_DETAIL_DELETE_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ALLOCATE_DETAIL_DELETE_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_ALLOCATE_DETAIL_DELETE_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_ALLOCATE_DETAIL_DELETE_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASK_ALLOCATE_DETAIL");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_ALLOCATE_DETAIL_DELETE_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASK_ALLOCATE_DETAIL_INSERT_def MID_TASK_ALLOCATE_DETAIL_INSERT = new MID_TASK_ALLOCATE_DETAIL_INSERT_def();
			public class MID_TASK_ALLOCATE_DETAIL_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ALLOCATE_DETAIL_INSERT.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private intParameter ALLOCATE_SEQUENCE;
                private intParameter DETAIL_SEQUENCE;
                private intParameter WORKFLOW_METHOD_IND;
                private intParameter METHOD_RID;
                private intParameter WORKFLOW_RID;
                private intParameter EXECUTE_CDR_RID;
			
			    public MID_TASK_ALLOCATE_DETAIL_INSERT_def()
			    {
			        base.procedureName = "MID_TASK_ALLOCATE_DETAIL_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TASK_ALLOCATE_DETAIL");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			        ALLOCATE_SEQUENCE = new intParameter("@ALLOCATE_SEQUENCE", base.inputParameterList);
			        DETAIL_SEQUENCE = new intParameter("@DETAIL_SEQUENCE", base.inputParameterList);
			        WORKFLOW_METHOD_IND = new intParameter("@WORKFLOW_METHOD_IND", base.inputParameterList);
			        METHOD_RID = new intParameter("@METHOD_RID", base.inputParameterList);
			        WORKFLOW_RID = new intParameter("@WORKFLOW_RID", base.inputParameterList);
			        EXECUTE_CDR_RID = new intParameter("@EXECUTE_CDR_RID", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      int? TASK_SEQUENCE,
			                      int? ALLOCATE_SEQUENCE,
			                      int? DETAIL_SEQUENCE,
			                      int? WORKFLOW_METHOD_IND,
			                      int? METHOD_RID,
			                      int? WORKFLOW_RID,
			                      int? EXECUTE_CDR_RID
			                      )
			    {
                    lock (typeof(MID_TASK_ALLOCATE_DETAIL_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.ALLOCATE_SEQUENCE.SetValue(ALLOCATE_SEQUENCE);
                        this.DETAIL_SEQUENCE.SetValue(DETAIL_SEQUENCE);
                        this.WORKFLOW_METHOD_IND.SetValue(WORKFLOW_METHOD_IND);
                        this.METHOD_RID.SetValue(METHOD_RID);
                        this.WORKFLOW_RID.SetValue(WORKFLOW_RID);
                        this.EXECUTE_CDR_RID.SetValue(EXECUTE_CDR_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TASK_ROLLUP_READ_FROM_TASKLIST_def MID_TASK_ROLLUP_READ_FROM_TASKLIST = new MID_TASK_ROLLUP_READ_FROM_TASKLIST_def();
			public class MID_TASK_ROLLUP_READ_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ROLLUP_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_ROLLUP_READ_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_ROLLUP_READ_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_ROLLUP");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_ROLLUP_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_ROLLUP_READ_FROM_TASKLIST_AND_SEQ_def MID_TASK_ROLLUP_READ_FROM_TASKLIST_AND_SEQ = new MID_TASK_ROLLUP_READ_FROM_TASKLIST_AND_SEQ_def();
			public class MID_TASK_ROLLUP_READ_FROM_TASKLIST_AND_SEQ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ROLLUP_READ_FROM_TASKLIST_AND_SEQ.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
			
			    public MID_TASK_ROLLUP_READ_FROM_TASKLIST_AND_SEQ_def()
			    {
			        base.procedureName = "MID_TASK_ROLLUP_READ_FROM_TASKLIST_AND_SEQ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_ROLLUP");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TASKLIST_RID,
			                          int? TASK_SEQUENCE
			                          )
			    {
                    lock (typeof(MID_TASK_ROLLUP_READ_FROM_TASKLIST_AND_SEQ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_ROLLUP_DELETE_def MID_TASK_ROLLUP_DELETE = new MID_TASK_ROLLUP_DELETE_def();
			public class MID_TASK_ROLLUP_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ROLLUP_DELETE.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_ROLLUP_DELETE_def()
			    {
			        base.procedureName = "MID_TASK_ROLLUP_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASK_ROLLUP");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_ROLLUP_DELETE_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASK_ROLLUP_INSERT_def MID_TASK_ROLLUP_INSERT = new MID_TASK_ROLLUP_INSERT_def();
			public class MID_TASK_ROLLUP_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_ROLLUP_INSERT.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private stringParameter ROLLUP_SEQUENCE;
                private intParameter HN_RID;
                private intParameter FV_RID;
                private intParameter ROLLUP_CDR_RID;
                private intParameter FROM_PH_OFFSET_IND;
                private intParameter FROM_PH_RID;
                private intParameter FROM_PHL_SEQUENCE;
                private intParameter FROM_OFFSET;
                private intParameter TO_PH_OFFSET_IND;
                private intParameter TO_PH_RID;
                private intParameter TO_PHL_SEQUENCE;
                private intParameter TO_OFFSET;
                private charParameter POSTING_IND;
                private charParameter HIERARCHY_LEVELS_IND;
                private charParameter DAY_TO_WEEK_IND;
                private charParameter DAY_IND;
                private charParameter WEEK_IND;
                private charParameter STORE_IND;
                private charParameter CHAIN_IND;
                private charParameter STORE_TO_CHAIN_IND;
                private charParameter INTRANSIT_IND;
                private charParameter RECLASS_IND;
			
			    public MID_TASK_ROLLUP_INSERT_def()
			    {
			        base.procedureName = "MID_TASK_ROLLUP_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TASK_ROLLUP");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
                    ROLLUP_SEQUENCE = new stringParameter("@ROLLUP_SEQUENCE", base.inputParameterList);
			        HN_RID = new intParameter("@HN_RID", base.inputParameterList);
			        FV_RID = new intParameter("@FV_RID", base.inputParameterList);
			        ROLLUP_CDR_RID = new intParameter("@ROLLUP_CDR_RID", base.inputParameterList);
			        FROM_PH_OFFSET_IND = new intParameter("@FROM_PH_OFFSET_IND", base.inputParameterList);
			        FROM_PH_RID = new intParameter("@FROM_PH_RID", base.inputParameterList);
			        FROM_PHL_SEQUENCE = new intParameter("@FROM_PHL_SEQUENCE", base.inputParameterList);
			        FROM_OFFSET = new intParameter("@FROM_OFFSET", base.inputParameterList);
			        TO_PH_OFFSET_IND = new intParameter("@TO_PH_OFFSET_IND", base.inputParameterList);
			        TO_PH_RID = new intParameter("@TO_PH_RID", base.inputParameterList);
			        TO_PHL_SEQUENCE = new intParameter("@TO_PHL_SEQUENCE", base.inputParameterList);
			        TO_OFFSET = new intParameter("@TO_OFFSET", base.inputParameterList);
			        POSTING_IND = new charParameter("@POSTING_IND", base.inputParameterList);
			        HIERARCHY_LEVELS_IND = new charParameter("@HIERARCHY_LEVELS_IND", base.inputParameterList);
			        DAY_TO_WEEK_IND = new charParameter("@DAY_TO_WEEK_IND", base.inputParameterList);
			        DAY_IND = new charParameter("@DAY_IND", base.inputParameterList);
			        WEEK_IND = new charParameter("@WEEK_IND", base.inputParameterList);
			        STORE_IND = new charParameter("@STORE_IND", base.inputParameterList);
			        CHAIN_IND = new charParameter("@CHAIN_IND", base.inputParameterList);
			        STORE_TO_CHAIN_IND = new charParameter("@STORE_TO_CHAIN_IND", base.inputParameterList);
			        INTRANSIT_IND = new charParameter("@INTRANSIT_IND", base.inputParameterList);
			        RECLASS_IND = new charParameter("@RECLASS_IND", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      int? TASK_SEQUENCE,
			                      string ROLLUP_SEQUENCE,
			                      int? HN_RID,
			                      int? FV_RID,
			                      int? ROLLUP_CDR_RID,
			                      int? FROM_PH_OFFSET_IND,
			                      int? FROM_PH_RID,
			                      int? FROM_PHL_SEQUENCE,
			                      int? FROM_OFFSET,
			                      int? TO_PH_OFFSET_IND,
			                      int? TO_PH_RID,
			                      int? TO_PHL_SEQUENCE,
			                      int? TO_OFFSET,
			                      char? POSTING_IND,
			                      char? HIERARCHY_LEVELS_IND,
			                      char? DAY_TO_WEEK_IND,
			                      char? DAY_IND,
			                      char? WEEK_IND,
			                      char? STORE_IND,
			                      char? CHAIN_IND,
			                      char? STORE_TO_CHAIN_IND,
			                      char? INTRANSIT_IND,
			                      char? RECLASS_IND
			                      )
			    {
                    lock (typeof(MID_TASK_ROLLUP_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.ROLLUP_SEQUENCE.SetValue(ROLLUP_SEQUENCE);
                        this.HN_RID.SetValue(HN_RID);
                        this.FV_RID.SetValue(FV_RID);
                        this.ROLLUP_CDR_RID.SetValue(ROLLUP_CDR_RID);
                        this.FROM_PH_OFFSET_IND.SetValue(FROM_PH_OFFSET_IND);
                        this.FROM_PH_RID.SetValue(FROM_PH_RID);
                        this.FROM_PHL_SEQUENCE.SetValue(FROM_PHL_SEQUENCE);
                        this.FROM_OFFSET.SetValue(FROM_OFFSET);
                        this.TO_PH_OFFSET_IND.SetValue(TO_PH_OFFSET_IND);
                        this.TO_PH_RID.SetValue(TO_PH_RID);
                        this.TO_PHL_SEQUENCE.SetValue(TO_PHL_SEQUENCE);
                        this.TO_OFFSET.SetValue(TO_OFFSET);
                        this.POSTING_IND.SetValue(POSTING_IND);
                        this.HIERARCHY_LEVELS_IND.SetValue(HIERARCHY_LEVELS_IND);
                        this.DAY_TO_WEEK_IND.SetValue(DAY_TO_WEEK_IND);
                        this.DAY_IND.SetValue(DAY_IND);
                        this.WEEK_IND.SetValue(WEEK_IND);
                        this.STORE_IND.SetValue(STORE_IND);
                        this.CHAIN_IND.SetValue(CHAIN_IND);
                        this.STORE_TO_CHAIN_IND.SetValue(STORE_TO_CHAIN_IND);
                        this.INTRANSIT_IND.SetValue(INTRANSIT_IND);
                        this.RECLASS_IND.SetValue(RECLASS_IND);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

			public static MID_TASK_POSTING_READ_FROM_TASKLIST_AND_SEQ_def MID_TASK_POSTING_READ_FROM_TASKLIST_AND_SEQ = new MID_TASK_POSTING_READ_FROM_TASKLIST_AND_SEQ_def();
			public class MID_TASK_POSTING_READ_FROM_TASKLIST_AND_SEQ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_POSTING_READ_FROM_TASKLIST_AND_SEQ.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
			
			    public MID_TASK_POSTING_READ_FROM_TASKLIST_AND_SEQ_def()
			    {
			        base.procedureName = "MID_TASK_POSTING_READ_FROM_TASKLIST_AND_SEQ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_POSTING");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TASKLIST_RID,
			                          int? TASK_SEQUENCE
			                          )
			    {
                    lock (typeof(MID_TASK_POSTING_READ_FROM_TASKLIST_AND_SEQ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_POSTING_READ_FROM_TASKLIST_def MID_TASK_POSTING_READ_FROM_TASKLIST = new MID_TASK_POSTING_READ_FROM_TASKLIST_def();
			public class MID_TASK_POSTING_READ_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_POSTING_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_POSTING_READ_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_POSTING_READ_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_POSTING");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_POSTING_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_POSTING_DELETE_FROM_TASKLIST_def MID_TASK_POSTING_DELETE_FROM_TASKLIST = new MID_TASK_POSTING_DELETE_FROM_TASKLIST_def();
			public class MID_TASK_POSTING_DELETE_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_POSTING_DELETE_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_POSTING_DELETE_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_POSTING_DELETE_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASK_POSTING");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_POSTING_DELETE_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASK_POSTING_INSERT_def MID_TASK_POSTING_INSERT = new MID_TASK_POSTING_INSERT_def();
			public class MID_TASK_POSTING_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_POSTING_INSERT.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private stringParameter INPUT_DIRECTORY;
                private stringParameter FILE_MASK;
                private intParameter CONCURRENT_FILES;
                private charParameter RUN_UNTIL_FILE_PRESENT_IND;
                private stringParameter RUN_UNTIL_FILE_MASK;
                private intParameter FILE_PROCESSING_DIRECTION;
			
			    public MID_TASK_POSTING_INSERT_def()
			    {
			        base.procedureName = "MID_TASK_POSTING_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TASK_POSTING");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			        INPUT_DIRECTORY = new stringParameter("@INPUT_DIRECTORY", base.inputParameterList);
			        FILE_MASK = new stringParameter("@FILE_MASK", base.inputParameterList);
			        CONCURRENT_FILES = new intParameter("@CONCURRENT_FILES", base.inputParameterList);
			        RUN_UNTIL_FILE_PRESENT_IND = new charParameter("@RUN_UNTIL_FILE_PRESENT_IND", base.inputParameterList);
			        RUN_UNTIL_FILE_MASK = new stringParameter("@RUN_UNTIL_FILE_MASK", base.inputParameterList);
			        FILE_PROCESSING_DIRECTION = new intParameter("@FILE_PROCESSING_DIRECTION", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      int? TASK_SEQUENCE,
			                      string INPUT_DIRECTORY,
			                      string FILE_MASK,
			                      int? CONCURRENT_FILES,
			                      char? RUN_UNTIL_FILE_PRESENT_IND,
			                      string RUN_UNTIL_FILE_MASK,
			                      int? FILE_PROCESSING_DIRECTION
			                      )
			    {
                    lock (typeof(MID_TASK_POSTING_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.INPUT_DIRECTORY.SetValue(INPUT_DIRECTORY);
                        this.FILE_MASK.SetValue(FILE_MASK);
                        this.CONCURRENT_FILES.SetValue(CONCURRENT_FILES);
                        this.RUN_UNTIL_FILE_PRESENT_IND.SetValue(RUN_UNTIL_FILE_PRESENT_IND);
                        this.RUN_UNTIL_FILE_MASK.SetValue(RUN_UNTIL_FILE_MASK);
                        this.FILE_PROCESSING_DIRECTION.SetValue(FILE_PROCESSING_DIRECTION);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}


            public static MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_AND_SEQ_def MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_AND_SEQ = new MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_AND_SEQ_def();
            public class MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_AND_SEQ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_AND_SEQ.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;

                public MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_AND_SEQ_def()
                {
                    base.procedureName = "MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_AND_SEQ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("TASK_HEADER_RECONCILE");
                    TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
                    TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? TASKLIST_RID,
                                      int? TASK_SEQUENCE
                                      )
                {
                    lock (typeof(MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_AND_SEQ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_def MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST = new MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_def();
            public class MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;

                public MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_def()
                {
                    base.procedureName = "MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("TASK_HEADER_RECONCILE");
                    TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
                {
                    lock (typeof(MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_TASK_HEADER_RECONCILE_DELETE_FROM_TASKLIST_def MID_TASK_HEADER_RECONCILE_DELETE_FROM_TASKLIST = new MID_TASK_HEADER_RECONCILE_DELETE_FROM_TASKLIST_def();
            public class MID_TASK_HEADER_RECONCILE_DELETE_FROM_TASKLIST_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_HEADER_RECONCILE_DELETE_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;

                public MID_TASK_HEADER_RECONCILE_DELETE_FROM_TASKLIST_def()
                {
                    base.procedureName = "MID_TASK_HEADER_RECONCILE_DELETE_FROM_TASKLIST";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("TASK_HEADER_RECONCILE");
                    TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
                {
                    lock (typeof(MID_TASK_HEADER_RECONCILE_DELETE_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_TASK_HEADER_RECONCILE_INSERT_def MID_TASK_HEADER_RECONCILE_INSERT = new MID_TASK_HEADER_RECONCILE_INSERT_def();
            public class MID_TASK_HEADER_RECONCILE_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_HEADER_RECONCILE_INSERT.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private stringParameter INPUT_DIRECTORY;
                private stringParameter OUTPUT_DIRECTORY;
                private stringParameter TRIGGER_SUFFIX;
                private stringParameter REMOVE_TRANS_FILE_NAME;
                private stringParameter REMOVE_TRANS_TRIGGER_SUFFIX;
                private stringParameter HEADER_TYPES;
                private stringParameter HEADER_KEYS_FILE_NAME;


                public MID_TASK_HEADER_RECONCILE_INSERT_def()
                {
                    base.procedureName = "MID_TASK_HEADER_RECONCILE_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("TASK_HEADER_RECONCILE");
                    TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
                    TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
                    INPUT_DIRECTORY = new stringParameter("@INPUT_DIRECTORY", base.inputParameterList);
                    OUTPUT_DIRECTORY = new stringParameter("@OUTPUT_DIRECTORY", base.inputParameterList);
                    TRIGGER_SUFFIX = new stringParameter("@TRIGGER_SUFFIX", base.inputParameterList);
                    REMOVE_TRANS_FILE_NAME = new stringParameter("@REMOVE_TRANS_FILE_NAME", base.inputParameterList);
                    REMOVE_TRANS_TRIGGER_SUFFIX = new stringParameter("@REMOVE_TRANS_TRIGGER_SUFFIX", base.inputParameterList);
                    HEADER_TYPES = new stringParameter("@HEADER_TYPES", base.inputParameterList);
                    HEADER_KEYS_FILE_NAME = new stringParameter("@HEADER_KEYS_FILE_NAME", base.inputParameterList);

                }

                public int Insert(DatabaseAccess _dba,
                                  int? TASKLIST_RID,
                                  int? TASK_SEQUENCE,
                                  string INPUT_DIRECTORY,
                                  string OUTPUT_DIRECTORY,
                                  string TRIGGER_SUFFIX,
                                  string REMOVE_TRANS_FILE_NAME,
                                  string REMOVE_TRANS_TRIGGER_SUFFIX,
                                  string HEADER_TYPES,
                                  string HEADER_KEYS_FILE_NAME
                                  )
                {
                    lock (typeof(MID_TASK_HEADER_RECONCILE_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.INPUT_DIRECTORY.SetValue(INPUT_DIRECTORY);
                        this.OUTPUT_DIRECTORY.SetValue(OUTPUT_DIRECTORY);
                        this.TRIGGER_SUFFIX.SetValue(TRIGGER_SUFFIX);
                        this.REMOVE_TRANS_FILE_NAME.SetValue(REMOVE_TRANS_FILE_NAME);
                        this.REMOVE_TRANS_TRIGGER_SUFFIX.SetValue(REMOVE_TRANS_TRIGGER_SUFFIX);
                        this.HEADER_TYPES.SetValue(HEADER_TYPES);
                        this.HEADER_KEYS_FILE_NAME.SetValue(HEADER_KEYS_FILE_NAME);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }

			public static MID_TASK_PROGRAM_READ_FROM_TASKLIST_AND_SEQ_def MID_TASK_PROGRAM_READ_FROM_TASKLIST_AND_SEQ = new MID_TASK_PROGRAM_READ_FROM_TASKLIST_AND_SEQ_def();
			public class MID_TASK_PROGRAM_READ_FROM_TASKLIST_AND_SEQ_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_PROGRAM_READ_FROM_TASKLIST_AND_SEQ.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
			
			    public MID_TASK_PROGRAM_READ_FROM_TASKLIST_AND_SEQ_def()
			    {
			        base.procedureName = "MID_TASK_PROGRAM_READ_FROM_TASKLIST_AND_SEQ";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_PROGRAM");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, 
			                          int? TASKLIST_RID,
			                          int? TASK_SEQUENCE
			                          )
			    {
                    lock (typeof(MID_TASK_PROGRAM_READ_FROM_TASKLIST_AND_SEQ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_PROGRAM_READ_FROM_TASKLIST_def MID_TASK_PROGRAM_READ_FROM_TASKLIST = new MID_TASK_PROGRAM_READ_FROM_TASKLIST_def();
			public class MID_TASK_PROGRAM_READ_FROM_TASKLIST_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_PROGRAM_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_PROGRAM_READ_FROM_TASKLIST_def()
			    {
			        base.procedureName = "MID_TASK_PROGRAM_READ_FROM_TASKLIST";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASK_PROGRAM");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_PROGRAM_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASK_PROGRAM_DELETE_def MID_TASK_PROGRAM_DELETE = new MID_TASK_PROGRAM_DELETE_def();
			public class MID_TASK_PROGRAM_DELETE_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_PROGRAM_DELETE.SQL"

                private intParameter TASKLIST_RID;
			
			    public MID_TASK_PROGRAM_DELETE_def()
			    {
			        base.procedureName = "MID_TASK_PROGRAM_DELETE";
			        base.procedureType = storedProcedureTypes.Delete;
			        base.tableNames.Add("TASK_PROGRAM");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			    }
			
			    public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
			    {
                    lock (typeof(MID_TASK_PROGRAM_DELETE_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
			    }
			}

			public static MID_TASK_PROGRAM_INSERT_def MID_TASK_PROGRAM_INSERT = new MID_TASK_PROGRAM_INSERT_def();
			public class MID_TASK_PROGRAM_INSERT_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_PROGRAM_INSERT.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private stringParameter PROGRAM_PATH;
                private stringParameter PROGRAM_PARMS;
			
			    public MID_TASK_PROGRAM_INSERT_def()
			    {
			        base.procedureName = "MID_TASK_PROGRAM_INSERT";
			        base.procedureType = storedProcedureTypes.Insert;
			        base.tableNames.Add("TASK_PROGRAM");
			        TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
			        TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
			        PROGRAM_PATH = new stringParameter("@PROGRAM_PATH", base.inputParameterList);
			        PROGRAM_PARMS = new stringParameter("@PROGRAM_PARMS", base.inputParameterList);
			    }
			
			    public int Insert(DatabaseAccess _dba, 
			                      int? TASKLIST_RID,
			                      int? TASK_SEQUENCE,
			                      string PROGRAM_PATH,
			                      string PROGRAM_PARMS
			                      )
			    {
                    lock (typeof(MID_TASK_PROGRAM_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.PROGRAM_PATH.SetValue(PROGRAM_PATH);
                        this.PROGRAM_PARMS.SetValue(PROGRAM_PARMS);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
			    }
			}

            // Begin TT#1595-MD - stodd - Batch Comp
            public static MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_AND_SEQ_def MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_AND_SEQ = new MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_AND_SEQ_def();
            public class MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_AND_SEQ_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_AND_SEQ.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;

                public MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_AND_SEQ_def()
                {
                    base.procedureName = "MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_AND_SEQ";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("TASK_BATCH_COMP");
                    TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
                    TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba,
                                      int? TASKLIST_RID,
                                      int? TASK_SEQUENCE
                                      )
                {
                    lock (typeof(MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_AND_SEQ_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_def MID_TASK_BATCH_COMP_READ_FROM_TASKLIST = new MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_def();
            public class MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_BATCH_COMP_READ_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;

                public MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_def()
                {
                    base.procedureName = "MID_TASK_BATCH_COMP_READ_FROM_TASKLIST";
                    base.procedureType = storedProcedureTypes.Read;
                    base.tableNames.Add("TASK_BATCH_COMP");
                    TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
                }

                public DataTable Read(DatabaseAccess _dba, int? TASKLIST_RID)
                {
                    lock (typeof(MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForRead(_dba);
                    }
                }
            }

            public static MID_TASK_BATCH_COMP_DELETE_FROM_TASKLIST_def MID_TASK_BATCH_COMP_DELETE_FROM_TASKLIST = new MID_TASK_BATCH_COMP_DELETE_FROM_TASKLIST_def();
            public class MID_TASK_BATCH_COMP_DELETE_FROM_TASKLIST_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_BATCH_COMP_DELETE_FROM_TASKLIST.SQL"

                private intParameter TASKLIST_RID;

                public MID_TASK_BATCH_COMP_DELETE_FROM_TASKLIST_def()
                {
                    base.procedureName = "MID_TASK_BATCH_COMP_DELETE_FROM_TASKLIST";
                    base.procedureType = storedProcedureTypes.Delete;
                    base.tableNames.Add("TASK_BATCH_COMP");
                    TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
                }

                public int Delete(DatabaseAccess _dba, int? TASKLIST_RID)
                {
                    lock (typeof(MID_TASK_BATCH_COMP_DELETE_FROM_TASKLIST_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        return ExecuteStoredProcedureForDelete(_dba);
                    }
                }
            }

            public static MID_TASK_BATCH_COMP_INSERT_def MID_TASK_BATCH_COMP_INSERT = new MID_TASK_BATCH_COMP_INSERT_def();
            public class MID_TASK_BATCH_COMP_INSERT_def : baseStoredProcedure
            {
                //"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASK_BATCH_COMP_INSERT.SQL"

                private intParameter TASKLIST_RID;
                private intParameter TASK_SEQUENCE;
                private intParameter BATCH_COMP_RID;

                public MID_TASK_BATCH_COMP_INSERT_def()
                {
                    base.procedureName = "MID_TASK_BATCH_COMP_INSERT";
                    base.procedureType = storedProcedureTypes.Insert;
                    base.tableNames.Add("TASK_BATCH_COMP");
                    TASKLIST_RID = new intParameter("@TASKLIST_RID", base.inputParameterList);
                    TASK_SEQUENCE = new intParameter("@TASK_SEQUENCE", base.inputParameterList);
                    BATCH_COMP_RID = new intParameter("@BATCH_COMP_RID", base.inputParameterList);
                }

                public int Insert(DatabaseAccess _dba,
                                  int? TASKLIST_RID,
                                  int? TASK_SEQUENCE,
                                  int? BATCH_COMP_RID
                                  )
                {
                    lock (typeof(MID_TASK_BATCH_COMP_INSERT_def))
                    {
                        this.TASKLIST_RID.SetValue(TASKLIST_RID);
                        this.TASK_SEQUENCE.SetValue(TASK_SEQUENCE);
                        this.BATCH_COMP_RID.SetValue(BATCH_COMP_RID);
                        return ExecuteStoredProcedureForInsert(_dba);
                    }
                }
            }
            // End TT#1595-MD - stodd - batch Comp



			public static MID_SCHEDULE_READ_NEW_def MID_SCHEDULE_READ_NEW = new MID_SCHEDULE_READ_NEW_def();
			public class MID_SCHEDULE_READ_NEW_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_READ_NEW.SQL"

			
			    public MID_SCHEDULE_READ_NEW_def()
			    {
			        base.procedureName = "MID_SCHEDULE_READ_NEW";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SCHEDULE");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SCHEDULE_READ_NEW_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_JOB_READ_NEW_def MID_JOB_READ_NEW = new MID_JOB_READ_NEW_def();
			public class MID_JOB_READ_NEW_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_READ_NEW.SQL"

			
			    public MID_JOB_READ_NEW_def()
			    {
			        base.procedureName = "MID_JOB_READ_NEW";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("JOB");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_JOB_READ_NEW_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SPECIAL_REQUEST_JOB_READ_NEW_def MID_SPECIAL_REQUEST_JOB_READ_NEW = new MID_SPECIAL_REQUEST_JOB_READ_NEW_def();
			public class MID_SPECIAL_REQUEST_JOB_READ_NEW_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SPECIAL_REQUEST_JOB_READ_NEW.SQL"

			
			    public MID_SPECIAL_REQUEST_JOB_READ_NEW_def()
			    {
			        base.procedureName = "MID_SPECIAL_REQUEST_JOB_READ_NEW";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SPECIAL_REQUEST_JOB");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SPECIAL_REQUEST_JOB_READ_NEW_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_SCHEDULE_JOB_JOIN_READ_NEW_def MID_SCHEDULE_JOB_JOIN_READ_NEW = new MID_SCHEDULE_JOB_JOIN_READ_NEW_def();
			public class MID_SCHEDULE_JOB_JOIN_READ_NEW_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_SCHEDULE_JOB_JOIN_READ_NEW.SQL"

			
			    public MID_SCHEDULE_JOB_JOIN_READ_NEW_def()
			    {
			        base.procedureName = "MID_SCHEDULE_JOB_JOIN_READ_NEW";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("SCHEDULE_JOB_JOIN");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_SCHEDULE_JOB_JOIN_READ_NEW_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_TASKLIST_READ_NEW_def MID_TASKLIST_READ_NEW = new MID_TASKLIST_READ_NEW_def();
			public class MID_TASKLIST_READ_NEW_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_TASKLIST_READ_NEW.SQL"

			
			    public MID_TASKLIST_READ_NEW_def()
			    {
			        base.procedureName = "MID_TASKLIST_READ_NEW";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("TASKLIST");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_TASKLIST_READ_NEW_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			public static MID_JOB_TASKLIST_JOIN_READ_NEW_def MID_JOB_TASKLIST_JOIN_READ_NEW = new MID_JOB_TASKLIST_JOIN_READ_NEW_def();
			public class MID_JOB_TASKLIST_JOIN_READ_NEW_def : baseStoredProcedure
			{
				//"file:///C:\SCMVS2010\gohere.html?filepath=DatabaseDefinition\SQL_StoredProcedures\MID_JOB_TASKLIST_JOIN_READ_NEW.SQL"

			
			    public MID_JOB_TASKLIST_JOIN_READ_NEW_def()
			    {
			        base.procedureName = "MID_JOB_TASKLIST_JOIN_READ_NEW";
			        base.procedureType = storedProcedureTypes.Read;
			        base.tableNames.Add("JOB_TASKLIST_JOIN");
			    }
			
			    public DataTable Read(DatabaseAccess _dba)
			    {
                    lock (typeof(MID_JOB_TASKLIST_JOIN_READ_NEW_def))
                    {
                        return ExecuteStoredProcedureForRead(_dba);
                    }
			    }
			}

			//INSERT NEW STORED PROCEDURES ABOVE HERE
        }
    }  
}
