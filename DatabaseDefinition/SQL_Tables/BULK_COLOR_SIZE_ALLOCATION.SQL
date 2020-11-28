--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1401 -GTaylor -Reservation Stores 
--dv History: TT#246-MD -JEllis -AnF VSW In-Store Minimum
--dv History: TT#246-MD -JEllis -AnF VSW In Store Minimum phase 2 
--dv ============================================= 
create table [dbo].[BULK_COLOR_SIZE_ALLOCATION] ( 
    "HDR_RID" int not null,  
	"HDR_BC_RID" int not null,
    "HDR_BCSZ_KEY" int not null,
	"ST_RID" int not null,
	"UNITS_ALLOCATED" int null,
	"UNITS_SHIPPED" int null,
	"UNITS_ALLOCATED_BY_AUTO" int null,
	"MINIMUM" int null,
	"MAXIMUM" int null,
	"PRIMARY_MAX" int null,
	"ALLOC_STORE_DET_AUDIT_FLAGS" int null,
	"SHIPPING_STATUS_FLAGS" int null,
	"ITEM_UNITS_ALLOCATED" int null,					
	"ITEM_MINIMUM" int null,                            
	"ITEM_IDEAL_MINIMUM" int null                       
	) ON 'ALLOCATION'  

go
