--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================
create table [dbo].[SCHEDULE_JOB_JOIN] ( 
	"SCHED_RID" int not null,
	"JOB_RID" int not null,
	"USER_RID" int not null,
	"EXECUTION_STATUS" int null,
	"LAST_COMPLETION_STATUS" int null,
	"LAST_RUN_DATETIME" datetime null,
	"LAST_COMPLETION_DATETIME" datetime null,
	"NEXT_RUN_DATETIME" datetime null,
	"REPEAT_UNTIL_DATETIME" datetime null,
	"HOLD_BY_USER_RID" int null,
	"HOLD_BY_DATETIME" datetime null,
	"RELEASED_BY_USER_RID" int null,
	"RELEASED_BY_DATETIME" datetime null,
	"CURRENT_TASKLIST_RID" int null,
	"CURRENT_TASKLIST_SEQUENCE" int null)  

go
