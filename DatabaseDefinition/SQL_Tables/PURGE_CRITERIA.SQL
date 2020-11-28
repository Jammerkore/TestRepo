--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[PURGE_CRITERIA] ( 
	"HN_RID" int not null,
	"PURGE_DAILY_HISTORY_TIMEFRAME" int null,
	"PURGE_DAILY_HISTORY" int null,
	"PURGE_WEEKLY_HISTORY_TIMEFRAME" int null,
	"PURGE_WEEKLY_HISTORY" int null,
	"PURGE_PLANS_TIMEFRAME" int null,
	"PURGE_PLANS" int null)  

go
