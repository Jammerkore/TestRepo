--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[HEADER_LOAD_INFO] ( 
	"PROCESS_RID" int not null,
	"RECS_READ" int null,
	"RECS_WITH_ERRORS" int null,
	"HDRS_CREATED" int null,
	"HDRS_MODIFIED" int null,
	"HDRS_REMOVED" int null,
	"HDRS_RESET" int null) ON 'AUDIT'  

go
