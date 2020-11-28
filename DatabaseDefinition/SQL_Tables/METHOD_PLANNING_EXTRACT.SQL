--dv =============================================
--dv Modified date: 2/23/2018
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[METHOD_PLANNING_EXTRACT] ( 
	"METHOD_RID" int not null,
	"HN_RID" int not null,
	"FV_RID" int not null,
	"CDR_RID" int not null,
	"STORE_FILTER_RID" int null,
	"CHAIN_IND" char(1) not null,
	"STORE_IND" char(1) not null,
	"ATTRIBUTE_SET_IND" char(1) not null,
	"ATTRIBUTE_RID" int null,
	"LOW_LEVELS_IND" char(1) not null,
	"LOW_LEVELS_ONLY_IND" char(1) not null,
	"LOW_LEVELS_TYPE" int not null,
	"LOW_LEVEL_SEQUENCE" int not null,
	"LOW_LEVEL_OFFSET" int not null,
	"SHOW_INELIGIBLE_IND" char(1) not null,
	"EXCLUDE_ZERO_VALUES_IND" char(1) not null,
	"CONCURRENT_PROCESSES" int null,
	"OLL_RID" int null,
	"UPDATE_DATE" datetime null,
	"EXTRACT_DATE" datetime null)  

go
