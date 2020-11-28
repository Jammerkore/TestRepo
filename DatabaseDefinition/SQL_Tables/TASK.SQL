--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#506-MD -jsobek -Add infrastructure to allow Email to be sent from the application
--dv =============================================  
create table [dbo].[TASK] ( 
	"TASKLIST_RID" int not null,
	"TASK_SEQUENCE" int not null,
	"TASK_TYPE" int not null,
	"MAX_MESSAGE_LEVEL" int not null,
	"EMAIL_SUCCESS_FROM" varchar(500) null,
	"EMAIL_SUCCESS_TO" varchar(500) null,
	"EMAIL_SUCCESS_CC" varchar(500) null,
	"EMAIL_SUCCESS_BCC" varchar(500) null,
	"EMAIL_SUCCESS_SUBJECT" varchar(500) null,
	"EMAIL_SUCCESS_BODY" varchar(4000) null,
	"EMAIL_FAILURE_FROM" varchar(500) null,
	"EMAIL_FAILURE_TO" varchar(500) null,
	"EMAIL_FAILURE_CC" varchar(500) null,
	"EMAIL_FAILURE_BCC" varchar(500) null,
	"EMAIL_FAILURE_SUBJECT" varchar(500) null,
	"EMAIL_FAILURE_BODY" varchar(4000) null
	)  

go
