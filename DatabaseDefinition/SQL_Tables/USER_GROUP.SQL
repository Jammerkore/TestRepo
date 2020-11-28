--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[USER_GROUP] ( 
	"GROUP_RID" int identity not null,
	"GROUP_NAME" varchar(30) not null,
	"GROUP_DESCRIPTION" varchar(250) null,
	"GROUP_ACTIVE_IND" char(1) not null,
	"GROUP_DELETE_IND" char(1) null,
	"GROUP_DELETE_DATETIME" smalldatetime null)  

go
