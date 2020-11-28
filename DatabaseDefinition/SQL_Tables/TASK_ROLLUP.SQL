--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[TASK_ROLLUP] ( 
	"TASK_SEQUENCE" int not null,
	"TASKLIST_RID" int not null,
	"ROLLUP_SEQUENCE" char(10) not null,
	"HN_RID" int null,
	"FV_RID" int null,
	"ROLLUP_CDR_RID" int null,
	"FROM_PH_OFFSET_IND" int not null,
	"FROM_PH_RID" int null,
	"FROM_PHL_SEQUENCE" int null,
	"FROM_OFFSET" int null,
	"TO_PH_OFFSET_IND" int null,
	"TO_PH_RID" int null,
	"TO_PHL_SEQUENCE" int null,
	"TO_OFFSET" int null,
	"DAY_TO_WEEK_IND" char(1) not null,
	"DAY_IND" char(1) not null,
	"WEEK_IND" char(1) not null,
	"STORE_IND" char(1) not null,
	"CHAIN_IND" char(1) not null,
	"STORE_TO_CHAIN_IND" char(1) default '0' not null,
	"HIERARCHY_LEVELS_IND" char(1) default '1' not null,
	"POSTING_IND" char(1) default '0' not null,
	"INTRANSIT_IND" char(1) null,
	"RECLASS_IND" char(1) null)  

go
