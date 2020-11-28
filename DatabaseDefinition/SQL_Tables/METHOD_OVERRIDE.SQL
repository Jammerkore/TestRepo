--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1401 -GTaylor -Reservation Stores 
--dv History: TT#667 -stodd
--dv History: TT#1287 -AGallagher -Inventory Min/Max
--dv ============================================= 	 
create table [dbo].[METHOD_OVERRIDE] ( 
	"METHOD_RID" int not null,
	"STORE_FILTER_RID" int null,
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
	"COLOR_MULT" int null,
	"SIZE_MULT" int null,
	"ALL_COLOR_MIN" int null,
	"ALL_COLOR_MAX" int null,
	"SG_RID" int null,
	"EXCEED_CAPACITY" char(1) null,
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
	"IMO_SG_RID" int NULL DEFAULT 0,
	"IMO_APPLY_VSW" bit NULL DEFAULT 0	
	)

go
