--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#795 -JScott -STORE_INTRANSIT_REV table is not being purged
--dv History: TT#269-MD -JSmith -Modify Purge to use header weeks when purging daily percentages
--dv History: TT#739-MD -STodd -delete stores
--dv History:		3/6/2015 TT#1576-MD -jsobek -Data Layer Request - Add VSW Revision Records to Purge
--dv =============================================     
create table [dbo].[PURGE_INFO] ( 
	"PROCESS_RID" int not null,
	"STORE_DAILY_HISTORY" int not null,
	"CHAIN_WEEKLY_HISTORY" int not null,
	"STORE_WEEKLY_HISTORY" int not null,
	"CHAIN_WEEKLY_FORECAST" int not null,
	"STORE_WEEKLY_FORECAST" int not null,
	"HEADERS" int not null,
	"INTRANSIT" int null, 
	"INTRANSIT_REV" int null,
	"USERS" int null,
	"GROUPS" int null,
	"DAILY_PERCENTAGES" int null,         
	"EMPTY_ATTRIBUTE_SETS" int null,
	"AUDITS" int null,
	"IMO_REV" int null
	) ON 'AUDIT' 

go
