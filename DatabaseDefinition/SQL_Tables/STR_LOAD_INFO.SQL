--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#739-MD -STodd -delete stores
--dv =============================================  
create table [dbo].[STR_LOAD_INFO] ( 
	"PROCESS_RID" int not null,
	"STORE_RECS" int null,
	"RECS_WITH_ERRORS" int null,
	"STORES_CREATED" int null,
	"STORES_MODIFIED" int null,
	"STORES_DELETED" int null,
	"STORES_RECOVERED" int null) ON 'AUDIT' 

go
