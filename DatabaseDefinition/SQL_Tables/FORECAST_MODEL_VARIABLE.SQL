--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[FORECAST_MODEL_VARIABLE] ( 
	"FORECAST_MOD_RID" int not null,
	"VARIABLE_SEQUENCE" int not null,
	"VARIABLE_NUMBER" int not null,
	"FORECAST_FORMULA" int not null,
	"ASSOC_VARIABLE" int not null,
	"GRADE_WOS_IDX" char null,
	"STOCK_MODIFIER" char null,
	"FWOS_OVERRIDE" char null,
	"STOCK_MIN" char null,
	"STOCK_MAX" char null,
	"MIN_PLUS_SALES" char null,
	"SALES_MODIFIER" char null,
	"USE_PLAN" char null,
	"ALLOW_CHAIN_NEGATIVES" char default '0' not null )	
go
