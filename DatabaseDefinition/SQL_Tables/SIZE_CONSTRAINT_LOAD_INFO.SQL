--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[SIZE_CONSTRAINT_LOAD_INFO] ( 
	"PROCESS_RID" int not null,
	"RECS_READ" int null,
	"RECS_WITH_ERRORS" int null,
	"MODELS_CREATED" int null,
	"MODELS_MODIFIED" int null,
	"MODELS_REMOVED" int null) ON 'AUDIT'  

go
