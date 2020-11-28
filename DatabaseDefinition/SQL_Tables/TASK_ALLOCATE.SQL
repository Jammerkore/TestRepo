--dv =============================================
--dv Modified date: 10/23/2014
--dv Notes:	Holds an allocate task
--dv History: TT#1313-MD -jsobek -Header Filters
--dv ============================================= 
create table [dbo].[TASK_ALLOCATE] ( 
	"TASK_SEQUENCE" int not null,
	"TASKLIST_RID" int not null,
	"ALLOCATE_SEQUENCE" int not null,
	"HN_RID" int null,
	"FILTER_RID" int null
)  
go
