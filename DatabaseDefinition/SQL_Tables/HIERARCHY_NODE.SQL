--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[HIERARCHY_NODE] ( 
	"HN_RID" int identity not null,
	"HN_TYPE" int not null,
	"HOME_PH_RID" int not null,
	"HOME_LEVEL" int not null,
	"OTS_PLANLEVEL_TYPE" int null,
	"USE_BASIC_REPLENISHMENT" char(1) null,
	"OTS_FORECAST_LEVEL_SELECT_TYPE" int null,
	"OTS_FORECAST_LEVEL_TYPE" int null,
	"OTS_FORECAST_LEVEL_PH_RID" int null,
	"OTS_FORECAST_LEVEL_PHL_SEQUENCE" int null,
	"OTS_FORECAST_LEVEL_ANCHOR_NODE" int null,
	"OTS_FORECAST_LEVEL_MASK_FIELD" int null,
	"OTS_FORECAST_LEVEL_MASK" varchar(50) null,
	"STOCK_MIN_MAX_SG_RID" int null,
	"VIRTUAL_IND" char(1) null,
	"PURPOSE" int null,
	"ACTIVE_IND" char(1) null,
	--"INACTIVE_DATE" smalldatetime null  -- TT#3630 - JSmith - Delete My Hierarchy
	"INACTIVE_DATE" smalldatetime null,   -- TT#3630 - JSmith - Delete My Hierarchy
	"NODE_DELETE_IND" char(1) null)       -- TT#3630 - JSmith - Delete My Hierarchy  

go
