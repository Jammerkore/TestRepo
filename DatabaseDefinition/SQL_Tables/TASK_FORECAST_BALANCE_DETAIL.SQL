--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[TASK_FORECAST_BALANCE_DETAIL] ( 
	"TASK_SEQUENCE" int not null,
	"TASKLIST_RID" int not null,
	"FORECAST_BALANCE_SEQUENCE" int not null,
	"DETAIL_SEQUENCE" int not null,
	"METHOD_RID" int null,
	"EXECUTE_CDR_RID" int null)  

go
