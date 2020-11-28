--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[APPLICATION_SERVICES_HISTORY] ( 
	"SERVICE_EPROCESSES_ID" int not null,
	"SERVICE_START_DATETIME" datetime null,
	"SERVICE_VERSION" varchar(250) not null)

go
