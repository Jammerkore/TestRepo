--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#413 -JSmith
--dv		  TT#413 -RMatelic
--dv		  TT#2155 -JEllis
--dv		  TT#41-MD -GTaylor -UC#2
--dv		  TT#246-MD -AGallagher -VSW Size -Add In-store Size Constraint options
--dv		  6/15/2015 -TT#1636-MD -jsobek -Pre-Pack Fill Size
--dv =============================================  
create table [dbo].[METHOD_FILL_SIZE_HOLES] ( 
	"METHOD_RID" int not null,
	"AVAILABLE" float null,
	"AVAILABLE_TYPE_IND" char(1) null,
	"SIZE_GROUP_RID" int null,
	"SIZE_CURVE_GROUP_RID" int null,
	"MERCH_TYPE" int null,
	"MERCH_HN_RID" int null,
	"MERCH_PH_RID" int null,
	"MERCH_PHL_SEQUENCE" int null,
	"STORE_FILTER_RID" int null,
	"SIZE_CONSTRAINT_RID" int null,
	"SIZE_ALTERNATE_RID" int null,
	"GENCURVE_HCG_RID" int null,
	"GENCURVE_HN_RID" int null,
	"GENCURVE_PH_RID" int null,
	"GENCURVE_PHL_SEQUENCE" int null,
	"GENCURVE_COLOR_IND" char(1) null,
	"GENCURVE_MERCH_TYPE" int null,
	"GENCONSTRAINT_HCG_RID" int null,
	"GENCONSTRAINT_HN_RID" int null,
	"GENCONSTRAINT_PH_RID" int null,
	"GENCONSTRAINT_PHL_SEQUENCE" int null,
	"GENCONSTRAINT_COLOR_IND" char(1) null,
	"GENCONSTRAINT_MERCH_TYPE" int null,
	"NORMALIZE_SIZE_CURVES_IND" char(1) null,
	"FILL_SIZES_TO_TYPE" int null,
	"USE_DEFAULT_CURVE_IND" char(1) null,
	"GENCURVE_NSCCD_RID" int null, 
	"APPLY_RULES_ONLY_IND" char(1) default '0' null,
	"IB_MERCH_TYPE" int null,
	"IB_MERCH_HN_RID" int null,
	"IB_MERCH_PH_RID" int null,
	"IB_MERCH_PHL_SEQUENCE" int null,
	"VSW_SIZE_CONSTRAINTS" int null,			 
	"VSW_SIZE_CONSTRAINTS_IND" char(1) null,
	"AVG_PACK_DEV_TOLERANCE" float null,
	"MAX_PACK_NEED_TOLERANCE" float null,
	"OVERRIDE_AVG_PACK_DEV_IND" char(1) null,
	"OVERRIDE_MAX_PACK_NEED_IND" char(1) null, 
	"PACK_TOLERANCE_NO_MAX_STEP_IND" char(1) default '0' null,
	"PACK_TOLERANCE_STEPPED_IND" char(1) default '0' null) 
go
