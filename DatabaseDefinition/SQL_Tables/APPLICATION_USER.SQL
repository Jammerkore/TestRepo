--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[APPLICATION_USER] ( 
	"USER_NAME" varchar(256) not null,
	"USER_PASSWORD" varchar(250) null,
	"USER_FULLNAME" varchar(250) null,
	"USER_DESCRIPTION" varchar(250) null,
	"USER_ACTIVE_IND" char(1) not null,
	"USER_RID" int identity not null,
	"USER_DELETE_IND" char(1) null,
	"USER_DELETE_DATETIME" smalldatetime null)  

go
