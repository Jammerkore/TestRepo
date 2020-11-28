--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 1/20/2014 -TT#1511-MD -jsobek -Data Layer Request - Changes for Method and Workflow change history
--dv =============================================   
create table [dbo].[WORKFLOW_CHANGE_HISTORY] ( 
	"WORKFLOW_RID" int not null,
	"CHANGE_DATE" datetime not null,
	"USER_RID" int not null,
	"WORKFLOW_COMMENT" varchar(250) null,  
	"WINDOWS_USER" varchar(250) null,
	"WINDOWS_MACHINE" varchar(250) null,
	"WINDOWS_REMOTE_MACHINE" varchar(250) null) 
go
