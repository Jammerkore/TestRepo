--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[METHOD_VELOCITY_MATRIX] ( 
	"METHOD_RID" int not null,
	"VELOCITY_SELL_THRU_INDEX" int not null,
	"SGL_RID" int not null,
	"BOUNDARY" int not null,
	"VELOCITY_RULE" int not null,
	"VELOCITY_QUANTITY" float null)  

go
