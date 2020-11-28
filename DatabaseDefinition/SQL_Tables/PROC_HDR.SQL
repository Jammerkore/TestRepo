--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[PROC_HDR] ( 
	"PROCESS_RID" int identity not null,
	"PROCESS_ID" int not null,
	"COMPLETION_STATUS_CODE" int default (800500) not null,
	"EXECUTION_STATUS_CODE" int default 801600 not null,
	"START_TIME" datetime not null,
	"STOP_TIME" datetime null,
	"SUMMARY_CODE" int not null,
	"USER_RID" int null,
	"HIGHEST_LEVEL" int null,
	"PROC_DESC" char(250) null) ON 'AUDIT'  

go
