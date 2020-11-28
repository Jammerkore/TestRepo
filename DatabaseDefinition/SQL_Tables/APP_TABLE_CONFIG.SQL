--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[APP_TABLE_CONFIG] ( 
	"TABLE_TYPE" varchar(30) not null,
	"NUMBER_OF_TABLES" int default 1 not null,
	"TABLE_PARTITION_COLUMN" varchar(30) not null)  

go
