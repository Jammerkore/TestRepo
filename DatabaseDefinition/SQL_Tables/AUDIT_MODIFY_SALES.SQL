--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================     
create table [dbo].[AUDIT_MODIFY_SALES] ( 
	"AUDIT_FORECAST_RID" int not null,
	"FILTER_NAME" varchar(50) null,
	"AVERAGE_BY" varchar(30) null) ON 'AUDIT'  

go
