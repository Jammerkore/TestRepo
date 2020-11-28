--dv =============================================
--dv Modified date: 9/15/2014
--dv Notes:	
--dv History:  RMatelic - Add CREATE table SQL
--dv 		   TT#1300-MD -jsobek -Format Exception when opening a newly created GA Method (New or copied)
--dv ============================================= 
create table [dbo].[METHOD_GROUP_ALLOCATION] ( 
	"METHOD_RID" int not null,
	"STORE_GRADE_TIMEFRAME" int null,
	"EXCEED_MAX_IND" char(1) null,
	"PERCENT_NEED_LIMIT" float null,
	"RESERVE" float null,
	"PERCENT_IND" char(1) null,
	"MERCH_HN_RID" int null,
	"MERCH_PH_RID" int null,
	"MERCH_PHL_SEQUENCE" int null,
	"ON_HAND_HN_RID" int null,
	"ON_HAND_PH_RID" int null,
	"ON_HAND_PHL_SEQUENCE" int null,
	"ON_HAND_FACTOR" float null,
	"SG_RID" int null,
	"MERCH_UNSPECIFIED" char(1) null,
	"ON_HAND_UNSPECIFIED" char(1) null,
	"RESERVE_AS_BULK" float null,
	"RESERVE_AS_PACKS" float null,
	"STORE_GRADES_SG_RID" int null, 
	"INVENTORY_IND" char(1) null,
	"IB_MERCH_TYPE" int default 3 not null,
	"IB_MERCH_HN_RID" int null,
	"IB_MERCH_PH_RID" int null,
	"IB_MERCH_PHL_SEQUENCE" int null,
	"BEGIN_CDR_RID" int null,
	"SHIP_TO_CDR_RID" int null,
	"LINE_ITEM_MIN_OVERRIDE_IND" char(1) default 0 not null,
	"LINE_ITEM_MIN_OVERRIDE" int null,
	"HDRINVENTORY_IND" char(1) null,
	"HDRIB_MERCH_TYPE" int default 3 not null,
	"HDRIB_MERCH_HN_RID" int null,
	"HDRIB_MERCH_PH_RID" int null,
	"HDRIB_MERCH_PHL_SEQUENCE" int null	
	)
go