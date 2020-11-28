--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[FORECAST_MODEL] ( 
	"FORECAST_MOD_RID" int identity not null,
	"FORECAST_MOD_ID" varchar(50) not null,
	"DEFAULT_IND" char(1) not null default 0,
	"CALC_MODE" varchar(50) not null)  

go
