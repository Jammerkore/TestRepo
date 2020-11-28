--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[COMPUTATION_ITEM] ( 
	"PROCESS" int not null,
	"CG_RID" int not null,
	"ITEM_TYPE" int not null,
	"HN_RID" int not null,
	"FV_RID" int not null,
	"FISCAL_YEAR_WEEK" int not null,
	"ITEM_PROCESSED" char(1) null)  

go
