--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================    
create table [dbo].[AUDIT_MODIFY_SALES_MATRIX] ( 
	"AUDIT_FORECAST_RID" int not null,
	"SGL_RID" int not null,
	"BOUNDARY" int not null,
	"GRADE_CODE" varchar(20) null,
	"SELL_THRU" int not null,
	"NUMBER_OF_STORES" int null,
	"MATRIX_RULE" int null,
	"MATRIX_RULE_QUANTITY" float null)  

go
