--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1401 -GTaylor -Reservation Stores 
--dv ============================================= 
create table [dbo].[PACK_ALLOCATION] ( 
    "HDR_RID" int not null,
    "HDR_PACK_RID" int not null,
    "ST_RID" int not null,
    "PACKS_ALLOCATED" int null,
    "PACKS_SHIPPED" int null,
    "PACKS_ALLOCATED_BY_AUTO"  int null,
    "PACKS_ALLOCATED_BY_RULE"  int null,
    "CHOSEN_RULE_TYPE_ID" int null,
    "CHOSEN_RULE_LAYER_ID" int null,
    "NEED_DAY" smalldatetime null,
    "UNIT_NEED_BEFORE" int null,
    "PERCENT_NEED_BEFORE" float null,
    "MINIMUM"  int null,
    "MAXIMUM" int null, 
    "PRIMARY_MAX" int null,
    "ALLOC_STORE_DET_AUDIT_FLAGS" int null,
    "SHIPPING_STATUS_FLAGS" int null,
    "CHOSEN_RULE_PACKS" int null,
    "ITEM_PACKS_ALLOCATED" int null		
    ) ON 'ALLOCATION'

go
