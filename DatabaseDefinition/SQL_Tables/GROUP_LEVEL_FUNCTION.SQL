--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#43 -stodd -projected sales
--dv =============================================   
create table [dbo].[GROUP_LEVEL_FUNCTION] ( 
	"METHOD_RID" int not null,
	"SGL_RID" int not null,
	"DEFAULT_IND" char(1) null,
	"PLAN_IND" char(1) null,
	"USE_DEFAULT_IND" char(1) null,
	"CLEAR_IND" char(1) null,
	"SEASON_IND" char(1) null,
	"SEASON_HN_RID" int null,
	"GLFT_ID" int not null,
	"GLSB_ID" int not null,
	"LY_ALT_IND" char(1) null,
	"TREND_ALT_IND" char(1) null,
	"TY_EQUALIZE_WEIGHT_IND" char(1) null,
	"LY_EQUALIZE_WEIGHT_IND" char(1) null,
	"APPLY_EQUALIZE_WEIGHT_IND" char(1) null,
	"PROJECT_CURR_WEEK_SALES_IND" char(1) null)

go
