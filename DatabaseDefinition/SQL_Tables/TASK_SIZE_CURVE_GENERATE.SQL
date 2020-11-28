--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[TASK_SIZE_CURVE_GENERATE] (
	"TASK_SEQUENCE" int not null,
	"TASKLIST_RID" int not null,
	"GENERATE_SEQUENCE" int not null,
	"METHOD_NODE_TYPE" int null,
	"HN_RID" int null,
	"METHOD_RID" int null,
	"EXECUTE_CDR_RID" int null)

go
