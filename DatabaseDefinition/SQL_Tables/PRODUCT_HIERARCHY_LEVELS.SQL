--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv Fields: PHL_SEQUENCE: Level_Sequence is used to control order of levels 
--dv History: 
--dv =============================================   
create table [dbo].[PRODUCT_HIERARCHY_LEVELS] ( 
	"PH_RID" int not null,
	"PHL_SEQUENCE" int not null,
	"PHL_ID" varchar(50) not null,
	"PHL_COLOR" varchar(30) null,
	"PHL_TYPE" int not null,
	"LENGTH_TYPE" int not null,
	"REQUIRED_SIZE" int null,
	"SIZE_RANGE_FROM" int null,
	"SIZE_RANGE_TO" int null,
	"OTS_PLANLEVEL_TYPE" int not null,
	"PHL_DISPLAY_OPTION_ID" int null,
	"PHL_ID_FORMAT" int null,
	"PURGE_DAILY_HISTORY_TIMEFRAME" int null,
	"PURGE_DAILY_HISTORY" int null,
	"PURGE_WEEKLY_HISTORY_TIMEFRAME" int null,
	"PURGE_WEEKLY_HISTORY" int null,
	"PURGE_PLANS_TIMEFRAME" int null,
	"PURGE_PLANS" int null)  

go
