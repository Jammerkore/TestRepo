using System;
using System.Data;
using System.Globalization;

namespace MIDRetail.DataCommon
{
	/// <summary>
	/// This class defines the JobProfile, information about a Job.
	/// </summary>

	[Serializable]
	public class JobProfile : Profile, ICloneable
	{
		//=======
		// FIELDS
		//=======

		private string _name;
		private bool _systemGen;

		//=============
		// CONSTRUCTORS
		//=============

		public JobProfile(int aKey)
			: base(aKey)
		{
			_name = "";
			_systemGen = false;
		}

		public JobProfile(DataRow aRow)
			: base(-1)
		{
			try
			{
				LoadFromDataRow(aRow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public JobProfile(int aKey, string aName, bool aSystemGenerated)
			: base(aKey)
		{
			_name = aName;
			_systemGen = aSystemGenerated;
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Job;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public bool SystemGenerated
		{
			get
			{
				return _systemGen;
			}
			set
			{
				_systemGen = value;
			}
		}

		//========
		// METHODS
		//========

		public object Clone()
		{
			try
			{
				return new JobProfile(_key, _name, _systemGen);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public string GetUniqueName()
		{
			try
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//return _name + System.DateTime.Now.ToString(" (MM/dd/yy hh:mm:ss.fff)");
				return _name + System.DateTime.Now.ToUniversalTime().ToString(" (MM/dd/yy hh:mm:ss.fff UTC)");
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void LoadFromDataRow(DataRow aRow)
		{
			try
			{
				_key = Convert.ToInt32(aRow["JOB_RID"], CultureInfo.CurrentUICulture);
				_name = Convert.ToString(aRow["JOB_NAME"], CultureInfo.CurrentUICulture);
				_systemGen = aRow["SYSTEM_GENERATED_IND"].ToString() == "1";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataRow UnloadToDataRow(DataRow aRow)
		{
			try
			{
				aRow["JOB_RID"] = _key;
				aRow["JOB_NAME"] = _name;
				aRow["SYSTEM_GENERATED_IND"] = (_systemGen) ? "1" : "0";

				return aRow;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// This class defines the ScheduleProfile, information about a Schedule.
	/// </summary>

	[Serializable]
	public class ScheduleProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private string _name;

		private DateTime _startTime;

		private eScheduleByType _schedByType;
		private int _schedByInterval;
		private DaysInWeek _schedByDaysInWeek;
		private eScheduleByMonthWeekType _schedByMonthsWeekType;

		private DateTime _startDateRange;
		private bool _endDate;
		private DateTime _endDateRange;

		private int _repeatInterval;
		private eScheduleRepeatIntervalType _repeatIntervalType;
		private bool _repeatUntil;
		private DateTime _repeatUntilTime;
		private bool _repeatDuration;
		private int _repeatDurationHours;
		private int _repeatDurationMinutes;
		private bool _terminateAfterConditionMet;
		private bool _repeatUntilSuccessful;

		private eScheduleConditionType _conditionType;
		private string _conditionTriggerDirectory;
		private string _conditionTriggerSuffix;

		//=============
		// CONSTRUCTORS
		//=============

		public ScheduleProfile(int aKey)
			: base(aKey)
		{
			try
			{
				InitFields();
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ScheduleProfile(DataRow aDataRow)
			: base(-1)
		{
			try
			{
				LoadFromDataRow(aDataRow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public ScheduleProfile(int aKey, string aName)
			: base(aKey)
		{
			try
			{
				InitFields();

				_name = aName;
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

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.Schedule;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public DateTime StartTime
		{
			get
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//return _startTime;
				return _startTime.ToLocalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
			set
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_startTime = value;
				_startTime = value.ToUniversalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
		}

		public eScheduleByType ScheduleByType
		{
			get
			{
				return _schedByType;
			}
			set
			{
				_schedByType = value;
			}
		}

		public int ScheduleByInterval
		{
			get
			{
				return _schedByInterval;
			}
			set
			{
				_schedByInterval = value;
			}
		}

		public DaysInWeek ScheduleByDaysInWeek
		{
			get
			{
				return _schedByDaysInWeek;
			}
			set
			{
				_schedByDaysInWeek = value;
			}
		}

		public eScheduleByMonthWeekType ScheduleByMonthWeekType
		{
			get
			{
				return _schedByMonthsWeekType;
			}
			set
			{
				_schedByMonthsWeekType = value;
			}
		}

		public DateTime StartDateRange
		{
			get
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//return _startDateRange;
				return _startDateRange.ToLocalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
			set
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_startDateRange = value;
				_startDateRange = value.ToUniversalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
		}

		public bool EndDate
		{
			get
			{
				return _endDate;
			}
			set
			{
				_endDate = value;
			}
		}

		public DateTime EndDateRange
		{
			get
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//return _endDateRange;
				return _endDateRange.ToLocalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
			set
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_endDateRange = value;
				_endDateRange = value.ToUniversalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
		}

		public int RepeatInterval
		{
			get
			{
				return _repeatInterval;
			}
			set
			{
				_repeatInterval = value;
			}
		}

		public eScheduleRepeatIntervalType RepeatIntervalType
		{
			get
			{
				return _repeatIntervalType;
			}
			set
			{
				_repeatIntervalType = value;
			}
		}

		public bool RepeatUntil
		{
			get
			{
				return _repeatUntil;
			}
			set
			{
				_repeatUntil = value;
			}
		}

		public DateTime RepeatUntilTime
		{
			get
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//return _repeatUntilTime;
				return _repeatUntilTime.ToLocalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
			set
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_repeatUntilTime = value;
				_repeatUntilTime = value.ToUniversalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
		}

		public bool RepeatDuration
		{
			get
			{
				return _repeatDuration;
			}
			set
			{
				_repeatDuration = value;
			}
		}

		public int RepeatDurationHours
		{
			get
			{
				return _repeatDurationHours;
			}
			set
			{
				_repeatDurationHours = value;
			}
		}

		public int RepeatDurationMinutes
		{
			get
			{
				return _repeatDurationMinutes;
			}
			set
			{
				_repeatDurationMinutes = value;
			}
		}

		public bool TerminateAfterConditionMet
		{
			get
			{
				return _terminateAfterConditionMet;
			}
			set
			{
				_terminateAfterConditionMet = value;
			}
		}

		public bool RepeatUntilSuccessful
		{
			get
			{
				return _repeatUntilSuccessful;
			}
			set
			{
				_repeatUntilSuccessful = value;
			}
		}

		public eScheduleConditionType ConditionType
		{
			get
			{
				return _conditionType;
			}
			set
			{
				_conditionType = value;
			}
		}

		public string ConditionTriggerDirectory
		{
			get
			{
				return _conditionTriggerDirectory;
			}
			set
			{
				_conditionTriggerDirectory = value;
			}
		}

		public string ConditionTriggerSuffix
		{
			get
			{
				return _conditionTriggerSuffix;
			}
			set
			{
				_conditionTriggerSuffix = value;
			}
		}

		//========
		// METHODS
		//========

		private void InitFields()
		{
			DateTime currDateTime;

			try
			{
				currDateTime = DateTime.Now;
				currDateTime = new DateTime(currDateTime.Year, currDateTime.Month, currDateTime.Day, currDateTime.Hour, currDateTime.Minute, 0);

				_name = "";
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_startTime = currDateTime;
				_startTime = currDateTime.ToUniversalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
				_schedByType = eScheduleByType.Once;
				_schedByInterval = 0;
				_schedByDaysInWeek = new DaysInWeek();
				_schedByMonthsWeekType = eScheduleByMonthWeekType.None;
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_startDateRange = currDateTime;
				_startDateRange = currDateTime.ToUniversalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
				_endDate = false;
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_endDateRange = currDateTime;
				_endDateRange = currDateTime.ToUniversalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
				_repeatInterval = 0;
				_repeatIntervalType = eScheduleRepeatIntervalType.None;
				_repeatUntil = false;
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_repeatUntilTime = currDateTime;
				_repeatUntilTime = currDateTime.ToUniversalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
				_repeatDuration = false;
				_repeatDurationHours = 0;
				_repeatDurationMinutes = 0;
				_terminateAfterConditionMet = false;
				_repeatUntilSuccessful = false;
				_conditionType = eScheduleConditionType.None;
				_conditionTriggerDirectory = "";
				_conditionTriggerSuffix = "";
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void LoadFromDataRow(DataRow aRow)
		{
			try
			{
				_key = Convert.ToInt32(aRow["SCHED_RID"], CultureInfo.CurrentUICulture);
				_name = Convert.ToString(aRow["SCHED_NAME"], CultureInfo.CurrentUICulture);
				_startTime = Convert.ToDateTime(aRow["START_TIME"], CultureInfo.CurrentUICulture);
				_schedByType = (eScheduleByType)Convert.ToInt32(aRow["SCHEDULE_BY_TYPE"]);
				_schedByInterval = Convert.ToInt32(aRow["SCHEDULE_BY_INTERVAL"], CultureInfo.CurrentUICulture);
				_schedByDaysInWeek = new DaysInWeek(
					aRow["SCHEDULE_ON_SUNDAY_IND"].ToString() == "1",
					aRow["SCHEDULE_ON_MONDAY_IND"].ToString() == "1",
					aRow["SCHEDULE_ON_TUESDAY_IND"].ToString() == "1",
					aRow["SCHEDULE_ON_WEDNESDAY_IND"].ToString() == "1",
					aRow["SCHEDULE_ON_THURSDAY_IND"].ToString() == "1",
					aRow["SCHEDULE_ON_FRIDAY_IND"].ToString() == "1",
					aRow["SCHEDULE_ON_SATURDAY_IND"].ToString() == "1");
				_schedByMonthsWeekType = (eScheduleByMonthWeekType)Convert.ToInt32(aRow["SCHEDULE_BY_MONTH_WEEK_TYPE"], CultureInfo.CurrentUICulture);
				_startDateRange = Convert.ToDateTime(aRow["START_DATE_RANGE"], CultureInfo.CurrentUICulture);
				_endDate = aRow["END_DATE_IND"].ToString() == "1";
				_endDateRange = Convert.ToDateTime(aRow["END_DATE_RANGE"], CultureInfo.CurrentUICulture);
				_repeatInterval = Convert.ToInt32(aRow["REPEAT_INTERVAL"], CultureInfo.CurrentUICulture);
				_repeatIntervalType = (eScheduleRepeatIntervalType)Convert.ToInt32(aRow["REPEAT_INTERVAL_TYPE"]);
				_repeatUntil = aRow["REPEAT_UNTIL_IND"].ToString() == "1";
				_repeatUntilTime = Convert.ToDateTime(aRow["REPEAT_UNTIL_TIME"], CultureInfo.CurrentUICulture);
				_repeatDuration = aRow["REPEAT_DURATION_IND"].ToString() == "1";
				_repeatDurationHours = Convert.ToInt32(aRow["REPEAT_DURATION_HOURS"], CultureInfo.CurrentUICulture);
				_repeatDurationMinutes = Convert.ToInt32(aRow["REPEAT_DURATION_MINUTES"], CultureInfo.CurrentUICulture);
				_terminateAfterConditionMet = aRow["TERMINATE_AFTER_COND_MET_IND"].ToString() == "1";
				_repeatUntilSuccessful = aRow["REPEAT_UNTIL_SUCCESSFUL_IND"].ToString() == "1";
				_conditionType = (eScheduleConditionType)Convert.ToInt32(aRow["CONDITION_TYPE"]);
				_conditionTriggerDirectory = Convert.ToString(aRow["CONDITION_TRIGGER_DIRECTORY"], CultureInfo.CurrentUICulture);
				_conditionTriggerSuffix = Convert.ToString(aRow["CONDITION_TRIGGER_SUFFIX"], CultureInfo.CurrentUICulture);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataRow UnloadToDataRow(DataRow aRow)
		{
			try
			{
				aRow["SCHED_RID"] = _key;
				aRow["SCHED_NAME"] = _name;
				aRow["START_TIME"] = _startTime;
				aRow["SCHEDULE_BY_TYPE"] = (int)_schedByType;
				aRow["SCHEDULE_BY_INTERVAL"] = _schedByInterval;
				aRow["SCHEDULE_ON_MONDAY_IND"] = (_schedByDaysInWeek.Monday) ? "1" : "0";
				aRow["SCHEDULE_ON_TUESDAY_IND"] = (_schedByDaysInWeek.Tuesday) ? "1" : "0";
				aRow["SCHEDULE_ON_WEDNESDAY_IND"] = (_schedByDaysInWeek.Wednesday) ? "1" : "0";
				aRow["SCHEDULE_ON_THURSDAY_IND"] = (_schedByDaysInWeek.Thursday) ? "1" : "0";
				aRow["SCHEDULE_ON_FRIDAY_IND"] = (_schedByDaysInWeek.Friday) ? "1" : "0";
				aRow["SCHEDULE_ON_SATURDAY_IND"] = (_schedByDaysInWeek.Saturday) ? "1" : "0";
				aRow["SCHEDULE_ON_SUNDAY_IND"] = (_schedByDaysInWeek.Sunday) ? "1" : "0";
				aRow["SCHEDULE_BY_MONTH_WEEK_TYPE"] = _schedByMonthsWeekType;
				aRow["START_DATE_RANGE"] = _startDateRange;
				aRow["END_DATE_IND"] = (_endDate) ? "1" : "0";
				aRow["END_DATE_RANGE"] = _endDateRange;
				aRow["REPEAT_INTERVAL"] = _repeatInterval;
				aRow["REPEAT_INTERVAL_TYPE"] = _repeatIntervalType;
				aRow["REPEAT_UNTIL_IND"] = (_repeatUntil) ? "1" : "0";
				aRow["REPEAT_UNTIL_TIME"] = _repeatUntilTime;
				aRow["REPEAT_DURATION_IND"] = (_repeatDuration) ? "1" : "0";
				aRow["REPEAT_DURATION_HOURS"] = _repeatDurationHours;
				aRow["REPEAT_DURATION_MINUTES"] = _repeatDurationMinutes;
				aRow["TERMINATE_AFTER_COND_MET_IND"] = (_terminateAfterConditionMet) ? "1" : "0";
				aRow["REPEAT_UNTIL_SUCCESSFUL_IND"] = (_repeatUntilSuccessful) ? "1" : "0";
				aRow["CONDITION_TYPE"] = _conditionType;
				aRow["CONDITION_TRIGGER_DIRECTORY"] = _conditionTriggerDirectory;
				aRow["CONDITION_TRIGGER_SUFFIX"] = _conditionTriggerSuffix;

				return aRow;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// This class defines the ScheduleProfile, information about a TaskList.
	/// </summary>

	[Serializable]
	public class TaskListProfile : Profile, ICloneable
	{
		//=======
		// FIELDS
		//=======

		private string _name;
		private int _userRID;
		private bool _systemGen;
		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		private int _ownerUserRID;
		//End Track #4815

		//=============
		// CONSTRUCTORS
		//=============

		public TaskListProfile(int aKey)
			: base(aKey)
		{
			_name = "";
			_userRID = 0;
			_systemGen = false;
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			_ownerUserRID = 0;
			//End Track #4815
		}

		public TaskListProfile(DataRow aRow)
			: base(-1)
		{
			try
			{
				LoadFromDataRow(aRow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//		private TaskListProfile(int aKey, string aName, int aUserRID, bool aSystemGenerated)
		private TaskListProfile(int aKey, string aName, int aUserRID, bool aSystemGenerated, int aOwnerUserRID)
		//End Track #4815
			: base(aKey)
		{
			_name = aName;
			_userRID = aUserRID;
			_systemGen = aSystemGenerated;
			//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
			_ownerUserRID = aOwnerUserRID;
			//End Track #4815
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.TaskList;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public int UserRID
		{
			get
			{
				return _userRID;
			}
			set
			{
				_userRID = value;
			}
		}

		public bool SystemGenerated
		{
			get
			{
				return _systemGen;
			}
			set
			{
				_systemGen = value;
			}
		}

		//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
		/// <summary>
		/// Gets or sets the key of the owner of the Tasklist.
		/// </summary>
		public int OwnerUserRID
		{
			get { return _ownerUserRID ; }
			set { _ownerUserRID = value; }
		}
		//End Track #4815

		//========
		// METHODS
		//========

		public object Clone()
		{
			try
			{
				//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
//				return new TaskListProfile(_key, _name, _userRID, _systemGen);
				return new TaskListProfile(_key, _name, _userRID, _systemGen, _ownerUserRID);
				//End Track #4815
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public string GetUniqueName()
		{
			try
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//return _name + System.DateTime.Now.ToString(" (MM/dd/yy hh:mm:ss.fff)");
				return _name + System.DateTime.Now.ToUniversalTime().ToString(" (MM/dd/yy hh:mm:ss.fff UTC)");
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void LoadFromDataRow(DataRow aRow)
		{
			try
			{
				_key = Convert.ToInt32(aRow["TASKLIST_RID"], CultureInfo.CurrentUICulture);
				_name = Convert.ToString(aRow["TASKLIST_NAME"], CultureInfo.CurrentUICulture);
				_userRID = Convert.ToInt32(aRow["USER_RID"], CultureInfo.CurrentUICulture);
				_systemGen = aRow["SYSTEM_GENERATED_IND"].ToString() == "1";
				//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
				_ownerUserRID = Convert.ToInt32(aRow["OWNER_USER_RID"], CultureInfo.CurrentUICulture);
				//End Track #4815
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataRow UnloadToDataRow(DataRow aRow)
		{
			try
			{
				aRow["TASKLIST_RID"] = _key;
				aRow["TASKLIST_NAME"] = _name;
				aRow["USER_RID"] = _userRID;
				aRow["SYSTEM_GENERATED_IND"] = (_systemGen) ? "1" : "0";
				//Begin Track #4815 - JSmith - #283-User (Security) Maintenance
				aRow["USER_RID"] = _ownerUserRID;
				//End Track #4815

				return aRow;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// This class defines the JobTaskListJoinProfile, information that connects a Job to a TaskList.
	/// </summary>

	[Serializable]
	public class JobTaskListJoinProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private int _taskListKey;
		private int _taskListSeq;

		//=============
		// CONSTRUCTORS
		//=============

		public JobTaskListJoinProfile(int aJobKey, int aTaskListKey, int aTaskListSequence)
			: base(aJobKey)
		{
			_taskListKey = aTaskListKey;
			_taskListSeq = aTaskListSequence;
		}

		public JobTaskListJoinProfile(DataRow aRow)
			: base(-1)
		{
			try
			{
				LoadFromDataRow(aRow);
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

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.ScheduleJobJoin;
			}
		}

		private int JobKey
		{
			get
			{
				return _key;
			}
		}

		private int TaskListKey
		{
			get
			{
				return _taskListKey;
			}
		}

		private int TaskListSequence
		{
			get
			{
				return _taskListSeq;
			}
		}

		//========
		// METHODS
		//========

		private void LoadFromDataRow(DataRow aRow)
		{
			try
			{
				_key = Convert.ToInt32(aRow["JOB_RID"], CultureInfo.CurrentUICulture);
				_taskListKey = Convert.ToInt32(aRow["TASKLIST_RID"], CultureInfo.CurrentUICulture);
				_taskListSeq = Convert.ToInt32(aRow["TASKLIST_SEQUENCE"], CultureInfo.CurrentUICulture);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataRow UnloadToDataRow(DataRow aRow)
		{
			try
			{
				aRow["JOB_RID"] = _key;
				aRow["TASKLIST_RID"] = _taskListKey;
				aRow["TASKLIST_SEQUENCE"] = _taskListSeq;

				return aRow;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	/// <summary>
	/// This class defines the ScheduleJobJoinProfile, information that connects a Schedule to a Job.
	/// </summary>

	[Serializable]
	public class ScheduleJobJoinProfile : Profile
	{
		//=======
		// FIELDS
		//=======

		private int _jobKey;
		private int _userKey;
		private eProcessExecutionStatus _execStat;
		private eProcessCompletionStatus _lastCompStat;
		private DateTime _lastRunDateTime;
		private DateTime _lastCompDateTime;
		private DateTime _nextRunDateTime;
		private DateTime _repeatUntilDateTime;

		//=============
		// CONSTRUCTORS
		//=============

		public ScheduleJobJoinProfile(int aSchedKey, int aJobKey, int aUserKey)
			: base(aSchedKey)
		{
			_jobKey = aJobKey;
			_userKey = aUserKey;
			_execStat = eProcessExecutionStatus.None;
			_lastCompStat = eProcessCompletionStatus.None;
			_lastRunDateTime = System.DateTime.MinValue;
			_lastCompDateTime = System.DateTime.MinValue;
			_nextRunDateTime = System.DateTime.MinValue;
			_repeatUntilDateTime = System.DateTime.MinValue;
		}

		public ScheduleJobJoinProfile(DataRow aRow)
			: base(-1)
		{
			try
			{
				LoadFromDataRow(aRow);
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

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.ScheduleJobJoin;
			}
		}

		private int SchedKey
		{
			get
			{
				return _key;
			}
		}

		private int JobKey
		{
			get
			{
				return _jobKey;
			}
		}

		private int UserKey
		{
			get
			{
				return _userKey;
			}
		}

		private eProcessExecutionStatus ExecutionStatus
		{
			get
			{
				return _execStat;
			}
			set
			{
				_execStat = value;
			}
		}

		private eProcessCompletionStatus LastCompletionStatus
		{
			get
			{
				return _lastCompStat;
			}
			set
			{
				_lastCompStat = value;
			}
		}

		private DateTime LastRunDateTime
		{
			get
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//return _lastRunDateTime;
				return _lastRunDateTime.ToLocalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
			set
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_lastRunDateTime = value;
				_lastRunDateTime = value.ToUniversalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
		}

		private DateTime LastCompletionDateTime
		{
			get
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//return _lastCompDateTime;
				return _lastCompDateTime.ToLocalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
			set
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_lastCompDateTime = value;
				_lastCompDateTime = value.ToUniversalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
		}

		private DateTime NextRunDateTime
		{
			get
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//return _nextRunDateTime;
				return _nextRunDateTime.ToLocalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
			set
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_nextRunDateTime = value;
				_nextRunDateTime = value.ToUniversalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
		}

		private DateTime RepeatUntilDateTime
		{
			get
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//return _repeatUntilDateTime;
				return _repeatUntilDateTime.ToLocalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
			set
			{
				//Begin Track #398 - JScott - Scheduler does not recognize different time zones
				//_repeatUntilDateTime = value;
				_repeatUntilDateTime = value.ToUniversalTime();
				//End Track #398 - JScott - Scheduler does not recognize different time zones
			}
		}

		//========
		// METHODS
		//========

		private void LoadFromDataRow(DataRow aRow)
		{
			try
			{
				_key = Convert.ToInt32(aRow["SCHED_RID"], CultureInfo.CurrentUICulture);
				_jobKey = Convert.ToInt32(aRow["JOB_RID"], CultureInfo.CurrentUICulture);
				_userKey = Convert.ToInt32(aRow["USER_RID"], CultureInfo.CurrentUICulture);
				_execStat = (eProcessExecutionStatus)Convert.ToInt32(aRow["EXECUTION_STATUS"], CultureInfo.CurrentUICulture);
				_lastCompStat = (eProcessCompletionStatus)Convert.ToInt32(aRow["LAST_COMPLETION_STATUS"], CultureInfo.CurrentUICulture);

				if (aRow["LAST_RUN_DATETIME"] != System.DBNull.Value)
				{
					_lastRunDateTime = Convert.ToDateTime(aRow["LAST_RUN_DATETIME"], CultureInfo.CurrentUICulture);
				}
				else
				{
					_lastRunDateTime = System.DateTime.MinValue;
				}

				if (aRow["LAST_COMPLETION_DATETIME"] != System.DBNull.Value)
				{
					_lastCompDateTime = Convert.ToDateTime(aRow["LAST_COMPLETION_DATETIME"], CultureInfo.CurrentUICulture);
				}
				else
				{
					_lastCompDateTime = System.DateTime.MinValue;
				}

				if (aRow["NEXT_RUN_DATETIME"] != System.DBNull.Value)
				{
					_nextRunDateTime = Convert.ToDateTime(aRow["NEXT_RUN_DATETIME"], CultureInfo.CurrentUICulture);
				}
				else
				{
					_nextRunDateTime = System.DateTime.MinValue;
				}

				if (aRow["REPEAT_UNTIL_DATETIME"] != System.DBNull.Value)
				{
					_repeatUntilDateTime = Convert.ToDateTime(aRow["REPEAT_UNTIL_DATETIME"], CultureInfo.CurrentUICulture);
				}
				else
				{
					_repeatUntilDateTime = System.DateTime.MinValue;
				}
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataRow UnloadToDataRow(DataRow aRow)
		{
			try
			{
				aRow["SCHED_RID"] = _key;
				aRow["JOB_RID"] = _jobKey;
				aRow["USER_RID"] = _userKey;
				aRow["EXECUTION_STATUS"] = _execStat;
				aRow["LAST_COMPLETION_STATUS"] = _lastCompStat;
				aRow["LAST_RUN_DATETIME"] = _lastRunDateTime;
				aRow["LAST_COMPLETION_DATETIME"] = _lastCompDateTime;
				aRow["NEXT_RUN_DATETIME"] = _nextRunDateTime;
				aRow["REPEAT_UNTIL_DATETIME"] = _repeatUntilDateTime;

				return aRow;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}

	// BEGIN Issue 5117 
	/// <summary>
	/// This class defines the SpecialRequestProfile, information about a Job.
	/// </summary>

	[Serializable]
	public class SpecialRequestProfile : Profile, ICloneable
	{
		//=======
		// FIELDS
		//=======

		private string _name;
		private int _concurrentJobs;

		//=============
		// CONSTRUCTORS
		//=============

		public SpecialRequestProfile(int aKey)
			: base(aKey)
		{
			_name = "";
			_concurrentJobs = 1;
		}

		public SpecialRequestProfile(DataRow aRow)
			: base(-1)
		{
			try
			{
				LoadFromDataRow(aRow);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public SpecialRequestProfile(int aKey, string aName, int concurrentJobs)
			: base(aKey)
		{
			_name = aName;
			_concurrentJobs = concurrentJobs;
		}

		//===========
		// PROPERTIES
		//===========

		override public eProfileType ProfileType
		{
			get
			{
				return eProfileType.SpecialRequest;
			}
		}

		public string Name
		{
			get
			{
				return _name;
			}
			set
			{
				_name = value;
			}
		}

		public int ConcurrentJobs
		{
			get
			{
				return _concurrentJobs;
			}
			set
			{
				_concurrentJobs = value;
			}
		}

		//========
		// METHODS
		//========

		public object Clone()
		{
			try
			{
				return new SpecialRequestProfile(_key, _name, _concurrentJobs);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		private void LoadFromDataRow(DataRow aRow)
		{
			try
			{
				_key = Convert.ToInt32(aRow["SPECIAL_REQ_RID"], CultureInfo.CurrentUICulture);
				_name = Convert.ToString(aRow["SPECIAL_REQ_NAME"], CultureInfo.CurrentUICulture);
				_concurrentJobs = Convert.ToInt32(aRow["CONCURRENT_PROCESSES"], CultureInfo.CurrentUICulture);
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}

		public DataRow UnloadToDataRow(DataRow aRow)
		{
			try
			{
				aRow["SPECIAL_REQ_RID"] = _key;
				aRow["SPECIAL_REQ_NAME"] = _name;
				aRow["CONCURRENT_PROCESSES"] = _concurrentJobs;

				return aRow;
			}
			catch (Exception exc)
			{
				string message = exc.ToString();
				throw;
			}
		}
	}
}
