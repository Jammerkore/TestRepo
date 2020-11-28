--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1401 - Reservation Stores - GTaylor
--dv ============================================= 	
create table [dbo].[TOTAL_ALLOCATION] ( 
	"HDR_RID" int not null,
	"ST_RID" int not null,
	"STORE_GRADE_INDEX" int null,
	"UNITS_ALLOCATED" int null,
	"UNITS_SHIPPED" int null,
	"UNITS_ALLOCATED_BY_AUTO" int null,
	"UNITS_ALLOCATED_BY_RULE" int null,
	"CHOSEN_RULE_TYPE_ID" int null,
	"CHOSEN_RULE_LAYER_ID" int null,
	"SHIP_DAY" smalldatetime null,
	"NEED_DAY" smalldatetime null,
	"UNIT_NEED_BEFORE" int null,
	"PERCENT_NEED_BEFORE" float null,
	"MINIMUM" int null,
	"MAXIMUM" int null,
	"PRIMARY_MAX" int null,
	"STORE_CAPACITY" int null,
	"ALLOC_STORE_DET_AUDIT_FLAGS" int null,
	"ALLOC_STORE_GEN_AUDIT_FLAGS" int null,
	"SHIPPING_STATUS_FLAGS" int null,
	"CHOSEN_RULE_UNITS" int null,
	"EXCEED_CAPACITY_PERCENT" float null,
	"IMO_MAX_VALUE" int null DEFAULT null,			
	"IMO_MIN_SHIP_QTY" int null,				
	"IMO_PCT_PK_THRSHLD" float null,			
	"ITEM_UNITS_ALLOCATED" int null			
	) ON 'ALLOCATION'  

go
