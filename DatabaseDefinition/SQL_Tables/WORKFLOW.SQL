--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[WORKFLOW] ( 
	"WORKFLOW_RID" int identity not null,
	"WORKFLOW_NAME" varchar(50) not null,
	"WORKFLOW_TYPE_ID" int not null,
	"WORKFLOW_USER_RID" int not null,
	"WORKFLOW_DESCRIPTION" varchar(250) null,
	"STORE_FILTER_RID" int null,
	"WORKFLOW_OVERRIDE" char(1) null)  

go
