--dv =============================================
--dv Modified date: 2/19/2014
--dv History:	TT#41-MD - Jellis -UC#2 -Size Inventory Min Max pt 1
--dv			TT#246-MD - Jellis -AnF VSW In_Store Minimum
--dv			5.2.1.1 10-10-2013 -TT#848-MD -jsobek -Fill to Size Plan Presentation
--dv =============================================  
create table [dbo].[HEADER_BULK_COLOR_SIZE_NEED] ( 
	"HDR_RID" int not null,
	"HDR_BC_RID" int not null,
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
	"VSW_SIZE_CONSTRAINTS" int null,
	"FILL_SIZES_TO_TYPE" int null
	) ON 'ALLOCATION'
go
