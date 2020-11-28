--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================    
create table [dbo].[AUDIT_OTS_FORECAST_SET] ( 
	"AUDIT_FORECAST_RID" int not null,
	"SGL_RID" int not null,
	"SET_NAME" varchar(50) null,
	"FORECAST_METHOD_TYPE" varchar(30) null,
	"STOCK_MIN_MAX" char(1) null) ON 'AUDIT'  

go
