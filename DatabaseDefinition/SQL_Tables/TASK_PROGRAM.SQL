--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[TASK_PROGRAM] ( 
	"TASK_SEQUENCE" int not null,
	"TASKLIST_RID" int not null,
	"PROGRAM_PATH" varchar(150) not null,
	"PROGRAM_PARMS" varchar(150) null)  

go
