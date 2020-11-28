--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1076 -JScott -Size Curves by Set
--dv ============================================= 
create table [dbo].[NODE_SIZE_CURVE_CRITERIA_DETAIL] ( 
	"NSCCD_RID" int identity not null,
	"PH_OFFSET_IND" int null,
	"PH_RID" int null,
	"PHL_SEQUENCE" int null,
	"PHL_OFFSET" int null,
	"CDR_RID" int null,
	"APPLY_LOST_SALES_IND" char(1) null,
	"OLL_RID" int null,
	"CUSTOM_OLL_RID" int null,
	"SIZE_GROUP_RID" int null,
	"CURVE_NAME" varchar(50) null,
	"SG_RID" int null)

go
