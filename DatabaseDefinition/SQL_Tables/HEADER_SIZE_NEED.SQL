--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#41-MD -Jellis -UC#2 - Size Inventory Min Max pt 1
--dv History: TT#246-MD -Jellis -AnF VSW In_Store Minimum
--dv =============================================   
create table [dbo].[HEADER_SIZE_NEED] ( 
	"HDR_RID" int not null,
	"SIZE_CURVE_GROUP_RID" int null,
	"MERCH_TYPE" int null,
	"HN_RID" int null,
	"PH_RID" int null,
	"PHL_SEQUENCE" int null,
	"SIZE_CONSTRAINT_RID" int null,
	"SIZE_ALTERNATE_RID" int null, 
	"NORMALIZE_SIZE_CURVES_IND" char(1) null,
	"IB_MERCH_TYPE" int null,
	"IB_MERCH_HN_RID" int null,
	"IB_MERCH_PH_RID" int null,
	"IB_MERCH_PHL_SEQUENCE" int null,
	"VSW_SIZE_CONSTRAINTS" int null
	) ON 'ALLOCATION'

go
