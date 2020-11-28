--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[METHOD_VELOCITY_BASIS] ( 
	"METHOD_RID" int not null,
	"BASIS_SEQUENCE" int not null,
	"BASIS_HN_RID" int null,
	"BASIS_FV_RID" int null,
	"CDR_RID" int null,
	"SALES_WEIGHT" float null,
	"BASIS_PH_RID" int null,
	"BASIS_PHL_SEQUENCE" int null)  

go
