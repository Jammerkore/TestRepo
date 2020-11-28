--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[FORECAST_VERSION] ( 
	"FV_RID" int identity not null,
	"FV_ID" char(1) not null,
	"DESCRIPTION" varchar(50) not null,
	"PROTECT_HISTORY_IND" char(1) not null,
	"ACTIVE_IND" char(1) not null,
	"BLEND_TYPE" int null,
	"ACTUAL_FV_RID" int null,
	"FORECAST_FV_RID" int null,
	"CURRENT_BLEND_IND" char(1) null,
	"SIMILAR_STORE_IND" char(1) null)  

go
