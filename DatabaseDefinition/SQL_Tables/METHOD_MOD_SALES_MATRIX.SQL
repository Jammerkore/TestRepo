--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[METHOD_MOD_SALES_MATRIX] ( 
	"METHOD_RID" int not null,
	"SGL_RID" int null,
	"BOUNDARY" int not null,
	"SELL_THRU" int not null,
	"MATRIX_RULE" int not null,
	"MATRIX_RULE_QUANTITY" float null)  

go
