--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1430 - JScott - Long Tasklist Name causes duplicate key on JOB table
--dv =============================================     
create table [dbo].[SCHEDULE] ( 
	"SCHED_RID" int identity not null,
	"SCHED_NAME" varchar(100) not null,
	"START_TIME" datetime null,
	"SCHEDULE_BY_TYPE" int null,
	"SCHEDULE_BY_INTERVAL" int null,
	"SCHEDULE_ON_MONDAY_IND" char(1) null,
	"SCHEDULE_ON_TUESDAY_IND" char(1) null,
	"SCHEDULE_ON_WEDNESDAY_IND" char(1) null,
	"SCHEDULE_ON_THURSDAY_IND" char(1) null,
	"SCHEDULE_ON_FRIDAY_IND" char(1) null,
	"SCHEDULE_ON_SATURDAY_IND" char(1) null,
	"SCHEDULE_ON_SUNDAY_IND" char(1) null,
	"SCHEDULE_BY_MONTH_WEEK_TYPE" int null,
	"START_DATE_RANGE" datetime null,
	"END_DATE_IND" char(1) null,
	"END_DATE_RANGE" datetime null,
	"REPEAT_INTERVAL" int null,
	"REPEAT_INTERVAL_TYPE" int null,
	"REPEAT_UNTIL_IND" char(1) null,
	"REPEAT_UNTIL_TIME" datetime null,
	"REPEAT_DURATION_IND" char(1) null,
	"REPEAT_DURATION_HOURS" int null,
	"REPEAT_DURATION_MINUTES" int null,
	"CONDITION_TYPE" int null,
	"CONDITION_TRIGGER_DIRECTORY" varchar(50) null,
	"CONDITION_TRIGGER_SUFFIX" varchar(50) null,
	"TERMINATE_AFTER_COND_MET_IND" char(1) null,
	"REPEAT_UNTIL_SUCCESSFUL_IND" char(1) null,
	"CREATED_BY_USER_RID" int null,
	"CREATED_DATETIME" datetime null,
	"LAST_MODIFIED_BY_USER_RID" int null,
	"LAST_MODIFIED_DATETIME" datetime null)  

go
