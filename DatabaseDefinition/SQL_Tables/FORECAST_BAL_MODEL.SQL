--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: MID Track #5647 - KJohnson - Matrix Forecast
--dv =============================================    
create table [dbo].[FORECAST_BAL_MODEL] ( 
	"FBMOD_RID" int identity not null,
	"FBMOD_ID" varchar(50) null,
	"MATRIX_TYPE" int null,
	"ITERATIONS_TYPE" int null,
	"ITERATIONS_COUNT" int null,
	"BALANCE_MODE" int null,
	"CALC_MODE" varchar(50) null)  

go
