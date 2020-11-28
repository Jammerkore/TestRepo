--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#195 MD - JSmith - Add environment authentication
--dv =============================================  
create table [dbo].[APPLICATION_UPGRADE_HISTORY] ( 
    "UPGRADE_RID" int identity not null,
	"UPGRADE_VERSION" varchar(50) not null, 
	"UPGRADE_DATETIME" datetime not null,
	"UPGRADE_USER" varchar(250) not null,
	"UPGRADE_MACHINE" varchar(250) not null,
	"UPGRADE_REMOTE_MACHINE" varchar(250) null,
	"UPGRADE_CONFIGURATION" varchar(50) not null)

go
