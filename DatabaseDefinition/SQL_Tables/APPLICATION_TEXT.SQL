--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[APPLICATION_TEXT] ( 
	"TEXT_CODE" int not null,
	"TEXT_VALUE" varchar(250) not null,
	"TEXT_MAX_LENGTH" int not null,
	"TEXT_TYPE" int not null,
	"TEXT_LEVEL" int null,
	"TEXT_VALUE_TYPE" int null,
	"TEXT_ORDER" int default 0 null)  

go
