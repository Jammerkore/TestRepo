--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#269-MD -JSmith -Modify Purge to use header weeks when purging daily percentages
--dv History: TT#400-MD -JSmith -Add Header Purge Criteria by Header Type
--dv =============================================     
create table [dbo].[PURGE_DATES] ( 
	"HN_RID" int not null,
	"PURGE_DAILY_HISTORY" int null,
	"PURGE_DAILY_HISTORY_BIN" int null,
	"PURGE_WEEKLY_HISTORY" int null,
	"PURGE_WEEKLY_HISTORY_BIN" int null,
	"PURGE_PLANS" int null,
	"HN_MOD" int null,
	"PURGE_DAILY_HISTORY_WEEKS" int null,
	"PURGE_WEEKLY_HISTORY_WEEKS" int null,
	"PURGE_PLANS_WEEKS" int null,
	"PURGE_HEADERS_RECEIPT_DATETIME" datetime null,
	"PURGE_HEADERS_ASN_DATETIME" datetime null,
	"PURGE_HEADERS_DUMMY_DATETIME" datetime null,
	"PURGE_HEADERS_DROPSHIP_DATETIME" datetime null,
	"PURGE_HEADERS_RESERVE_DATETIME" datetime null,
	"PURGE_HEADERS_WORKUPTOTALBUY_DATETIME" datetime null,
	"PURGE_HEADERS_PO_DATETIME" datetime null,
	"PURGE_HEADERS_VSW_DATETIME" datetime null,
	"PURGE_HEADERS_RECEIPT" int null,
	"PURGE_HEADERS_ASN" int null,
	"PURGE_HEADERS_DUMMY" int null,
	"PURGE_HEADERS_DROPSHIP" int null,
	"PURGE_HEADERS_RESERVE" int null,
	"PURGE_HEADERS_WORKUPTOTALBUY" int null,
	"PURGE_HEADERS_PO" int null,
	"PURGE_HEADERS_VSW" int null,
	"PURGE_HEADERS_WEEKS_RECEIPT" int null,
	"PURGE_HEADERS_WEEKS_ASN" int null,
	"PURGE_HEADERS_WEEKS_DUMMY" int null,
	"PURGE_HEADERS_WEEKS_DROPSHIP" int null,
	"PURGE_HEADERS_WEEKS_RESERVE" int null,
	"PURGE_HEADERS_WEEKS_WORKUPTOTALBUY" int null,
	"PURGE_HEADERS_WEEKS_PO" int null,
	"PURGE_HEADERS_WEEKS_VSW" int null) 

go
