--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[APPLICATION_VERSION] ( 
	"MAJOR_VERSION" int not null,
	"MINOR_VERSION" int not null,
	"REVISION" int not null,
	"MODIFICATION" int not null,
	"WHEN_RUN" datetime not null)  

go
