--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[METHOD_EXPORT] ( 
	"METHOD_RID" int not null,
	"HN_RID" int not null,
	"FV_RID" int not null,
	"CDR_RID" int not null,
	"STORE_FILTER_RID" int null,
	"PLAN_TYPE" int not null,
	"LOW_LEVELS_IND" char(1) not null,
	"LOW_LEVELS_ONLY_IND" char(1) not null,
	"LOW_LEVELS_TYPE" int not null,
	"LOW_LEVEL_SEQUENCE" int not null,
	"LOW_LEVEL_OFFSET" int not null,
	"SHOW_INELIGIBLE_IND" char(1) not null,
	"USE_DEFAULT_SETTINGS_IND" char(1) not null,
	"PREINIT_VALUES_IND" char(1) not null,
	"EXCLUDE_ZERO_VALUES_IND" char(1) not null,
	"EXPORT_TYPE" int not null,
	"DELIMETER" char(1) null,
	"FILE_PATH" varchar(250) not null,
	"ADD_DATE_STAMP_IND" char(1) not null,
	"ADD_TIME_STAMP_IND" char(1) not null,
	"SPLIT_TYPE" int not null,
	"SPLIT_NUM_ENTRIES" int null,
	"CONCURRENT_PROCESSES" int null,
	"CREATE_FLAG_FILE_IND" char(1) not null,
	"FLAG_FILE_EXTENSION" varchar(20) null,
	"CREATE_END_FILE_IND" char(1) not null,
	"END_FILE_EXTENSION" varchar(20) null,
	"CSV_FILE_EXTENSION" varchar(20) null,
	"DATE_TYPE" int not null,
	"OLL_RID" int null)  

go
