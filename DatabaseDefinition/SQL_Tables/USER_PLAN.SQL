--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#609-MD -jsobek -OTS Forecast Chain Ladder View
--dv History: TT#639-MD -agallagher -OTS Forecast Totals Right
--dv =============================================   
create table [dbo].[USER_PLAN] ( 
	"USER_RID" int not null,
	"SG_RID" int null,
	"FILTER_RID" int null,
	"GROUP_BY_ID" int null,
	"VIEW_RID" int null,
	"STORE_HN_RID" int null,
	"STORE_FV_RID" int null,
	"TIME_PERIOD_CDR_RID" int null,
	"DISPLAY_TIME_BY_ID" int null,
	"CHAIN_HN_RID" int null,
	"CHAIN_FV_RID" int null,
	"INCLUDE_INELIGIBLE_STORES_IND" char(1) null,
	"INCLUDE_SIMILAR_STORES_IND" char(1) null,
	"SESSION_TYPE" int null,
	"LOW_LEVEL_FV_RID" int null,
	"LOW_LEVEL_TYPE" int null,
	"LOW_LEVEL_OFFSET" int null,
	"LOW_LEVEL_SEQUENCE" int null,
	"CALC_MODE" varchar(50) null,
	"OLL_RID" int null,
	"CUSTOM_OLL_RID" int null,
	"IS_LADDER" int null, 
	"TOT_RIGHT" int null, 
	"IS_MULTI" int null) 

go
