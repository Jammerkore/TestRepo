--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[USER_SESSION] ( 
	"SESSION_RID" int identity not null,
	"USER_RID" int not null,
	"SESSION_STATUS" int null,
	"LOGIN_DATETIME" datetime null,
	"LOGOUT_DATETIME" datetime null,
	"COMPUTER_NAME" varchar(50) null)  

go
