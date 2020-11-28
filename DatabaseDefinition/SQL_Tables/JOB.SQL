--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#1430 - JScott - Long Tasklist Name causes duplicate key on JOB table
--dv =============================================  
create table [dbo].[JOB] ( 
	"JOB_RID" int identity not null,
	"JOB_NAME" varchar(100) not null,
	"SYSTEM_GENERATED_IND" char(1) not null,
	"CREATED_BY_USER_RID" int null,
	"CREATED_DATETIME" datetime null,
	"LAST_MODIFIED_BY_USER_RID" int null,
	"LAST_MODIFIED_DATETIME" datetime null)  

go
