--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[METHOD_MATRIX] ( 
	"METHOD_RID" int not null,
	"FILTER_RID" int null,
	"HN_RID" int not null,
	"HIGH_LEVEL_FV_RID" int not null,
	"CDR_RID" int not null,
	"MATRIX_TYPE" int null,
	"MODEL_RID" int null,
	"LOW_LEVEL_FV_RID" int not null,
	"LOW_LEVEL_TYPE" int not null,
	"LOW_LEVEL_OFFSET" int not null,
	"LOW_LEVEL_SEQUENCE" int not null,
	"INCLUDE_INELIGIBLE_STORES_IND" char(1) not null,
	"INCLUDE_SIMILAR_STORES_IND" char(1) not null,
	"VARIABLE_NUMBER" int not null,
	"ITERATIONS_TYPE" int not null,
	"ITERATIONS_COUNT" int not null,
	"BALANCE_MODE" int not null,
	"CALC_MODE" varchar(50) not null,
	"OLL_RID" int null)  

go
