--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[METHOD_SPREAD_BASIS_DETAIL] ( 
	"METHOD_RID" int not null,
	"DETAIL_SEQ" int not null,
	"FV_RID" int not null,
	"CDR_RID" int not null,
	"WEIGHT" float not null)  

go
