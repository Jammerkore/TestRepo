--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[TASKLIST] ( 
	"TASKLIST_RID" int identity not null,
	"TASKLIST_NAME" varchar(50) not null,
	"USER_RID" int not null,
	"SYSTEM_GENERATED_IND" char(1) not null,
	"CREATED_BY_USER_RID" int null,
	"CREATED_DATETIME" datetime null,
	"LAST_MODIFIED_BY_USER_RID" int null,
	"LAST_MODIFIED_DATETIME" datetime null)  

go
