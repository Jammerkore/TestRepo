using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Data;
using System.Globalization;
using System.Text;

using MIDRetail.DataCommon;

namespace MIDRetail.Data
{
	public partial class ScheduleData : DataLayer
	{
		public ScheduleData() : base()
		{
        }

        public ScheduleData(DatabaseAccess dba)
            : base(dba)
        {
        }

		public DataTable ReadScheduledJobs()
		{
			try
			{
                return StoredProcedures.MID_SCHEDULE_READ_ALL_JOBS.Read(_dba);
			}
			catch
			{
				throw;
			}
		}

        public DataTable ReadScheduledJobsForJobManager(string scheduleName, string jobName, int userRid)
        {
            try
            {
                return StoredProcedures.MID_SCHEDULE_READ_JOBS_FOR_JOB_MANAGER.Read(_dba, scheduleName, jobName, userRid);
            }
            catch
            {
                throw;
            }
        }

		public DataRow ReadScheduledJob(int aScheduleRID, int aJobRID)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_SCHEDULE_READ_JOB.Read(_dba,
                                                                   SCHED_RID: aScheduleRID,
                                                                   JOB_RID: aJobRID
                                                                   );

				if (dt.Rows.Count == 1)
				{
					return dt.Rows[0];
				}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}

		public DataTable ReadScheduledJob(int aJobRID)
		{
			try
			{
                return StoredProcedures.MID_SCHEDULE_READ_JOB_FROM_JOB_RID.Read(_dba, JOB_RID: aJobRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable ReadActiveJob(int aJobRID)
		{
			try
			{
                return StoredProcedures.MID_SCHEDULE_READ_ACTIVE_JOBS.Read(_dba, JOB_RID: aJobRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable ReadCompletedJobsOlderThanDate(DateTime aDateTime)
		{
			try
			{
                return StoredProcedures.MID_SCHEDULE_READ_JOBS_OLDER_THAN_DATE.Read(_dba, LAST_COMPLETION_DATETIME: aDateTime);
			}
			catch
			{
				throw;
			}
		}

		public DataTable ReadOrphanedSystemJobs()
		{
			try
			{
                return StoredProcedures.MID_JOB_READ_ALL_ORPHANED_SYSTEM_JOBS.Read(_dba);
			}
			catch
			{
				throw;
			}
		}

		public DataTable ReadOrphanedSystemTaskLists()
		{
			try
			{
                return StoredProcedures.MID_JOB_READ_ALL_ORPHANED_SYSTEM_TASKLISTS.Read(_dba);
			}
			catch
			{
				throw;
			}
		}

		public DataTable ReadActiveJobsByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_SCHEDULE_READ_ACTIVE_JOBS_BY_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable ReadScheduledSystemJobsByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_SCHEDULE_READ_SYSTEM_JOBS_BY_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable ReadNonSystemJobsByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_SCHEDULE_READ_NON_SYSTEM_JOBS_BY_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable ReadSystemJobsByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_JOB_READ_SYSTEM_JOBS_BY_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable ReadSpecialRequestsByJob(int aJobRID)
		{
			try
			{
                return StoredProcedures.MID_SPECIAL_REQUEST_JOB_READ.Read(_dba, JOB_RID: aJobRID);
			}
			catch
			{
				throw;
			}
		}

        //Begin Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"
		public int GetNextScheduleProcessId()
		{
			try
			{
                return StoredProcedures.SP_MID_SCHEDULE_PROCESS_INSERT.InsertAndReturnRID(_dba);
			}
			catch
			{
				throw;
			}
		}
        //End Track #6141 - JScott - Defect ID = 1958  -  Forecast errors from "in use" message, user is "global"


		public DataRow Schedule_Read(int aScheduleRID)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_SCHEDULE_READ.Read(_dba, SCHED_RID: aScheduleRID);

				if (dt.Rows.Count == 1)
				{
					return dt.Rows[0];
				}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}

       

      

		public int Schedule_GetKey(string aScheduleName)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_SCHEDULE_READ_KEY.Read(_dba, SCHED_NAME: aScheduleName);

				if (dt.Rows.Count == 1)
				{
					return (Convert.ToInt32(dt.Rows[0]["SCHED_RID"], CultureInfo.CurrentUICulture));
				}
				else
				{
					return -1;
				}
			}
			catch
			{
				throw;
			}
		}

		public int Schedule_Insert(ScheduleProfile aSchedProfile, int aUserRID)
		{
			try
			{
				return Schedule_Insert(aSchedProfile.UnloadToDataRow(NewScheduleRow()), aUserRID);
			}
			catch
			{
				throw;
			}
		}

		public int Schedule_Insert(DataRow aRow, int aUserRID)
		{
			try
			{
                return StoredProcedures.SP_MID_SCHEDULE_INSERT.InsertAndReturnRID(_dba,
                                                                        SCHED_NAME: Convert.ToString(aRow["SCHED_NAME"], CultureInfo.CurrentUICulture),
                                                                        START_TIME: Convert.ToDateTime(aRow["START_TIME"], CultureInfo.CurrentUICulture),
                                                                        SCHEDULE_BY_TYPE: Convert.ToInt32(aRow["SCHEDULE_BY_TYPE"], CultureInfo.CurrentUICulture),
                                                                        SCHEDULE_BY_INTERVAL: Convert.ToInt32(aRow["SCHEDULE_BY_INTERVAL"], CultureInfo.CurrentUICulture),
                                                                        SCHEDULE_ON_MONDAY_IND: Convert.ToChar(aRow["SCHEDULE_ON_MONDAY_IND"], CultureInfo.CurrentUICulture),
                                                                        SCHEDULE_ON_TUESDAY_IND: Convert.ToChar(aRow["SCHEDULE_ON_TUESDAY_IND"], CultureInfo.CurrentUICulture),
                                                                        SCHEDULE_ON_WEDNESDAY_IND: Convert.ToChar(aRow["SCHEDULE_ON_WEDNESDAY_IND"], CultureInfo.CurrentUICulture),
                                                                        SCHEDULE_ON_THURSDAY_IND: Convert.ToChar(aRow["SCHEDULE_ON_THURSDAY_IND"], CultureInfo.CurrentUICulture),
                                                                        SCHEDULE_ON_FRIDAY_IND: Convert.ToChar(aRow["SCHEDULE_ON_FRIDAY_IND"], CultureInfo.CurrentUICulture),
                                                                        SCHEDULE_ON_SATURDAY_IND: Convert.ToChar(aRow["SCHEDULE_ON_SATURDAY_IND"], CultureInfo.CurrentUICulture),
                                                                        SCHEDULE_ON_SUNDAY_IND: Convert.ToChar(aRow["SCHEDULE_ON_SUNDAY_IND"], CultureInfo.CurrentUICulture),
                                                                        SCHEDULE_BY_MONTH_WEEK_TYPE: Convert.ToInt32(aRow["SCHEDULE_BY_MONTH_WEEK_TYPE"], CultureInfo.CurrentUICulture),
                                                                        START_DATE_RANGE: Convert.ToDateTime(aRow["START_DATE_RANGE"], CultureInfo.CurrentUICulture),
                                                                        END_DATE_IND: Convert.ToChar(aRow["END_DATE_IND"], CultureInfo.CurrentUICulture),
                                                                        END_DATE_RANGE: Convert.ToDateTime(aRow["END_DATE_RANGE"], CultureInfo.CurrentUICulture),
                                                                        REPEAT_INTERVAL: Convert.ToInt32(aRow["REPEAT_INTERVAL"], CultureInfo.CurrentUICulture),
                                                                        REPEAT_INTERVAL_TYPE: Convert.ToInt32(aRow["REPEAT_INTERVAL_TYPE"], CultureInfo.CurrentUICulture),
                                                                        REPEAT_UNTIL_IND: Convert.ToChar(aRow["REPEAT_UNTIL_IND"], CultureInfo.CurrentUICulture),
                                                                        REPEAT_UNTIL_TIME: Convert.ToDateTime(aRow["REPEAT_UNTIL_TIME"], CultureInfo.CurrentUICulture),
                                                                        REPEAT_DURATION_IND: Convert.ToChar(aRow["REPEAT_DURATION_IND"], CultureInfo.CurrentUICulture),
                                                                        REPEAT_DURATION_HOURS: Convert.ToInt32(aRow["REPEAT_DURATION_HOURS"], CultureInfo.CurrentUICulture),
                                                                        REPEAT_DURATION_MINUTES: Convert.ToInt32(aRow["REPEAT_DURATION_MINUTES"], CultureInfo.CurrentUICulture),
                                                                        CONDITION_TYPE: Convert.ToInt32(aRow["CONDITION_TYPE"], CultureInfo.CurrentUICulture),
                                                                        CONDITION_TRIGGER_DIRECTORY: Convert.ToString(aRow["CONDITION_TRIGGER_DIRECTORY"], CultureInfo.CurrentUICulture),
                                                                        CONDITION_TRIGGER_SUFFIX: Convert.ToString(aRow["CONDITION_TRIGGER_SUFFIX"], CultureInfo.CurrentUICulture),
                                                                        TERMINATE_AFTER_COND_MET_IND: Convert.ToChar(aRow["TERMINATE_AFTER_COND_MET_IND"], CultureInfo.CurrentUICulture),
                                                                        REPEAT_UNTIL_SUCCESSFUL_IND: Convert.ToChar(aRow["REPEAT_UNTIL_SUCCESSFUL_IND"], CultureInfo.CurrentUICulture),
                                                                        CREATED_BY_USER_RID: aUserRID,
                                                                        CREATED_DATETIME: DateTime.Now,
                                                                        LAST_MODIFIED_BY_USER_RID: aUserRID,
                                                                        LAST_MODIFIED_DATETIME: DateTime.Now
                                                                        );
			}
			catch
			{
				throw;
			}
		}

		public void Schedule_Update(ScheduleProfile aSchedProfile, int aUserRID)
		{
			try
			{
				Schedule_Update(aSchedProfile.UnloadToDataRow(NewScheduleRow()), aUserRID);
			}
			catch
			{
				throw;
			}
		}

		public void Schedule_Update(DataTable aSchedTable, int aUserRID)
		{
			try
			{
				foreach (DataRow row in aSchedTable.Rows)
				{
					Schedule_Update(row, aUserRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void Schedule_Update(DataRow aSchedDataRow, int aUserRID)
		{
			try
			{
                StoredProcedures.MID_SCHEDULE_UPDATE.Update(_dba,
                                                            SCHED_NAME: Convert.ToString(aSchedDataRow["SCHED_NAME"], CultureInfo.CurrentUICulture),
                                                            START_TIME: Convert.ToDateTime(aSchedDataRow["START_TIME"], CultureInfo.CurrentUICulture),
                                                            SCHEDULE_BY_TYPE: Convert.ToInt32(aSchedDataRow["SCHEDULE_BY_TYPE"], CultureInfo.CurrentUICulture),
                                                            SCHEDULE_BY_INTERVAL: Convert.ToInt32(aSchedDataRow["SCHEDULE_BY_INTERVAL"], CultureInfo.CurrentUICulture),
                                                            SCHEDULE_ON_MONDAY_IND: Convert.ToChar(aSchedDataRow["SCHEDULE_ON_MONDAY_IND"], CultureInfo.CurrentUICulture),
                                                            SCHEDULE_ON_TUESDAY_IND: Convert.ToChar(aSchedDataRow["SCHEDULE_ON_TUESDAY_IND"], CultureInfo.CurrentUICulture),
                                                            SCHEDULE_ON_WEDNESDAY_IND: Convert.ToChar(aSchedDataRow["SCHEDULE_ON_WEDNESDAY_IND"], CultureInfo.CurrentUICulture),
                                                            SCHEDULE_ON_THURSDAY_IND: Convert.ToChar(aSchedDataRow["SCHEDULE_ON_THURSDAY_IND"], CultureInfo.CurrentUICulture),
                                                            SCHEDULE_ON_FRIDAY_IND: Convert.ToChar(aSchedDataRow["SCHEDULE_ON_FRIDAY_IND"], CultureInfo.CurrentUICulture),
                                                            SCHEDULE_ON_SATURDAY_IND: Convert.ToChar(aSchedDataRow["SCHEDULE_ON_SATURDAY_IND"], CultureInfo.CurrentUICulture),
                                                            SCHEDULE_ON_SUNDAY_IND: Convert.ToChar(aSchedDataRow["SCHEDULE_ON_SUNDAY_IND"], CultureInfo.CurrentUICulture),
                                                            SCHEDULE_BY_MONTH_WEEK_TYPE: Convert.ToInt32(aSchedDataRow["SCHEDULE_BY_MONTH_WEEK_TYPE"], CultureInfo.CurrentUICulture),
                                                            START_DATE_RANGE: Convert.ToDateTime(aSchedDataRow["START_DATE_RANGE"], CultureInfo.CurrentUICulture),
                                                            END_DATE_IND: Convert.ToChar(aSchedDataRow["END_DATE_IND"], CultureInfo.CurrentUICulture),
                                                            END_DATE_RANGE: Convert.ToDateTime(aSchedDataRow["END_DATE_RANGE"], CultureInfo.CurrentUICulture),
                                                            REPEAT_INTERVAL: Convert.ToInt32(aSchedDataRow["REPEAT_INTERVAL"], CultureInfo.CurrentUICulture),
                                                            REPEAT_INTERVAL_TYPE: Convert.ToInt32(aSchedDataRow["REPEAT_INTERVAL_TYPE"], CultureInfo.CurrentUICulture),
                                                            REPEAT_UNTIL_IND: Convert.ToChar(aSchedDataRow["REPEAT_UNTIL_IND"], CultureInfo.CurrentUICulture),
                                                            REPEAT_UNTIL_TIME: Convert.ToDateTime(aSchedDataRow["REPEAT_UNTIL_TIME"], CultureInfo.CurrentUICulture),
                                                            REPEAT_DURATION_IND: Convert.ToChar(aSchedDataRow["REPEAT_DURATION_IND"], CultureInfo.CurrentUICulture),
                                                            REPEAT_DURATION_HOURS: Convert.ToInt32(aSchedDataRow["REPEAT_DURATION_HOURS"], CultureInfo.CurrentUICulture),
                                                            REPEAT_DURATION_MINUTES: Convert.ToInt32(aSchedDataRow["REPEAT_DURATION_MINUTES"], CultureInfo.CurrentUICulture),
                                                            CONDITION_TYPE: Convert.ToInt32(aSchedDataRow["CONDITION_TYPE"], CultureInfo.CurrentUICulture),
                                                            CONDITION_TRIGGER_DIRECTORY: Convert.ToString(aSchedDataRow["CONDITION_TRIGGER_DIRECTORY"], CultureInfo.CurrentUICulture),
                                                            CONDITION_TRIGGER_SUFFIX: Convert.ToString(aSchedDataRow["CONDITION_TRIGGER_SUFFIX"], CultureInfo.CurrentUICulture),
                                                            TERMINATE_AFTER_COND_MET_IND: Convert.ToChar(aSchedDataRow["TERMINATE_AFTER_COND_MET_IND"], CultureInfo.CurrentUICulture),
                                                            REPEAT_UNTIL_SUCCESSFUL_IND: Convert.ToChar(aSchedDataRow["REPEAT_UNTIL_SUCCESSFUL_IND"], CultureInfo.CurrentUICulture),
                                                            LAST_MODIFIED_BY_USER_RID: aUserRID,
                                                            LAST_MODIFIED_DATETIME: DateTime.Now,
                                                            SCHED_RID: Convert.ToInt32(aSchedDataRow["SCHED_RID"], CultureInfo.CurrentUICulture)
                                                            );
			}
			catch
			{
				throw;
			}
		}

		public void Schedule_DeleteFromList(DataTable aTable)
		{
			try
			{
				foreach (DataRow row in aTable.Rows)
				{
					Schedule_Delete(Convert.ToInt32(row["SCHED_RID"]));
				}
			}
			catch
			{
				throw;
			}
		}

		public void Schedule_Delete(int aSchedRID)
		{
			try
			{
                StoredProcedures.MID_SCHEDULE_DELETE.Delete(_dba, SCHED_RID: aSchedRID);
			}
			catch
			{
				throw;
			}
		}

		public DataRow Job_Read(int aJobRID)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_JOB_READ.Read(_dba, JOB_RID: aJobRID);

				if (dt.Rows.Count == 1)
				{
					return dt.Rows[0];
				}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}



		public DataTable Job_ReadNonSystemParent()
		{
			try
			{
                return StoredProcedures.MID_JOB_READ_NON_SYSTEM_PARENT.Read(_dba, CHILD_ITEM_TYPE: (int)eProfileType.Job);
			}
			catch
			{
				throw;
			}
		}
		
		public int Job_GetKey(string aJobName)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_JOB_READ_FROM_NAME.Read(_dba, JOB_NAME: aJobName);

				if (dt.Rows.Count == 1)
				{
					return (Convert.ToInt32(dt.Rows[0]["JOB_RID"], CultureInfo.CurrentUICulture));
				}
				else
				{
					return -1;
				}
			}
			catch
			{
				throw;
			}
		}

		public DataTable Job_ReadByUser(int aUserRID)
		{
			try
			{
                return StoredProcedures.MID_JOB_READ_FROM_USER.Read(_dba, USER_RID: aUserRID);
			}
			catch
			{
				throw;
			}
		}

		public int Job_Insert(JobProfile aJobProfile, int aUserRID)
		{
			try
			{
				return Job_Insert(aJobProfile.UnloadToDataRow(NewJobRow()), aUserRID);
			}
			catch
			{
				throw;
			}
		}

		public int Job_Insert(DataRow aRow, int aUserRID)
		{
			try
			{
                return StoredProcedures.SP_MID_JOB_INSERT.InsertAndReturnRID(_dba,
                                                                             JOB_NAME: Convert.ToString(aRow["JOB_NAME"], CultureInfo.CurrentUICulture),
                                                                             SYSTEM_GENERATED_IND: Convert.ToChar(aRow["SYSTEM_GENERATED_IND"], CultureInfo.CurrentUICulture),
                                                                             CREATED_BY_USER_RID: aUserRID,
                                                                             CREATED_DATETIME: DateTime.Now,
                                                                             LAST_MODIFIED_BY_USER_RID: aUserRID,
                                                                             LAST_MODIFIED_DATETIME: DateTime.Now
                                                                             );
			}
			catch
			{
				throw;
			}
		}

		public void Job_UpdateName(int aJobRID, string aName, int aUserRID)
		{
			try
			{
                StoredProcedures.MID_JOB_UPDATE.Update(_dba,
                                                       JOB_RID: aJobRID,
                                                       JOB_NAME: aName,
                                                       LAST_MODIFIED_BY_USER_RID: aUserRID,
                                                       LAST_MODIFIED_DATETIME: DateTime.Now
                                                       );
			}
			catch
			{
				throw;
			}
		}

		public void Job_Update(JobProfile aJobProfile, int aUserRID)
		{
			try
			{
				Job_Update(aJobProfile.UnloadToDataRow(NewJobRow()), aUserRID);
			}
			catch
			{
				throw;
			}
		}

		public void Job_Update(DataTable aJobTable, int aUserRID)
		{
			try
			{
				foreach (DataRow row in aJobTable.Rows)
				{
					Job_Update(row, aUserRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void Job_Update(DataRow aJobRow, int aUserRID)
		{
			try
			{
                StoredProcedures.MID_JOB_UPDATE.Update(_dba,
                                                       JOB_RID: (int)aJobRow["JOB_RID"],
                                                       JOB_NAME: Convert.ToString(aJobRow["JOB_NAME"], CultureInfo.CurrentUICulture),
                                                       LAST_MODIFIED_BY_USER_RID: aUserRID,
                                                       LAST_MODIFIED_DATETIME: DateTime.Now
                                                       );
			}
			catch
			{
				throw;
			}
		}

		public void Job_Delete(int aJobRID)
		{
			try
			{
                StoredProcedures.MID_JOB_DELETE.Delete(_dba, JOB_RID: aJobRID);
			}
			catch
			{
				throw;
			}
		}

		public void Job_DeleteSystemFromList(DataTable aTable)
		{
			try
			{
				foreach (DataRow row in aTable.Rows)
				{
                    StoredProcedures.MID_JOB_DELETE_SYSTEM_JOBS.Delete(_dba, JOB_RID: (int)row["JOB_RID"]);
				}
			}
			catch
			{
				throw;
			}
		}
		
		
		//===================================
		// Special Request Job
		//===================================

		public DataTable SpecialRequest_ReadParent()
		{
			try
			{
                return StoredProcedures.MID_SPECIAL_REQUEST_JOB_READ_SPECIAL_REQUESTS.Read(_dba, CHILD_ITEM_TYPE: (int)eProfileType.SpecialRequest);
			}
			catch
			{
				throw;
			}
		}


		/// <summary>
		/// Reads only the SPECIAL_REQUEST_JOB Record.
		/// </summary>
		/// <param name="aSpecialReqJobRID"></param>
		/// <returns></returns>
		public DataRow SpecialRequest_Read(int aSpecialReqJobRID)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_SPECIAL_REQUEST_JOB_READ_FROM_RID.Read(_dba, SPECIAL_REQ_RID: aSpecialReqJobRID);

				if (dt.Rows.Count == 1)
				{
					return dt.Rows[0];
				}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}

		/// <summary>
		/// Reads the SPECIAL_REQUEST_JOB table joined with the JOB table.
		/// </summary>
		/// <param name="aSpecialRequestRID"></param>
		/// <returns></returns>
		public DataTable SpecialRequest_ReadByJob(int aSpecialRequestRID)
		{
			try
			{
                return StoredProcedures.MID_SPECIAL_REQUEST_JOB_JOIN_READ.Read(_dba, SPECIAL_REQ_RID: aSpecialRequestRID);
			}
			catch
			{
				throw;
			}
		}

		public int SpecialRequest_GetKey(string aSpecialRequestJobName)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_SPECIAL_REQUEST_JOB_READ_FROM_NAME.Read(_dba, SPECIAL_REQ_NAME: aSpecialRequestJobName);

				if (dt.Rows.Count == 1)
				{
					return (Convert.ToInt32(dt.Rows[0]["SPECIAL_REQ_RID"], CultureInfo.CurrentUICulture));
				}
				else
				{
					return -1;
				}
			}
			catch
			{
				throw;
			}
		}

		public int SpecialRequest_Insert( SpecialRequestProfile aSpecialReqJobProfile)
		{
			try
			{
				return SpecialRequest_Insert(aSpecialReqJobProfile.UnloadToDataRow(NewSpecialRequestRow()));
			}
			catch
			{
				throw;
			}
		}

		public int SpecialRequest_Insert(DataRow aRow)
		{
			try
			{
                return StoredProcedures.SP_MID_SPECIAL_REQ_JOB_INSERT.InsertAndReturnRID(_dba,
                                                                                      SPECIAL_REQ_NAME: Convert.ToString(aRow["SPECIAL_REQ_NAME"], CultureInfo.CurrentUICulture),
                                                                                      CONCURRENT_PROCESSES: Convert.ToInt32(aRow["CONCURRENT_PROCESSES"], CultureInfo.CurrentUICulture)
                                                                                      );
			}
			catch
			{
				throw;
			}
		}

		public void SpecialRequest_UpdateName(int aSpecialRequestRID, string aName)
		{
			try
			{
                StoredProcedures.MID_SPECIAL_REQUEST_JOB_UPDATE_NAME.Update(_dba,
                                                                           SPECIAL_REQ_RID: aSpecialRequestRID,
                                                                           SPECIAL_REQ_NAME: aName
                                                                           );
			}
			catch
			{
				throw;
			}
		}

		public void SpecialRequest_Update(SpecialRequestProfile aJobProfile)
		{
			try
			{
				SpecialRequest_Update(aJobProfile.UnloadToDataRow(NewSpecialRequestRow()));
			}
			catch
			{
				throw;
			}
		}

		public void SpecialRequest_Update(DataTable aJobTable)
		{
			try
			{
				foreach (DataRow row in aJobTable.Rows)
				{
					SpecialRequest_Update(row);
				}
			}
			catch
			{
				throw;
			}
		}

		public void SpecialRequest_Update(DataRow aSpecialRequestRow)
		{
			try
			{
                StoredProcedures.MID_SPECIAL_REQUEST_JOB_UPDATE.Update(_dba,
                                                                       SPECIAL_REQ_RID: (int)aSpecialRequestRow["SPECIAL_REQ_RID"],
                                                                       SPECIAL_REQ_NAME: Convert.ToString(aSpecialRequestRow["SPECIAL_REQ_NAME"], CultureInfo.CurrentUICulture),
                                                                       CONCURRENT_PROCESSES: Convert.ToInt32(aSpecialRequestRow["CONCURRENT_PROCESSES"], CultureInfo.CurrentUICulture)
                                                                       );
			}
			catch
			{
				throw;
			}
		}

		public void SpecialRequest_Delete(int aSpecialReqJobRID)
		{
			try
			{
                StoredProcedures.MID_SPECIAL_REQUEST_JOB_DELETE.Delete(_dba, SPECIAL_REQ_RID: aSpecialReqJobRID);
			}
			catch
			{
				throw;
			}
		}

		//============================
		// SPECIAL_REQUEST_JOB_JOIN
		//============================

		
		public DataTable SpecialRequestJoin_ReadBySpecialRequest(int aSpecialRequestRID)
		{
			try
			{
                return StoredProcedures.MID_SPECIAL_REQUEST_JOB_JOIN_READ_FROM_REQUEST_RID.Read(_dba, SPECIAL_REQ_RID: aSpecialRequestRID);
			}
			catch
			{
				throw;
			}
		}

		public void SpecialRequestJoin_DeleteBySpecialRequest(int aSpecialReqJobRID)
		{
			try
			{
                StoredProcedures.MID_SPECIAL_REQUEST_JOB_JOIN_DELETE_FROM_REQUEST_RID.Delete(_dba, SPECIAL_REQ_RID: aSpecialReqJobRID);
			}
			catch
			{
				throw;
			}
		}

		public void SpecialRequestJoin_DeleteByJob(int aSpecialReqJobRID, int aJobRID)
		{
			try
			{
                StoredProcedures.MID_SPECIAL_REQUEST_JOB_JOIN_DELETE.Delete(_dba,
                                                                            SPECIAL_REQ_RID: aSpecialReqJobRID,
                                                                            JOB_RID: aJobRID
                                                                            );
			}
			catch
			{
				throw;
			}
		}

		public void SpecialRequestJoin_Insert(DataTable aSpecialReqJobTable)
		{
			try
			{
				foreach (DataRow row in aSpecialReqJobTable.Rows)
				{
					SpecialRequestJoin_Insert(row, Convert.ToInt32(row["SPECIAL_REQ_RID"], CultureInfo.CurrentUICulture));
				}
			}
			catch
			{
				throw;
			}
		}

		public void SpecialRequestJoin_Insert(DataTable aSpecialReqJobTable, int aSpecialRequestRID)
		{
			try
			{
				foreach (DataRow row in aSpecialReqJobTable.Rows)
				{
					SpecialRequestJoin_Insert(row, aSpecialRequestRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void SpecialRequestJoin_Insert(DataRow aRow, int aSpecialRequestRID)
		{
			try
			{
                StoredProcedures.MID_SPECIAL_REQUEST_JOB_JOIN_INSERT.Insert(_dba,
                                                                            SPECIAL_REQ_RID: aSpecialRequestRID,
                                                                            JOB_RID: Convert.ToInt32(aRow["JOB_RID"], CultureInfo.CurrentUICulture),
                                                                            JOB_SEQUENCE: Convert.ToInt32(aRow["JOB_SEQUENCE"], CultureInfo.CurrentUICulture)
                                                                            );
			}
			catch
			{
				throw;
			}
		}	

		public void ScheduleJobJoin_Insert(ScheduleJobJoinProfile aScheduleJobJoinProfile)
		{
			try
			{
				ScheduleJobJoin_Insert(aScheduleJobJoinProfile.UnloadToDataRow(NewScheduleJobJoinRow()));
			}
			catch
			{
				throw;
			}
		}

		public void ScheduleJobJoin_Insert(DataTable aSpecialReqJobTable)
		{
			try
			{
				foreach (DataRow row in aSpecialReqJobTable.Rows)
				{
					ScheduleJobJoin_Insert(row);
				}
			}
			catch
			{
				throw;
			}
		}

		public void ScheduleJobJoin_Insert(DataRow aScheduleJobJoinRow)
		{
			try
			{
                DateTime? LAST_RUN_DATETIME_Nullable = System.DateTime.MinValue;
                if (aScheduleJobJoinRow["LAST_RUN_DATETIME"] != System.DBNull.Value) LAST_RUN_DATETIME_Nullable = Convert.ToDateTime(aScheduleJobJoinRow["LAST_RUN_DATETIME"], CultureInfo.CurrentUICulture);

                DateTime? LAST_COMPLETION_DATETIME_Nullable = System.DateTime.MinValue;
                if (aScheduleJobJoinRow["LAST_COMPLETION_DATETIME"] != System.DBNull.Value) LAST_COMPLETION_DATETIME_Nullable = Convert.ToDateTime(aScheduleJobJoinRow["LAST_COMPLETION_DATETIME"], CultureInfo.CurrentUICulture);

                DateTime? NEXT_RUN_DATETIME_Nullable = System.DateTime.MinValue;
                if (aScheduleJobJoinRow["NEXT_RUN_DATETIME"] != System.DBNull.Value) NEXT_RUN_DATETIME_Nullable = Convert.ToDateTime(aScheduleJobJoinRow["NEXT_RUN_DATETIME"], CultureInfo.CurrentUICulture);

                DateTime? REPEAT_UNTIL_DATETIME_Nullable = System.DateTime.MinValue;
                if (aScheduleJobJoinRow["REPEAT_UNTIL_DATETIME"] != System.DBNull.Value) REPEAT_UNTIL_DATETIME_Nullable = Convert.ToDateTime(aScheduleJobJoinRow["REPEAT_UNTIL_DATETIME"], CultureInfo.CurrentUICulture);

                StoredProcedures.MID_SCHEDULE_JOB_JOIN_INSERT.Insert(_dba,
                                                                     SCHED_RID: Convert.ToInt32(aScheduleJobJoinRow["SCHED_RID"], CultureInfo.CurrentUICulture),
                                                                     JOB_RID: Convert.ToInt32(aScheduleJobJoinRow["JOB_RID"], CultureInfo.CurrentUICulture),
                                                                     USER_RID: Convert.ToInt32(aScheduleJobJoinRow["USER_RID"], CultureInfo.CurrentUICulture),
                                                                     EXECUTION_STATUS: Convert.ToInt32(aScheduleJobJoinRow["EXECUTION_STATUS"], CultureInfo.CurrentUICulture),
                                                                     LAST_COMPLETION_STATUS: Convert.ToInt32(aScheduleJobJoinRow["LAST_COMPLETION_STATUS"], CultureInfo.CurrentUICulture),
                                                                     LAST_RUN_DATETIME: LAST_RUN_DATETIME_Nullable,
                                                                     LAST_COMPLETION_DATETIME: LAST_COMPLETION_DATETIME_Nullable,
                                                                     NEXT_RUN_DATETIME: NEXT_RUN_DATETIME_Nullable,
                                                                     REPEAT_UNTIL_DATETIME: REPEAT_UNTIL_DATETIME_Nullable
                                                                     );
			}
			catch
			{
				throw;
			}
		}

		public void ScheduleJobJoin_Update(DataTable aScheduleJobJoinTable)
		{
			try
			{
				foreach (DataRow row in aScheduleJobJoinTable.Rows)
				{
					ScheduleJobJoin_Update(row);
				}
			}
			catch
			{
				throw;
			}
		}

		public void ScheduleJobJoin_Update(DataRow aScheduleJobJoinRow)
		{
			try
			{
                DateTime? LAST_RUN_DATETIME_Nullable = System.DateTime.MinValue;
                if (aScheduleJobJoinRow["LAST_RUN_DATETIME"] != System.DBNull.Value) LAST_RUN_DATETIME_Nullable = Convert.ToDateTime(aScheduleJobJoinRow["LAST_RUN_DATETIME"], CultureInfo.CurrentUICulture);

                DateTime? LAST_COMPLETION_DATETIME_Nullable = System.DateTime.MinValue;
                if (aScheduleJobJoinRow["LAST_COMPLETION_DATETIME"] != System.DBNull.Value) LAST_COMPLETION_DATETIME_Nullable = Convert.ToDateTime(aScheduleJobJoinRow["LAST_COMPLETION_DATETIME"], CultureInfo.CurrentUICulture);

                DateTime? NEXT_RUN_DATETIME_Nullable = System.DateTime.MinValue;
                if (aScheduleJobJoinRow["NEXT_RUN_DATETIME"] != System.DBNull.Value) NEXT_RUN_DATETIME_Nullable = Convert.ToDateTime(aScheduleJobJoinRow["NEXT_RUN_DATETIME"], CultureInfo.CurrentUICulture);

                DateTime? REPEAT_UNTIL_DATETIME_Nullable = System.DateTime.MinValue;
                if (aScheduleJobJoinRow["REPEAT_UNTIL_DATETIME"] != System.DBNull.Value) REPEAT_UNTIL_DATETIME_Nullable = Convert.ToDateTime(aScheduleJobJoinRow["REPEAT_UNTIL_DATETIME"], CultureInfo.CurrentUICulture);

                int? HOLD_BY_USER_RID_Nullable = null;
                if (aScheduleJobJoinRow["HOLD_BY_USER_RID"] != System.DBNull.Value) HOLD_BY_USER_RID_Nullable = Convert.ToInt32(aScheduleJobJoinRow["HOLD_BY_USER_RID"]);

                DateTime? HOLD_BY_DATETIME_Nullable = System.DateTime.MinValue;
                if (aScheduleJobJoinRow["HOLD_BY_DATETIME"] != System.DBNull.Value) HOLD_BY_DATETIME_Nullable = Convert.ToDateTime(aScheduleJobJoinRow["HOLD_BY_DATETIME"]);

                int? RELEASED_BY_USER_RID = null;
                if (aScheduleJobJoinRow["RELEASED_BY_USER_RID"] != System.DBNull.Value) RELEASED_BY_USER_RID = Convert.ToInt32(aScheduleJobJoinRow["RELEASED_BY_USER_RID"]);

                DateTime? RELEASED_BY_DATETIME_Nullable = System.DateTime.MinValue;
                if (aScheduleJobJoinRow["RELEASED_BY_DATETIME"] != System.DBNull.Value) RELEASED_BY_DATETIME_Nullable = Convert.ToDateTime(aScheduleJobJoinRow["RELEASED_BY_DATETIME"]);


                StoredProcedures.MID_SCHEDULE_JOB_JOIN_UPDATE.Update(_dba,
                                                                     SCHED_RID: Convert.ToInt32(aScheduleJobJoinRow["SCHED_RID"], CultureInfo.CurrentUICulture),
                                                                     JOB_RID: Convert.ToInt32(aScheduleJobJoinRow["JOB_RID"], CultureInfo.CurrentUICulture),
                                                                     USER_RID: Convert.ToInt32(aScheduleJobJoinRow["USER_RID"], CultureInfo.CurrentUICulture),
                                                                     EXECUTION_STATUS: Convert.ToInt32(aScheduleJobJoinRow["EXECUTION_STATUS"], CultureInfo.CurrentUICulture),
                                                                     LAST_COMPLETION_STATUS: Convert.ToInt32(aScheduleJobJoinRow["LAST_COMPLETION_STATUS"], CultureInfo.CurrentUICulture),
                                                                     LAST_RUN_DATETIME: LAST_RUN_DATETIME_Nullable,
                                                                     LAST_COMPLETION_DATETIME: LAST_COMPLETION_DATETIME_Nullable,
                                                                     NEXT_RUN_DATETIME: NEXT_RUN_DATETIME_Nullable,
                                                                     REPEAT_UNTIL_DATETIME: REPEAT_UNTIL_DATETIME_Nullable,
                                                                     HOLD_BY_USER_RID: HOLD_BY_USER_RID_Nullable,
                                                                     HOLD_BY_DATETIME: HOLD_BY_DATETIME_Nullable,
                                                                     RELEASED_BY_USER_RID: RELEASED_BY_USER_RID,
                                                                     RELEASED_BY_DATETIME: RELEASED_BY_DATETIME_Nullable
                                                                     );
			}
			catch
			{
				throw;
			}
		}

		public void ScheduleJobJoin_DeleteFromList(DataTable aTable)
		{
			try
			{
				foreach (DataRow row in aTable.Rows)
				{
					ScheduleJobJoin_Delete(Convert.ToInt32(row["SCHED_RID"]), Convert.ToInt32(row["JOB_RID"]));
				}
			}
			catch
			{
				throw;
			}
		}

		public void ScheduleJobJoin_Delete(int aSchedRID, int aJobRID)
		{
			try
			{
                StoredProcedures.MID_SCHEDULE_JOB_JOIN_DELETE.Delete(_dba,
                                                                     SCHED_RID: aSchedRID,
                                                                     JOB_RID: aJobRID
                                                                     );
			}
			catch
			{
				throw;
			}
		}


		public int TaskList_GetKey(string aTaskListName, int aUserRID)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_TASKLIST_READ_KEY.Read(_dba,
                                                                 TASKLIST_NAME: aTaskListName,
                                                                 USER_RID: aUserRID
                                                                 );

				if (dt.Rows.Count == 1)
				{
					return (Convert.ToInt32(dt.Rows[0]["TASKLIST_RID"], CultureInfo.CurrentUICulture));
				}
				else
				{
					return -1;
				}
			}
			catch
			{
				throw;
			}
		}


		public DataTable TaskList_Read(ArrayList aUserRIDList, bool aIncludeOwned, bool aIncludeAssigned)
		{
			try
			{
                if (aIncludeAssigned && aIncludeOwned)
                {
                    return StoredProcedures.MID_TASKLIST_READ_FOR_ASSIGNED_AND_OWNED.Read(_dba, USER_LIST: BuildUserListAsDataset(aUserRIDList));
                }
                else if (aIncludeAssigned)
                {
                    return StoredProcedures.MID_TASKLIST_READ_FOR_ASSIGNED.Read(_dba, USER_LIST: BuildUserListAsDataset(aUserRIDList));
                }
                else if (aIncludeOwned)
                {
                    return StoredProcedures.MID_TASKLIST_READ_FOR_OWNED.Read(_dba, USER_LIST: BuildUserListAsDataset(aUserRIDList));
                }
                else
                {
                    return StoredProcedures.MID_TASKLIST_READ_ALL.Read(_dba);
                }
			}
			catch
			{
				throw;
			}
		}
		
		public DataRow TaskList_Read(int aTaskListRID)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_TASKLIST_READ.Read(_dba, TASKLIST_RID: aTaskListRID);

				if (dt.Rows.Count == 1)
				{
					return dt.Rows[0];
				}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskList_ReadByJob(int aJobRID)
		{
			try
			{
                return StoredProcedures.MID_JOB_TASKLIST_JOIN_READ.Read(_dba, JOB_RID: aJobRID);
			}
			catch
			{
				throw;
			}
		}

		public int TaskList_Insert(TaskListProfile aTaskListProfile, int aUserRID)
		{
			try
			{
				return TaskList_Insert(aTaskListProfile.UnloadToDataRow(NewTaskListRow()), aUserRID);
			}
			catch
			{
				throw;
			}
		}

		public int TaskList_Insert(DataRow aRow, int aUserRID)
		{
			try
			{
                int tasklist_RID = StoredProcedures.SP_MID_TASKLIST_INSERT.InsertAndReturnRID(_dba,
                                                                                              TASKLIST_NAME: Convert.ToString(aRow["TASKLIST_NAME"], CultureInfo.CurrentUICulture),
                                                                                              USER_RID: Convert.ToInt32(aRow["USER_RID"], CultureInfo.CurrentUICulture),
                                                                                              SYSTEM_GENERATED_IND: Convert.ToChar(aRow["SYSTEM_GENERATED_IND"], CultureInfo.CurrentUICulture),
                                                                                              CREATED_BY_USER_RID: aUserRID,
                                                                                              CREATED_DATETIME: DateTime.Now,
                                                                                              LAST_MODIFIED_BY_USER_RID: aUserRID,
                                                                                              LAST_MODIFIED_DATETIME: DateTime.Now
                                                                                              );

                //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.AddUserItem(aUserRID, (int)eProfileType.TaskList, tasklist_RID, aUserRID);
                }

                //SecurityAdmin sa = new SecurityAdmin();
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    //sa.AddUserItem(aUserRID, (int)eSharedDataType.Tasklist, tasklist_RID, aUserRID);
                //    sa.AddUserItem(aUserRID, (int)eProfileType.TaskList, tasklist_RID, aUserRID);
                //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    sa.CommitData();
                //}
                //catch
                //{
                //    throw;
                //}
                //finally
                //{
                //    sa.CloseUpdateConnection();
                //}
                //End TT#1564 - DOConnell - Missing Tasklist record prevents Login

				return tasklist_RID;
			}
			catch
			{
				throw;
			}
		}

		public void TaskList_UpdateName(int aTaskListRID, string aName, int aUserRID)
		{
			try
			{
                StoredProcedures.MID_TASKLIST_UPDATE_NAME.Update(_dba,
                                                                 TASKLIST_RID: aTaskListRID,
                                                                 TASKLIST_NAME: aName,
                                                                 LAST_MODIFIED_BY_USER_RID: aUserRID,
                                                                 LAST_MODIFIED_DATETIME: DateTime.Now
                                                                 );
			}
			catch
			{
				throw;
			}
		}



        // Begin TT#72 - JSmith -  Sharing did not meet expectation for the tasklist explorer
        public void TaskList_UpdateNameandUserRID(int aTaskListRID, string aName, int aUserRID, int aModUserRID)
        {
            try
            {
                StoredProcedures.MID_TASKLIST_UPDATE_NAME_AND_USER.Update(_dba,
                                                                          TASKLIST_RID: aTaskListRID,
                                                                          TASKLIST_NAME: aName,
                                                                          USER_RID: aUserRID,
                                                                          LAST_MODIFIED_BY_USER_RID: aModUserRID,
                                                                          LAST_MODIFIED_DATETIME: DateTime.Now
                                                                          );
            }
            catch
            {
                throw;
            }
        }
        // End TT#72

		public void TaskList_Update(TaskListProfile aTaskListProfile, int aUserRID)
		{
			try
			{
				TaskList_Update(aTaskListProfile.UnloadToDataRow(NewTaskListRow()), aUserRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskList_Update(DataTable aTaskListTable, int aUserRID)
		{
			try
			{
				foreach (DataRow row in aTaskListTable.Rows)
				{
					TaskList_Update(row, aUserRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskList_Update(DataRow aTaskListRow, int aUserRID)
		{
			try
			{
                StoredProcedures.MID_TASKLIST_UPDATE.Update(_dba,
                                                            TASKLIST_RID: (int)aTaskListRow["TASKLIST_RID"],
                                                            TASKLIST_NAME: Convert.ToString(aTaskListRow["TASKLIST_NAME"], CultureInfo.CurrentUICulture),
                                                            USER_RID: Convert.ToInt32(aTaskListRow["USER_RID"], CultureInfo.CurrentUICulture),
                                                            LAST_MODIFIED_BY_USER_RID: aUserRID,
                                                            LAST_MODIFIED_DATETIME: DateTime.Now
                                                            );
			}
			catch
			{
				throw;
			}
		}

		public void TaskList_DeleteSystemFromList(DataTable aTable)
		{
			try
			{
				foreach (DataRow row in aTable.Rows)
				{
					if (Convert.ToChar(row["TASKLIST_SYSTEM_GENERATED_IND"]) == '1')
					{
						TaskList_Delete(Convert.ToInt32(row["TASKLIST_RID"]));
					}
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskList_Delete(int aTaskListRID)
		{
			try
			{
				TaskForecastDetail_Delete(aTaskListRID);
				TaskForecast_Delete(aTaskListRID);
				TaskAllocateDetail_Delete(aTaskListRID);
				TaskAllocate_Delete(aTaskListRID);
				TaskRollup_Delete(aTaskListRID);
				TaskPosting_Delete(aTaskListRID);
                TaskBatchComp_Delete(aTaskListRID);         // TT#1581-MD - stodd - header reconcile
                TaskHeaderReconcile_Delete(aTaskListRID);   // TT#1581-MD - stodd - header reconcile
				TaskProgram_Delete(aTaskListRID);
                // Begin TT#2071 - JSmith - Unable to delete Size Day to Week Tasks
                TaskSizeDayToWeekSummary_Delete(aTaskListRID);
                // End TT#2071
                // Begin TT#3998 - JSmith - Task List Explorer - Size Curve Task does not copy fields
                TaskSizeCurveMethod_Delete(aTaskListRID);
                TaskSizeCurves_Delete(aTaskListRID);
                TaskSizeCurveGenerateNode_Delete(aTaskListRID);
                // End TT#3998 - JSmith - Task List Explorer - Size Curve Task does not copy fields
				Task_Delete(aTaskListRID);

                StoredProcedures.MID_TASKLIST_DELETE.Delete(_dba, TASKLIST_RID: aTaskListRID);
                //Begin TT#1564 - DOConnell - Missing Tasklist record prevents Login
				//SecurityAdmin sa = new SecurityAdmin();
                if (ConnectionIsOpen)
                {
                    SecurityAdmin sa = new SecurityAdmin(_dba);
                    sa.DeleteUserItemByTypeAndRID((int)eProfileType.TaskList, aTaskListRID);
                }
                //try
                //{
                //    sa.OpenUpdateConnection();
                //    //Begin Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    //sa.DeleteUserItemByTypeAndRID((int)eSharedDataType.Tasklist, aTaskListRID);
                //    sa.DeleteUserItemByTypeAndRID((int)eProfileType.TaskList, aTaskListRID);
                //    //End Track #5005 - JScott - Need ability to organize methods, workflows, tasks, filters etc into subfolders
                //    sa.CommitData();
                //}
                //catch
                //{
                //    throw;
                //}
                //finally
                //{
                //    sa.CloseUpdateConnection();
                //}
                //End TT#1564 - DOConnell - Missing Tasklist record prevents Login
			}
			catch
			{
				throw;
			}
		}

		public DataTable JobTaskListJoin_ReadByJob(int aJobRID)
		{
			try
			{
                return StoredProcedures.MID_JOB_TASKLIST_JOIN_READ_FROM_JOB.Read(_dba, JOB_RID: aJobRID);
			}
			catch
			{
				throw;
			}
		}

		public void JobTaskListJoin_DeleteSystemFromList(DataTable aTable)
		{
			try
			{
				foreach (DataRow row in aTable.Rows)
				{
					if (Convert.ToChar(row["JOB_SYSTEM_GENERATED_IND"]) == '1')
					{
                        StoredProcedures.MID_JOB_TASKLIST_JOIN_DELETE.Delete(_dba,
                                                                     JOB_RID: (int)row["JOB_RID"],
                                                                     TASKLIST_RID: (int)row["TASKLIST_RID"]
                                                                     );
					}
				}
			}
			catch
			{
				throw;
			}
		}

		public void JobTaskListJoin_DeleteByJob(int aJobRID)
		{
			try
			{
                StoredProcedures.MID_JOB_TASKLIST_JOIN_DELETE_FROM_JOB.Delete(_dba, JOB_RID: aJobRID);
			}
			catch
			{
				throw;
			}
		}

		public void JobTaskListJoin_Insert(JobTaskListJoinProfile aJobTaskListJoinProfile)
		{
			try
			{
				JobTaskListJoin_Insert(aJobTaskListJoinProfile.UnloadToDataRow(NewJobTaskListJoinRow()));
			}
			catch
			{
				throw;
			}
		}

		public void JobTaskListJoin_Insert(DataTable aTaskTable)
		{
			try
			{
				foreach (DataRow row in aTaskTable.Rows)
				{
					JobTaskListJoin_Insert(row);
				}
			}
			catch
			{
				throw;
			}
		}

		public void JobTaskListJoin_Insert(DataTable aTaskTable, int aJobRID)
		{
			try
			{
				foreach (DataRow row in aTaskTable.Rows)
				{
					JobTaskListJoin_Insert(row, aJobRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void JobTaskListJoin_Insert(DataRow aRow)
		{
			try
			{
				JobTaskListJoin_Insert(aRow, Convert.ToInt32(aRow["JOB_RID"], CultureInfo.CurrentUICulture));
			}
			catch
			{
				throw;
			}
		}

		public void JobTaskListJoin_Insert(DataRow aRow, int aJobRID)
		{
			try
			{
                StoredProcedures.MID_JOB_TASKLIST_JOIN_INSERT.Insert(_dba,
                                                                     // TT#1722-MD - RMatelic - Data Layer Request - Unable to copy a job and paste it from one folder to anothe
                                                                     //JOB_RID: Convert.ToInt32(aRow["JOB_RID"], CultureInfo.CurrentUICulture),
                                                                     JOB_RID: aJobRID,
                                                                     // End TT#1722-MD
                                                                     TASKLIST_RID: Convert.ToInt32(aRow["TASKLIST_RID"], CultureInfo.CurrentUICulture),
                                                                     TASKLIST_SEQUENCE: Convert.ToInt32(aRow["TASKLIST_SEQUENCE"], CultureInfo.CurrentUICulture)
                                                                     );
			}
			catch
			{
				throw;
			}
		}

		public DataTable Task_ReadByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void Task_Delete(int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_DELETE_FROM_TASKLIST.Delete(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void Task_Insert(DataTable aTaskTable)
		{
			try
			{
				foreach (DataRow row in aTaskTable.Rows)
				{
					Task_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture));
				}
			}
			catch
			{
				throw;
			}
		}

		public void Task_Insert(DataTable aTaskTable, int aTaskListRID)
		{
			try
			{
				foreach (DataRow row in aTaskTable.Rows)
				{
					Task_Insert(row, aTaskListRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void Task_Insert(DataRow aRow, int aTaskListRID)
		{
			try
			{
                //TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
                string EMAIL_SUCCESS_FROM = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aRow["EMAIL_SUCCESS_FROM"] != DBNull.Value) EMAIL_SUCCESS_FROM = (string)aRow["EMAIL_SUCCESS_FROM"];

                string EMAIL_SUCCESS_TO = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aRow["EMAIL_SUCCESS_TO"] != DBNull.Value) EMAIL_SUCCESS_TO = (string)aRow["EMAIL_SUCCESS_TO"];

                string EMAIL_SUCCESS_CC = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aRow["EMAIL_SUCCESS_CC"] != DBNull.Value) EMAIL_SUCCESS_CC = (string)aRow["EMAIL_SUCCESS_CC"];

                string EMAIL_SUCCESS_BCC = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aRow["EMAIL_SUCCESS_BCC"] != DBNull.Value) EMAIL_SUCCESS_BCC = (string)aRow["EMAIL_SUCCESS_BCC"];

                string EMAIL_SUCCESS_SUBJECT = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aRow["EMAIL_SUCCESS_SUBJECT"] != DBNull.Value) EMAIL_SUCCESS_SUBJECT = (string)aRow["EMAIL_SUCCESS_SUBJECT"];

                string EMAIL_SUCCESS_BODY = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aRow["EMAIL_SUCCESS_BODY"] != DBNull.Value) EMAIL_SUCCESS_BODY = (string)aRow["EMAIL_SUCCESS_BODY"];

                string EMAIL_FAILURE_FROM = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aRow["EMAIL_FAILURE_FROM"] != DBNull.Value) EMAIL_FAILURE_FROM = (string)aRow["EMAIL_FAILURE_FROM"];

                string EMAIL_FAILURE_TO = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aRow["EMAIL_FAILURE_TO"] != DBNull.Value) EMAIL_FAILURE_TO = (string)aRow["EMAIL_FAILURE_TO"];

                string EMAIL_FAILURE_CC = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aRow["EMAIL_FAILURE_CC"] != DBNull.Value) EMAIL_FAILURE_CC = (string)aRow["EMAIL_FAILURE_CC"];

                string EMAIL_FAILURE_BCC = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aRow["EMAIL_FAILURE_BCC"] != DBNull.Value) EMAIL_FAILURE_BCC = (string)aRow["EMAIL_FAILURE_BCC"];

                string EMAIL_FAILURE_SUBJECT = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aRow["EMAIL_FAILURE_SUBJECT"] != DBNull.Value) EMAIL_FAILURE_SUBJECT = (string)aRow["EMAIL_FAILURE_SUBJECT"];

                string EMAIL_FAILURE_BODY = Include.NullForStringValue;  //TT#1310-MD -jsobek -Error when adding a new Store
                if (aRow["EMAIL_FAILURE_BODY"] != DBNull.Value) EMAIL_FAILURE_BODY = (string)aRow["EMAIL_FAILURE_BODY"];

                StoredProcedures.MID_TASK_INSERT.Insert(_dba,
                                                        TASKLIST_RID: aTaskListRID,
                                                        TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                        TASK_TYPE: Convert.ToInt32(aRow["TASK_TYPE"], CultureInfo.CurrentUICulture),
                                                        MAX_MESSAGE_LEVEL: Convert.ToInt32(aRow["MAX_MESSAGE_LEVEL"], CultureInfo.CurrentUICulture),
                                                        EMAIL_SUCCESS_FROM: EMAIL_SUCCESS_FROM,
                                                        EMAIL_SUCCESS_TO: EMAIL_SUCCESS_TO,
                                                        EMAIL_SUCCESS_CC: EMAIL_SUCCESS_CC,
                                                        EMAIL_SUCCESS_BCC: EMAIL_SUCCESS_BCC,
                                                        EMAIL_SUCCESS_SUBJECT: EMAIL_SUCCESS_SUBJECT,
                                                        EMAIL_SUCCESS_BODY: EMAIL_SUCCESS_BODY,
                                                        EMAIL_FAILURE_FROM: EMAIL_FAILURE_FROM,
                                                        EMAIL_FAILURE_TO: EMAIL_FAILURE_TO,
                                                        EMAIL_FAILURE_CC: EMAIL_FAILURE_CC,
                                                        EMAIL_FAILURE_BCC: EMAIL_FAILURE_BCC,
                                                        EMAIL_FAILURE_SUBJECT: EMAIL_FAILURE_SUBJECT,
                                                        EMAIL_FAILURE_BODY: EMAIL_FAILURE_BODY
                                                        );
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskForecast_ReadByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_FORECAST_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskForecast_ReadByTaskList(int aTaskListRID, int aTaskSeq)
		{
			try
			{
                return StoredProcedures.MID_TASK_FORECAST_READ_FROM_TASKLIST_AND_SEQ.Read(_dba,
                                                                                          TASKLIST_RID: aTaskListRID,
                                                                                          TASK_SEQUENCE: aTaskSeq
                                                                                          );
			}
			catch
			{
				throw;
			}
		}

		public void TaskForecast_Delete(int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_FORECAST_DELETE_FROM_TASKLIST.Delete(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskForecast_Insert(DataTable aTaskForecastTable)
		{
			try
			{
				foreach (DataRow row in aTaskForecastTable.Rows)
				{
					TaskForecast_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture));
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskForecast_Insert(DataTable aTaskForecastTable, int aTaskListRID)
		{
			try
			{
				foreach (DataRow row in aTaskForecastTable.Rows)
				{
					TaskForecast_Insert(row, aTaskListRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskForecast_Insert(DataRow aRow, int aTaskListRID)
		{
			try
			{
                int? HN_RID_Nullable = null;
                if (aRow["HN_RID"] != System.DBNull.Value) HN_RID_Nullable = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);

                int? FV_RID_Nullable = null;
                if (aRow["FV_RID"] != System.DBNull.Value) FV_RID_Nullable = Convert.ToInt32(aRow["FV_RID"], CultureInfo.CurrentUICulture);
                StoredProcedures.MID_TASK_FORECAST_INSERT.Insert(_dba,
                                                                 TASKLIST_RID: aTaskListRID,
                                                                 TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                 FORECAST_SEQUENCE: Convert.ToInt32(aRow["FORECAST_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                 HN_RID: HN_RID_Nullable,
                                                                 FV_RID: FV_RID_Nullable
                                                                 );
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskForecastDetail_ReadByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskForecastDetail_ReadByTaskList(int aTaskListRID, int aTaskSeq, int aforecastSeq)
		{
			try
			{
                return StoredProcedures.MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_AND_SEQ.Read(_dba,
                                                                                                 TASKLIST_RID: aTaskListRID,
                                                                                                 TASK_SEQUENCE: aTaskSeq,
                                                                                                 FORECAST_SEQUENCE: aforecastSeq
                                                                                                 );
			}
			catch
			{
				throw;
			}
		}

		public void TaskForecastDetail_Delete(int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_FORECAST_DETAIL_DELETE_FROM_TASKLIST.Delete(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskForecastDetail_Insert(DataTable aTaskForecastDetailTable)
		{
			try
			{
				foreach (DataRow row in aTaskForecastDetailTable.Rows)
				{
					TaskForecastDetail_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture));
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskForecastDetail_Insert(DataTable aTaskForecastDetailTable, int aTaskListRID)
		{
			try
			{
				foreach (DataRow row in aTaskForecastDetailTable.Rows)
				{
					TaskForecastDetail_Insert(row, aTaskListRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskForecastDetail_Insert(DataRow aRow, int aTaskListRID)
		{
			try
			{
                int? WORKFLOW_METHOD_IND_Nullable = null;
                if (aRow["WORKFLOW_METHOD_IND"] != System.DBNull.Value) WORKFLOW_METHOD_IND_Nullable = Convert.ToInt32(aRow["WORKFLOW_METHOD_IND"], CultureInfo.CurrentUICulture);

                int? METHOD_RID_Nullable = null;
                if (aRow["METHOD_RID"] != System.DBNull.Value) METHOD_RID_Nullable = Convert.ToInt32(aRow["METHOD_RID"], CultureInfo.CurrentUICulture);

                int? WORKFLOW_RID_Nullable = null;
                if (aRow["WORKFLOW_RID"] != System.DBNull.Value) WORKFLOW_RID_Nullable = Convert.ToInt32(aRow["WORKFLOW_RID"], CultureInfo.CurrentUICulture);

                int? EXECUTE_CDR_RID_Nullable = null;
                if (aRow["EXECUTE_CDR_RID"] != System.DBNull.Value) EXECUTE_CDR_RID_Nullable = Convert.ToInt32(aRow["EXECUTE_CDR_RID"], CultureInfo.CurrentUICulture);

                StoredProcedures.MID_TASK_FORECAST_DETAIL_INSERT.Insert(_dba,
                                                                        TASKLIST_RID: aTaskListRID,
                                                                        TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                        FORECAST_SEQUENCE: Convert.ToInt32(aRow["FORECAST_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                        DETAIL_SEQUENCE: Convert.ToInt32(aRow["DETAIL_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                        WORKFLOW_METHOD_IND: WORKFLOW_METHOD_IND_Nullable,
                                                                        METHOD_RID: METHOD_RID_Nullable,
                                                                        WORKFLOW_RID: WORKFLOW_RID_Nullable,
                                                                        EXECUTE_CDR_RID: EXECUTE_CDR_RID_Nullable
                                                                        );
			}
			catch
			{
				throw;
			}
		}


		public DataTable TaskForecastDetail_ReadByMethod(int methodRid)
		{
			try
			{
                return StoredProcedures.MID_TASK_FORECAST_DETAIL_READ_FROM_METHOD.Read(_dba, METHOD_RID: methodRid);
			}
			catch
			{
				throw;
			}
		}

		
		public DataTable TaskSizeCurveGenerate_ReadByTaskList(int aTaskListRID, int aTaskSeq)
		{
			try
			{
                return StoredProcedures.MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_AND_SEQ.Read(_dba,
                                                                                                     TASKLIST_RID: aTaskListRID,
                                                                                                     TASK_SEQUENCE: aTaskSeq
                                                                                                     );
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskSizeCurveMethod_ReadByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskSizeCurveMethod_Delete(int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST.Delete(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskSizeCurveMethod_Insert(DataTable aTaskSzCrvGenTable)
		{
			try
			{
				foreach (DataRow row in aTaskSzCrvGenTable.Rows)
				{
					TaskSizeCurveGenerate_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture), eSizeCurveGenerateType.Method);
				}
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskSizeCurves_ReadByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_FOR_NODES.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskSizeCurves_Delete(int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_FOR_NODES.Delete(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskSizeCurves_Insert(DataTable aTaskSzCrvGenTable)
		{
			try
			{
				foreach (DataRow row in aTaskSzCrvGenTable.Rows)
				{
					TaskSizeCurveGenerate_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture), eSizeCurveGenerateType.Node);
				}
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#3998 - JSmith - Task List Explorer - Size Curve Task does not copy fields
        public void TaskSizeCurves_Insert(DataTable aTaskSzCrvGenTable, int aTaskListRID)
        {
            try
            {
                foreach (DataRow row in aTaskSzCrvGenTable.Rows)
                {
                    TaskSizeCurveGenerate_Insert(row, aTaskListRID, eSizeCurveGenerateType.Node);
                }
            }
            catch
            {
                throw;
            }
        }
        // End TT#3998 - JSmith - Task List Explorer - Size Curve Task does not copy fields

        //BEGIN TT#3997-VStuart-Task List Explorer - Size Curve Method Task does not copy properly
        public void TaskSizeCurveGenerate_Insert(DataTable aTaskSzCrvGenTable, int aTaskListRID, eSizeCurveGenerateType aGenType)
        //private void TaskSizeCurveGenerate_Insert(DataTable aTaskSzCrvGenTable, int aTaskListRID, eSizeCurveGenerateType aGenType)
        {
            try
            {
                foreach (DataRow row in aTaskSzCrvGenTable.Rows)
                {
                    TaskSizeCurveGenerate_Insert(row, aTaskListRID, aGenType);
                }
            }
            catch
            {
                throw;
            }
        }
        //END TT#3997-VStuart-Task List Explorer - Size Curve Method Task does not copy properly

		private void TaskSizeCurveGenerate_Insert(DataRow aRow, int aTaskListRID, eSizeCurveGenerateType aGenType)
		{
			try
			{
                int? HN_RID_Nullable = null;
                if (aRow["HN_RID"] != System.DBNull.Value) HN_RID_Nullable = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);

                int? METHOD_RID_Nullable = null;
                if (aRow["METHOD_RID"] != System.DBNull.Value) METHOD_RID_Nullable = Convert.ToInt32(aRow["METHOD_RID"], CultureInfo.CurrentUICulture);

                int? EXECUTE_CDR_RID_Nullable = null;
                if (aRow["EXECUTE_CDR_RID"] != System.DBNull.Value) EXECUTE_CDR_RID_Nullable = Convert.ToInt32(aRow["EXECUTE_CDR_RID"], CultureInfo.CurrentUICulture);

                StoredProcedures.MID_TASK_SIZE_CURVE_GENERATE_INSERT.Insert(_dba,
                                                                            TASKLIST_RID: aTaskListRID,
                                                                            TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                            GENERATE_SEQUENCE: Convert.ToInt32(aRow["GENERATE_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                            METHOD_NODE_TYPE: Convert.ToInt32(aGenType, CultureInfo.CurrentUICulture),
                                                                            HN_RID: HN_RID_Nullable,
                                                                            METHOD_RID: METHOD_RID_Nullable,
                                                                            EXECUTE_CDR_RID: EXECUTE_CDR_RID_Nullable
                                                                            );
			}
			catch
			{
				throw;
			}
		}

		//End TT#155 - JScott - Add Size Curve info to Node Properties
		//Begin TT#707 - JScott - Size Curve process needs to multi-thread
		public DataRow TaskSizeCurveGenerateNode_Read(int aTaskListRID, int aTaskSequence)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST_AND_SEQ.Read(_dba,
                                                                                                          TASKLIST_RID: aTaskListRID,
                                                                                                          TASK_SEQUENCE: aTaskSequence
                                                                                                          );

				if (dt.Rows.Count == 1)
				{
					return dt.Rows[0];
				}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskSizeCurveGenerateNode_ReadByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_SIZE_CURVE_GENERATE_NODE_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskSizeCurveGenerateNode_Delete(int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_SIZE_CURVE_GENERATE_NODE_DELETE_FROM_TASKLIST.Delete(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskSizeCurveGenerateNode_Insert(DataTable aTaskSizeCurveGenerateNodeTable)
		{
			try
			{
				foreach (DataRow row in aTaskSizeCurveGenerateNodeTable.Rows)
				{
					TaskSizeCurveGenerateNode_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture));
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskSizeCurveGenerateNode_Insert(DataTable aTaskSizeCurveGenerateNodeTable, int aTaskListRID)
		{
			try
			{
				foreach (DataRow row in aTaskSizeCurveGenerateNodeTable.Rows)
				{
					TaskSizeCurveGenerateNode_Insert(row, aTaskListRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskSizeCurveGenerateNode_Insert(DataRow aRow, int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_SIZE_CURVE_GENERATE_NODE_INSERT.Insert(_dba,
                                                                                 TASKLIST_RID: aTaskListRID,
                                                                                 TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                                 CONCURRENT_PROCESSES: Convert.ToInt32(aRow["CONCURRENT_PROCESSES"], CultureInfo.CurrentUICulture)
                                                                                 );
			}
			catch
			{
				throw;
			}
		}

		//End TT#707 - JScott - Size Curve process needs to multi-thread
		//Begin TT#391 - stodd - size day to week summary
		public DataTable TaskSizeDayToWeekSummary_ReadByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskSizeDayToWeekSummary_ReadByTaskList(int aTaskListRID, int aTaskSeq)
		{
			try
			{
                return StoredProcedures.MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST_AND_SEQ.Read(_dba,
                                                                                                          TASKLIST_RID: aTaskListRID,
                                                                                                          TASK_SEQUENCE: aTaskSeq
                                                                                                          );
			}
			catch
			{
				throw;
			}
		}

		public void TaskSizeDayToWeekSummary_Delete(int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_DELETE_FROM_TASKLIST.Delete(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

        //BEGIN TT#3999-VStuart-Task List Explorer-Size Day to Week Summary task does not copy fields
        public void TaskSizeDayToWeekSummary_Insert(DataTable aTaskSzDyWkSumTable, int aTaskListRID)
        {
            try
            {
                foreach (DataRow row in aTaskSzDyWkSumTable.Rows)
                {
                    TaskSizeDayToWeekSummary_Insert(row, aTaskListRID);
                }
            }
            catch
            {
                throw;
            }
        }
        //END TT#3999-VStuart-Task List Explorer-Size Day to Week Summary task does not copy fields

		public void TaskSizeDayToWeekSummary_Insert(DataTable aTaskSzDyWkSummTable)
		{
			try
			{
				foreach (DataRow row in aTaskSzDyWkSummTable.Rows)
				{
					TaskSizeDayToWeekSummary_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture));
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskSizeDayToWeekSummary_Insert(DataRow aRow, int aTaskListRID)
		{
			try
			{
                int? CDR_RID_Nullable = null;
                if (aRow["CDR_RID"] != System.DBNull.Value) CDR_RID_Nullable = Convert.ToInt32(aRow["CDR_RID"], CultureInfo.CurrentUICulture);

                int? HN_RID_Nullable = null;
                if (aRow["HN_RID"] != System.DBNull.Value && (int)aRow["HN_RID"] != Include.NoRID) HN_RID_Nullable = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture); //TT#1245-MD -jsobek -Foreign Key Error – Size Day To Week Summary

                StoredProcedures.MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_INSERT.Insert(_dba,
                                                                                 TASKLIST_RID: aTaskListRID,
                                                                                 TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                                 CDR_RID: CDR_RID_Nullable,
                                                                                 HN_RID: HN_RID_Nullable
                                                                                 );
			}
			catch
			{
				throw;
			}
		}

		//End TT#391 - stodd - size day to week summary

		public DataTable TaskAllocateDetail_ReadByMethod(int aMethodRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_ALLOCATE_DETAIL_READ_FROM_METHOD.Read(_dba, METHOD_RID: aMethodRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetAllocationTasksByNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_ALLOCATE_READ_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetForecastTasksByNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_FORECAST_READ_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetForecastBalanceTasksByNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_FORECAST_BALANCE_READ_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable GetRollupTasksByNode(int aNodeRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_ROLLUP_READ_FROM_NODE.Read(_dba, HN_RID: aNodeRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskForecastDetail_ReadByWorkflow(int workflowRid)
		{
			try
			{
                return StoredProcedures.MID_TASK_FORECAST_DETAIL_READ_FROM_WORKFLOW.Read(_dba, WORKFLOW_RID: workflowRid);
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskAllocate_ReadByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_ALLOCATE_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskAllocate_ReadByTaskList(int aTaskListRID, int aTaskSeq)
		{
			try
			{
                return StoredProcedures.MID_TASK_ALLOCATE_READ_FROM_TASKLIST_AND_SEQ.Read(_dba,
                                                                                          TASKLIST_RID: aTaskListRID,
                                                                                          TASK_SEQUENCE: aTaskSeq
                                                                                          );
			}
			catch
			{
				throw;
			}
		}

		public void TaskAllocate_Delete(int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_ALLOCATE_DELETE_FROM_TASKLIST.Delete(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskAllocate_Insert(DataTable aTaskAllocateTable)
		{
			try
			{
				foreach (DataRow row in aTaskAllocateTable.Rows)
				{
					TaskAllocate_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture));
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskAllocate_Insert(DataTable aTaskAllocateTable, int aTaskListRID)
		{
			try
			{
				foreach (DataRow row in aTaskAllocateTable.Rows)
				{
					TaskAllocate_Insert(row, aTaskListRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskAllocate_Insert(DataRow aRow, int aTaskListRID)
		{
			try
			{
                int? HN_RID_Nullable = null;
                if (aRow["HN_RID"] != System.DBNull.Value) HN_RID_Nullable = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);

                //Begin TT#1313-MD -jsobek -Header Filters
                //int? ALLOCATE_TYPE_Nullable = null;
                //if (aRow["ALLOCATE_TYPE"] != System.DBNull.Value) ALLOCATE_TYPE_Nullable = Convert.ToInt32(aRow["ALLOCATE_TYPE"], CultureInfo.CurrentUICulture);

                string HEADER_ID = "null";
                //if (aRow["HEADER_ID"] != System.DBNull.Value) HEADER_ID = Convert.ToString(aRow["HEADER_ID"], CultureInfo.CurrentUICulture);

                string PO_ID = "null";
                //if (aRow["PO_ID"] != System.DBNull.Value) PO_ID = Convert.ToString(aRow["PO_ID"], CultureInfo.CurrentUICulture);

                int? FILTER_RID_Nullable = null;
                if (aRow["FILTER_RID"] != System.DBNull.Value) FILTER_RID_Nullable = Convert.ToInt32(aRow["FILTER_RID"], CultureInfo.CurrentUICulture);

                StoredProcedures.MID_TASK_ALLOCATE_INSERT.Insert(_dba,
                                                                 TASKLIST_RID: aTaskListRID,
                                                                 TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                 ALLOCATE_SEQUENCE: Convert.ToInt32(aRow["ALLOCATE_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                 HN_RID: HN_RID_Nullable,
                                                                 FILTER_RID: FILTER_RID_Nullable
                                                                 );
                //End TT#1313-MD -jsobek -Header Filters
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskAllocateDetail_ReadByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_ALLOCATE_DETAIL_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskAllocateDetail_ReadByTaskList(int aTaskListRID, int aTaskSeq, int aAllocateSeq)
		{
			try
			{
                return StoredProcedures.MID_TASK_ALLOCATE_DETAIL_READ.Read(_dba,
                                                                           TASKLIST_RID: aTaskListRID,
                                                                           TASK_SEQUENCE: aTaskSeq,
                                                                           ALLOCATE_SEQUENCE: aAllocateSeq
                                                                           );
			}
			catch
			{
				throw;
			}
		}

		public void TaskAllocateDetail_Delete(int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_ALLOCATE_DETAIL_DELETE_FROM_TASKLIST.Delete(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskAllocateDetail_Insert(DataTable aTaskAllocateDetailTable)
		{
			try
			{
				foreach (DataRow row in aTaskAllocateDetailTable.Rows)
				{
					TaskAllocateDetail_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture));
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskAllocateDetail_Insert(DataTable aTaskAllocateDetailTable, int aTaskListRID)
		{
			try
			{
				foreach (DataRow row in aTaskAllocateDetailTable.Rows)
				{
					TaskAllocateDetail_Insert(row, aTaskListRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskAllocateDetail_Insert(DataRow aRow, int aTaskListRID)
		{
			try
			{
                int? WORKFLOW_METHOD_IND_Nullable = null;
                if (aRow["WORKFLOW_METHOD_IND"] != System.DBNull.Value) WORKFLOW_METHOD_IND_Nullable = Convert.ToInt32(aRow["WORKFLOW_METHOD_IND"], CultureInfo.CurrentUICulture);

                int? METHOD_RID_Nullable = null;
                if (aRow["METHOD_RID"] != System.DBNull.Value) METHOD_RID_Nullable = Convert.ToInt32(aRow["METHOD_RID"], CultureInfo.CurrentUICulture);

                int? WORKFLOW_RID_Nullable = null;
                if (aRow["WORKFLOW_RID"] != System.DBNull.Value) WORKFLOW_RID_Nullable = Convert.ToInt32(aRow["WORKFLOW_RID"], CultureInfo.CurrentUICulture);

                int? EXECUTE_CDR_RID_Nullable = null;
                if (aRow["EXECUTE_CDR_RID"] != System.DBNull.Value) EXECUTE_CDR_RID_Nullable = Convert.ToInt32(aRow["EXECUTE_CDR_RID"], CultureInfo.CurrentUICulture);

                StoredProcedures.MID_TASK_ALLOCATE_DETAIL_INSERT.Insert(_dba,
                                                                        TASKLIST_RID: aTaskListRID,
                                                                        TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                        ALLOCATE_SEQUENCE: Convert.ToInt32(aRow["ALLOCATE_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                        DETAIL_SEQUENCE: Convert.ToInt32(aRow["DETAIL_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                        WORKFLOW_METHOD_IND: WORKFLOW_METHOD_IND_Nullable,
                                                                        METHOD_RID: METHOD_RID_Nullable,
                                                                        WORKFLOW_RID: WORKFLOW_RID_Nullable,
                                                                        EXECUTE_CDR_RID: EXECUTE_CDR_RID_Nullable
                                                                        );
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskRollup_ReadByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_ROLLUP_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskRollup_ReadByTaskList(int aTaskListRID, int aTaskSeq)
		{
			try
			{
                return StoredProcedures.MID_TASK_ROLLUP_READ_FROM_TASKLIST_AND_SEQ.Read(_dba,
                                                                                        TASKLIST_RID: aTaskListRID,
                                                                                        TASK_SEQUENCE: aTaskSeq
                                                                                        );
			}
			catch
			{
				throw;
			}
		}

		public void TaskRollup_Delete(int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_ROLLUP_DELETE.Delete(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskRollup_Insert(DataTable aTaskRollupTable)
		{
			try
			{
				foreach (DataRow row in aTaskRollupTable.Rows)
				{
					TaskRollup_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture));
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskRollup_Insert(DataTable aTaskRollupTable, int aTaskListRID)
		{
			try
			{
				foreach (DataRow row in aTaskRollupTable.Rows)
				{
					TaskRollup_Insert(row, aTaskListRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskRollup_Insert(DataRow aRow, int aTaskListRID)
		{
			try
			{
                int? HN_RID_Nullable = null;
                if (aRow["HN_RID"] != System.DBNull.Value) HN_RID_Nullable = Convert.ToInt32(aRow["HN_RID"], CultureInfo.CurrentUICulture);

                int? FV_RID_Nullable = null;
                if (aRow["FV_RID"] != System.DBNull.Value) FV_RID_Nullable = Convert.ToInt32(aRow["FV_RID"], CultureInfo.CurrentUICulture);

                int? ROLLUP_CDR_RID_Nullable = null;
                if (aRow["ROLLUP_CDR_RID"] != System.DBNull.Value) ROLLUP_CDR_RID_Nullable = Convert.ToInt32(aRow["ROLLUP_CDR_RID"], CultureInfo.CurrentUICulture);

                int? FROM_PH_OFFSET_IND_Nullable = Convert.ToInt32(eHierarchyDescendantType.levelType);
                if (aRow["FROM_PH_OFFSET_IND"] != System.DBNull.Value) FROM_PH_OFFSET_IND_Nullable = Convert.ToInt32(aRow["FROM_PH_OFFSET_IND"], CultureInfo.CurrentUICulture);

                int? FROM_PH_RID_Nullable = null;
                if (aRow["FROM_PH_RID"] != System.DBNull.Value) FROM_PH_RID_Nullable = Convert.ToInt32(aRow["FROM_PH_RID"], CultureInfo.CurrentUICulture);

                int? FROM_PHL_SEQUENCE_Nullable = null;
                // Begin TT#4766 - JSmith - Rollup task not saving correctly
                //if (aRow["TO_PHL_SEQUENCE"] != System.DBNull.Value && Convert.ToInt32(aRow["TO_PHL_SEQUENCE"], CultureInfo.CurrentUICulture) != 0) FROM_PHL_SEQUENCE_Nullable = Convert.ToInt32(aRow["FROM_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
                if (aRow["FROM_PHL_SEQUENCE"] != System.DBNull.Value && Convert.ToInt32(aRow["FROM_PHL_SEQUENCE"], CultureInfo.CurrentUICulture) != 0) FROM_PHL_SEQUENCE_Nullable = Convert.ToInt32(aRow["FROM_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);
                // End TT#4766 - JSmith - Rollup task not saving correctly

                int? FROM_OFFSET_Nullable = null;
                if (aRow["FROM_OFFSET"] != System.DBNull.Value) FROM_OFFSET_Nullable = Convert.ToInt32(aRow["FROM_OFFSET"], CultureInfo.CurrentUICulture);

                int? TO_PH_OFFSET_IND_Nullable = null;
                if (aRow["TO_PH_OFFSET_IND"] != System.DBNull.Value) TO_PH_OFFSET_IND_Nullable = Convert.ToInt32(aRow["TO_PH_OFFSET_IND"], CultureInfo.CurrentUICulture);

                int? TO_PH_RID_Nullable = null;
                if (aRow["TO_PH_RID"] != System.DBNull.Value) TO_PH_RID_Nullable = Convert.ToInt32(aRow["TO_PH_RID"], CultureInfo.CurrentUICulture);

                int? TO_PHL_SEQUENCE_Nullable = null;
                if (aRow["TO_PHL_SEQUENCE"] != System.DBNull.Value && Convert.ToInt32(aRow["TO_PHL_SEQUENCE"], CultureInfo.CurrentUICulture) != 0) TO_PHL_SEQUENCE_Nullable = Convert.ToInt32(aRow["TO_PHL_SEQUENCE"], CultureInfo.CurrentUICulture);

                int? TO_OFFSET_Nullable = null;
                if (aRow["TO_OFFSET"] != System.DBNull.Value) TO_OFFSET_Nullable = Convert.ToInt32(aRow["TO_OFFSET"], CultureInfo.CurrentUICulture);

                char? INTRANSIT_IND_Nullable = null;
                if (aRow["INTRANSIT_IND"] != System.DBNull.Value) INTRANSIT_IND_Nullable = Convert.ToChar(aRow["INTRANSIT_IND"], CultureInfo.CurrentUICulture);

                char? RECLASS_IND_Nullable = null;
                if (aRow["RECLASS_IND"] != System.DBNull.Value) RECLASS_IND_Nullable = Convert.ToChar(aRow["RECLASS_IND"], CultureInfo.CurrentUICulture);

                StoredProcedures.MID_TASK_ROLLUP_INSERT.Insert(_dba,
                                                               TASKLIST_RID: aTaskListRID,
                                                               TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                               ROLLUP_SEQUENCE: Convert.ToInt32(aRow["ROLLUP_SEQUENCE"], CultureInfo.CurrentUICulture).ToString(),
                                                               HN_RID: HN_RID_Nullable,
                                                               FV_RID: FV_RID_Nullable,
                                                               ROLLUP_CDR_RID: ROLLUP_CDR_RID_Nullable,
                                                               FROM_PH_OFFSET_IND: FROM_PH_OFFSET_IND_Nullable,
                                                               FROM_PH_RID: FROM_PH_RID_Nullable,
                                                               FROM_PHL_SEQUENCE: FROM_PHL_SEQUENCE_Nullable,
                                                               FROM_OFFSET: FROM_OFFSET_Nullable,
                                                               TO_PH_OFFSET_IND: TO_PH_OFFSET_IND_Nullable,
                                                               TO_PH_RID: TO_PH_RID_Nullable,
                                                               TO_PHL_SEQUENCE: TO_PHL_SEQUENCE_Nullable,
                                                               TO_OFFSET: TO_OFFSET_Nullable,
                                                               POSTING_IND: Convert.ToChar(aRow["POSTING_IND"], CultureInfo.CurrentUICulture),
                                                               HIERARCHY_LEVELS_IND: Convert.ToChar(aRow["HIERARCHY_LEVELS_IND"], CultureInfo.CurrentUICulture),
                                                               DAY_TO_WEEK_IND: Convert.ToChar(aRow["DAY_TO_WEEK_IND"], CultureInfo.CurrentUICulture),
                                                               DAY_IND: Convert.ToChar(aRow["DAY_IND"], CultureInfo.CurrentUICulture),
                                                               WEEK_IND: Convert.ToChar(aRow["WEEK_IND"], CultureInfo.CurrentUICulture),
                                                               STORE_IND: Convert.ToChar(aRow["STORE_IND"], CultureInfo.CurrentUICulture),
                                                               CHAIN_IND: Convert.ToChar(aRow["CHAIN_IND"], CultureInfo.CurrentUICulture),
                                                               STORE_TO_CHAIN_IND: Convert.ToChar(aRow["STORE_TO_CHAIN_IND"], CultureInfo.CurrentUICulture),
                                                               INTRANSIT_IND: INTRANSIT_IND_Nullable,
                                                               RECLASS_IND: RECLASS_IND_Nullable
                                                               );
			}
			catch
			{
				throw;
			}
		}

		public DataRow TaskPosting_Read(int aTaskListRID, int aTaskSequence)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_TASK_POSTING_READ_FROM_TASKLIST_AND_SEQ.Read(_dba,
                                                                                         TASKLIST_RID: aTaskListRID,
                                                                                         TASK_SEQUENCE: aTaskSequence
                                                                                         );

				if (dt.Rows.Count == 1)
				{
					return dt.Rows[0];
				}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskPosting_ReadByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_POSTING_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskPosting_Delete(int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_POSTING_DELETE_FROM_TASKLIST.Delete(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskPosting_Insert(DataTable aTaskPostingTable)
		{
			try
			{
				foreach (DataRow row in aTaskPostingTable.Rows)
				{
					TaskPosting_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture));
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskPosting_Insert(DataTable aTaskPostingTable, int aTaskListRID)
		{
			try
			{
				foreach (DataRow row in aTaskPostingTable.Rows)
				{
					TaskPosting_Insert(row, aTaskListRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskPosting_Insert(DataRow aRow, int aTaskListRID)
		{
			try
			{
               
                int FILE_PROCESSING_DIRECTION;
                if (aRow["FILE_PROCESSING_DIRECTION"] == DBNull.Value)
                {
                    FILE_PROCESSING_DIRECTION = (int)eAPIFileProcessingDirection.Config.GetHashCode();
                }
                else
                {
                    FILE_PROCESSING_DIRECTION = Convert.ToInt32(aRow["FILE_PROCESSING_DIRECTION"], CultureInfo.CurrentUICulture);
                }
                StoredProcedures.MID_TASK_POSTING_INSERT.Insert(_dba,
                                                                TASKLIST_RID: aTaskListRID,
                                                                TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                INPUT_DIRECTORY: Convert.ToString(aRow["INPUT_DIRECTORY"], CultureInfo.CurrentUICulture),
                                                                FILE_MASK: Convert.ToString(aRow["FILE_MASK"], CultureInfo.CurrentUICulture),
                                                                CONCURRENT_FILES: Convert.ToInt32(aRow["CONCURRENT_FILES"], CultureInfo.CurrentUICulture),
                                                                RUN_UNTIL_FILE_PRESENT_IND: Convert.ToChar(aRow["RUN_UNTIL_FILE_PRESENT_IND"], CultureInfo.CurrentUICulture),
                                                                RUN_UNTIL_FILE_MASK: Convert.ToString(aRow["RUN_UNTIL_FILE_MASK"], CultureInfo.CurrentUICulture),
                                                                FILE_PROCESSING_DIRECTION: FILE_PROCESSING_DIRECTION
                                                                );
			}
			catch
			{
				throw;
			}
		}

        public DataRow TaskHeaderReconcile_Read(int aTaskListRID, int aTaskSequence)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_AND_SEQ.Read(_dba,
                                                                                         TASKLIST_RID: aTaskListRID,
                                                                                         TASK_SEQUENCE: aTaskSequence
                                                                                         );

                if (dt.Rows.Count == 1)
                {
                    return dt.Rows[0];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable TaskHeaderReconcile_ReadByTaskList(int aTaskListRID)
        {
            try
            {
                return StoredProcedures.MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
            }
            catch
            {
                throw;
            }
        }

        public void TaskHeaderReconcile_Delete(int aTaskListRID)
        {
            try
            {
                StoredProcedures.MID_TASK_HEADER_RECONCILE_DELETE_FROM_TASKLIST.Delete(_dba, TASKLIST_RID: aTaskListRID);
            }
            catch
            {
                throw;
            }
        }

        public void TaskHeaderReconcile_Insert(DataTable aTaskHeaderReconcileTable)
        {
            try
            {
                foreach (DataRow row in aTaskHeaderReconcileTable.Rows)
                {
                    TaskHeaderReconcile_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture));
                }
            }
            catch
            {
                throw;
            }
        }

        public void TaskHeaderReconcile_Insert(DataTable aTaskHeaderReconcileTable, int aTaskListRID)
        {
            try
            {
                foreach (DataRow row in aTaskHeaderReconcileTable.Rows)
                {
                    TaskHeaderReconcile_Insert(row, aTaskListRID);
                }
            }
            catch
            {
                throw;
            }
        }

        public void TaskHeaderReconcile_Insert(DataRow aRow, int aTaskListRID)
        {
            try
            {

                StoredProcedures.MID_TASK_HEADER_RECONCILE_INSERT.Insert(_dba,
                                                                TASKLIST_RID: aTaskListRID,
                                                                TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"]),
                                                                INPUT_DIRECTORY: Convert.ToString(aRow["INPUT_DIRECTORY"]),
                                                                OUTPUT_DIRECTORY: Convert.ToString(aRow["OUTPUT_DIRECTORY"]),
                                                                TRIGGER_SUFFIX: Convert.ToString(aRow["TRIGGER_SUFFIX"]),
                                                                REMOVE_TRANS_FILE_NAME: Convert.ToString(aRow["REMOVE_TRANS_FILE_NAME"]),
                                                                REMOVE_TRANS_TRIGGER_SUFFIX: Convert.ToString(aRow["REMOVE_TRANS_TRIGGER_SUFFIX"]),
                                                                HEADER_TYPES: Convert.ToString(aRow["HEADER_TYPES"]),
                                                                HEADER_KEYS_FILE_NAME: Convert.ToString(aRow["HEADER_KEYS_FILE_NAME"])
                                                                );
            }
            catch
            {
                throw;
            }
        }


		public DataRow TaskProgram_Read(int aTaskListRID, int aTaskSequence)
		{
			try
			{
                DataTable dt = StoredProcedures.MID_TASK_PROGRAM_READ_FROM_TASKLIST_AND_SEQ.Read(_dba,
                                                                                         TASKLIST_RID: aTaskListRID,
                                                                                         TASK_SEQUENCE: aTaskSequence
				                                                                         );

				if (dt.Rows.Count == 1)
				{
					return dt.Rows[0];
				}
				else
				{
					return null;
				}
			}
			catch
			{
				throw;
			}
		}

		public DataTable TaskProgram_ReadByTaskList(int aTaskListRID)
		{
			try
			{
                return StoredProcedures.MID_TASK_PROGRAM_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskProgram_Delete(int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_PROGRAM_DELETE.Delete(_dba, TASKLIST_RID: aTaskListRID);
			}
			catch
			{
				throw;
			}
		}

		public void TaskProgram_Insert(DataTable aTaskProgramTable)
		{
			try
			{
				foreach (DataRow row in aTaskProgramTable.Rows)
				{
					TaskProgram_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture));
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskProgram_Insert(DataTable aTaskProgramTable, int aTaskListRID)
		{
			try
			{
				foreach (DataRow row in aTaskProgramTable.Rows)
				{
					TaskProgram_Insert(row, aTaskListRID);
				}
			}
			catch
			{
				throw;
			}
		}

		public void TaskProgram_Insert(DataRow aRow, int aTaskListRID)
		{
			try
			{
                StoredProcedures.MID_TASK_PROGRAM_INSERT.Insert(_dba,
                                                                TASKLIST_RID: aTaskListRID,
                                                                TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                PROGRAM_PATH: Convert.ToString(aRow["PROGRAM_PATH"], CultureInfo.CurrentUICulture),
                                                                PROGRAM_PARMS: Convert.ToString(aRow["PROGRAM_PARMS"], CultureInfo.CurrentUICulture)
                                                                );
			}
			catch
			{
				throw;
			}
		}

        // Begin TT#1595-MD - stodd - Batch Comp
        public DataRow TaskBatchComp_Read(int aTaskListRID, int aTaskSequence)
        {
            try
            {
                DataTable dt = StoredProcedures.MID_TASK_BATCH_COMP_READ_FROM_TASKLIST_AND_SEQ.Read(_dba,
                                                                                         TASKLIST_RID: aTaskListRID,
                                                                                         TASK_SEQUENCE: aTaskSequence
                                                                                         );

                if (dt.Rows.Count == 1)
                {
                    return dt.Rows[0];
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                throw;
            }
        }

        public DataTable TaskBatchComp_ReadByTaskList(int aTaskListRID)
        {
            try
            {
                return StoredProcedures.MID_TASK_BATCH_COMP_READ_FROM_TASKLIST.Read(_dba, TASKLIST_RID: aTaskListRID);
            }
            catch
            {
                throw;
            }
        }

        public void TaskBatchComp_Delete(int aTaskListRID)
        {
            try
            {
                StoredProcedures.MID_TASK_BATCH_COMP_DELETE_FROM_TASKLIST.Delete(_dba, TASKLIST_RID: aTaskListRID);
            }
            catch
            {
                throw;
            }
        }

        public void TaskBatchComp_Insert(DataTable aTaskBatchCompTable)
        {
            try
            {
                foreach (DataRow row in aTaskBatchCompTable.Rows)
                {
                    TaskBatchComp_Insert(row, Convert.ToInt32(row["TASKLIST_RID"], CultureInfo.CurrentUICulture));
                }
            }
            catch
            {
                throw;
            }
        }

        public void TaskBatchComp_Insert(DataTable aTaskBatchCompTable, int aTaskListRID)
        {
            try
            {
                foreach (DataRow row in aTaskBatchCompTable.Rows)
                {
                    TaskBatchComp_Insert(row, aTaskListRID);
                }
            }
            catch
            {
                throw;
            }
        }

        public void TaskBatchComp_Insert(DataRow aRow, int aTaskListRID)
        {
            try
            {
                StoredProcedures.MID_TASK_BATCH_COMP_INSERT.Insert(_dba,
                                                                TASKLIST_RID: aTaskListRID,
                                                                TASK_SEQUENCE: Convert.ToInt32(aRow["TASK_SEQUENCE"], CultureInfo.CurrentUICulture),
                                                                BATCH_COMP_RID: Convert.ToInt32(aRow["BATCH_COMP_RID"], CultureInfo.CurrentUICulture)
                                                                );
            }
            catch
            {
                throw;
            }
        }
        // End TT#1595-MD - stodd - Batch Comp

        private DataTable BuildUserListAsDataset(ArrayList aValueList)
        {
            DataTable dtUserList = null;
            if (aValueList != null)
            {
                dtUserList = new DataTable();
                dtUserList.Columns.Add("USER_RID", typeof(int));
                foreach (int userRID in aValueList)
                {
                    if (dtUserList.Select("USER_RID=" + userRID.ToString()).Length == 0) //ensure userRIDs are distinct, and only added to the datatable one time
                    {
                        DataRow dr = dtUserList.NewRow();
                        dr["USER_RID"] = userRID;
                        dtUserList.Rows.Add(dr);
                    }
                }
            }
            return dtUserList;
        }

		private DataRow NewScheduleRow()
		{
			try
			{
                return StoredProcedures.MID_SCHEDULE_READ_NEW.Read(_dba).NewRow();
			}
			catch
			{
				throw;
			}
		}

		private DataRow NewJobRow()
		{
			try
			{
                return StoredProcedures.MID_JOB_READ_NEW.Read(_dba).NewRow();
			}
			catch
			{
				throw;
			}
		}

		private DataRow NewSpecialRequestRow()
		{
			try
			{
                return StoredProcedures.MID_SPECIAL_REQUEST_JOB_READ_NEW.Read(_dba).NewRow();
			}
			catch
			{
				throw;
			}
		}

		private DataRow NewScheduleJobJoinRow()
		{
			try
			{
                return StoredProcedures.MID_SCHEDULE_JOB_JOIN_READ_NEW.Read(_dba).NewRow();
			}
			catch
			{
				throw;
			}
		}

		private DataRow NewTaskListRow()
		{
			try
			{
                return StoredProcedures.MID_TASKLIST_READ_NEW.Read(_dba).NewRow();
			}
			catch
			{
				throw;
			}
		}

		private DataRow NewJobTaskListJoinRow()
		{
			try
			{
                return StoredProcedures.MID_JOB_TASKLIST_JOIN_READ_NEW.Read(_dba).NewRow();
			}
			catch
			{
				throw;
			}
		}
	}
}
