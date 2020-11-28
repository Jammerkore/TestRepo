--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[OTS_PLAN] ( 
	"METHOD_RID" int not null,
	"PLAN_HN_RID" int null,
	"PLAN_FV_RID" int null,
	"CDR_RID" int null,
	"CHAIN_FV_RID" int null,
	"BAL_SALES_IND" char(1) null,
	"BAL_STOCK_IND" char(1) null,
	"FORECAST_MOD_RID" int null,
	"HIGH_LEVEL_IND" char(1) null,
	"LOW_LEVELS_IND" char(1) null,
	"LOW_LEVEL_TYPE" int null,
	"LOW_LEVEL_SEQ" int null,
	"LOW_LEVEL_OFFSET" int null,
	"OLL_RID" int null, 
	"TREND_OPTIONS_IND" char(1) null,  
	"TREND_OPTIONS_PLUG_CHAIN_WOS" float null)  

go
