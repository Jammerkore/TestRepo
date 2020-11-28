--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================     
create table [dbo].[ROLLUP_ITEM] ( 
	"PROCESS" int not null,
	"HN_RID" int not null,
	"TIME_ID" int not null,
	"FV_RID" int not null,
	"ITEM_TYPE" int not null,
	"PH_RID" int not null,
	"HOME_LEVEL" int not null,
	"FIRST_DAY_OF_WEEK" int default 0 not null,
	"LAST_DAY_OF_WEEK" int default 0 not null,
	"BATCH_NUMBER" int null,
	"FIRST_DAY_OF_NEXT_WEEK" int null,
	"HN_MOD" int null,
	"GENERATED_ITEM" char(1) null,
	"ITEM_PROCESSED" char(1) null,
	"ALTERNATES_ONLY" char(1) null)  

go
