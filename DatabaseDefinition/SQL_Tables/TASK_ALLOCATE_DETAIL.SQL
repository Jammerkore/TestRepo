--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[TASK_ALLOCATE_DETAIL] ( 
	"TASK_SEQUENCE" int not null,
	"TASKLIST_RID" int not null,
	"ALLOCATE_SEQUENCE" int not null,
	"DETAIL_SEQUENCE" int not null,
	"WORKFLOW_METHOD_IND" int null,
	"METHOD_RID" int null,
	"WORKFLOW_RID" int null,
	"EXECUTE_CDR_RID" int null)  

go
