--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[STOCK_MIN_MAX] ( 
	"METHOD_RID" int not null,
	"SGL_RID" int not null,
	"HN_RID" int default -1 not null,
	"BOUNDARY" int not null,
	"CDR_RID" int not null,
	"MIN_STOCK" int null,
	"MAX_STOCK" int null)  

go
