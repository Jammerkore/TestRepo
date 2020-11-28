--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[COMPUTATION_DRIVER_INFO] ( 
	"PROCESS_RID" int not null,
	"TOTAL_ITEMS" int not null,
	"CONCURRENT_PROCESSES" int not null,
	"RECS_WITH_ERRORS" int not null) ON 'AUDIT'  

go
