--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[SIZE_CODE_LOAD_INFO] ( 
	"PROCESS_RID" int not null,
	"RECS_READ" int null,
	"RECS_WITH_ERRORS" int null,
	"CODES_CREATED" int null,
	"CODES_MODIFIED" int null) ON 'AUDIT'  

go
