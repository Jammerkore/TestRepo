--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[PROC_RPT] ( 
	"PROCESS_RID" int not null,
	"TIME_STAMP" datetime default '01/JAN/2003' not null,
	"REPORTING_MODULE" varchar(250) not null,
	"LINE_NUMBER" int null,
	"MESSAGE_LEVEL" int null,
	"MESSAGE_CODE" int null,
	"REPORT_MESSAGE" varchar(5000) null) ON 'AUDIT'  

go
