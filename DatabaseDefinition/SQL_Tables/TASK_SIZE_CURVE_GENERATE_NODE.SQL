--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#707 -JScott -Size Curve process needs to multi-thread
--dv ============================================= 
create table [dbo].[TASK_SIZE_CURVE_GENERATE_NODE] (
	"TASK_SEQUENCE" int not null,
	"TASKLIST_RID" int not null,
	"CONCURRENT_PROCESSES" int null)

go
