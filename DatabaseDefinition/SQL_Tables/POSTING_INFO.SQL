--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[POSTING_INFO] ( 
	"PROCESS_RID" int not null,
	"CH_DAY_HIS_RECS" int default 0 null,
	"CH_WK_HIS_RECS" int default 0 null,
	"CH_WK_FOR_RECS" int default 0 null,
	"ST_DAY_HIS_RECS" int default 0 null,
	"ST_WK_HIS_RECS" int default 0 null,
	"ST_WK_FOR_RECS" int default 0 null,
	"RECS_WITH_ERRORS" int default 0 null,
	"INTRANSIT_RECS" int default 0 null,
	"NODES_ADDED" int null) ON 'AUDIT'  

go
