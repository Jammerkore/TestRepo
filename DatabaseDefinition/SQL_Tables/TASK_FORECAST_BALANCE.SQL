--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[TASK_FORECAST_BALANCE] ( 
	"TASK_SEQUENCE" int not null,
	"TASKLIST_RID" int not null,
	"FORECAST_BALANCE_SEQUENCE" int not null,
	"HN_RID" int null,
	"FV_RID" int null,
	"FBMOD_RID" int null)  

go
