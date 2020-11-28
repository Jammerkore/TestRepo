--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#106 MD - JSmith - Number of Nodes Added needs to be included in the Audit > Summary of the Hierarchy Load API
--dv =============================================   
create table [dbo].[HIER_LOAD_INFO] ( 
	"PROCESS_RID" int not null,
	"HIER_RECS" int null,
	"LEVEL_RECS" int null,
	"MERCH_RECS" int null,
	"RECS_WITH_ERRORS" int null,
	"MOVE_RECS" int null,
	"RENAME_RECS" int null,
	"DELETE_RECS" int null,                
	"MERCH_ADDED" int null,                
	"MERCH_UPDATED" int null) ON 'AUDIT'   

go
