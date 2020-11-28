--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#413 -JSmith
--dv History: TT#413 -RMatelic
--dv History: TT#2155 -JEllis
--dv =============================================  
create table [dbo].[METHOD_SIZE_BASIS_ALLOCATION] ( 
	"METHOD_RID" int not null,
	"SIZE_BASIS_HDR_RID" int null,
	"COLOR_CODE_RID" int null,
	"SIZE_BASIS_RULE" int null,
	"STORE_FILTER_RID" int null,
	"SIZE_GROUP_RID" int null,
	"HEADER_COMPONENT" int null,
	"RULE_QUANTITY" int null,
	"SIZE_CONSTRAINT_RID" int null,
	"SIZE_CURVE_GROUP_RID" int null,
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
	"USE_DEFAULT_CURVE_IND" char(1) null,
	"GENCURVE_NSCCD_RID" int null, 
	"APPLY_RULES_ONLY_IND" char(1) default '0' null,
	"INCLUDE_RESERVE_IND" char(1) default '0' null)

go
