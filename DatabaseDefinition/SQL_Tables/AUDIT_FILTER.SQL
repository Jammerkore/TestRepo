--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[AUDIT_FILTER] ( 
	"USER_RID" int not null,
	"RUN_DATE_TYPE" int null,
	"RUN_DATE_BETWEEN_FROM" int null,
	"RUN_DATE_BETWEEN_TO" int null,
	"RUN_DATE_FROM" smalldatetime null,
	"RUN_DATE_TO" smalldatetime null,
	"PROCESS_HIGHEST_MESSAGE_LEVEL" int null,
	"DETAIL_HIGHEST_MESSAGE_LEVEL" int null,
	"DURATION" int null,
	"SHOW_RUNNING" char(1) null,
	"SHOW_COMPLETED" char(1) null,
	"SHOW_MY_TASKS_ONLY" char(1) null)  

go
