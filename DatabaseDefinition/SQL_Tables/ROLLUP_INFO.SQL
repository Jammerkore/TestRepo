--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================     
create table [dbo].[ROLLUP_INFO] ( 
	"PROCESS_RID" int not null,
	"TOTAL_ITEMS" int null,
	"BATCH_SIZE" int null,
	"CONCURRENT_PROCESSES" int null,
	"TOTAL_BATCHES" int null,
	"RECS_WITH_ERRORS" int null) ON 'AUDIT'  

go
