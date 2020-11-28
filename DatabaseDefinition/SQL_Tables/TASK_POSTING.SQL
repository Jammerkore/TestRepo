--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[TASK_POSTING] ( 
	"TASK_SEQUENCE" int not null,
	"TASKLIST_RID" int not null,
	"INPUT_DIRECTORY" varchar(150) not null,
	"FILE_MASK" varchar(50) not null,
	"CONCURRENT_FILES" int not null,
	"RUN_UNTIL_FILE_PRESENT_IND" char(1) not null,
	"RUN_UNTIL_FILE_MASK" varchar(50) null,  
	"FILE_PROCESSING_DIRECTION" int default 802873 not null)
go
