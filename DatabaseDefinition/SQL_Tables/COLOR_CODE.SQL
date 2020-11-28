--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[COLOR_CODE] ( 
	"COLOR_CODE_RID" int identity not null,
	"COLOR_CODE_ID" varchar(50) not null,
	"COLOR_CODE_NAME" varchar(250) not null,
	"COLOR_CODE_GROUP" varchar(250) null,
	"VIRTUAL_IND" char(1) null,
	"PURPOSE" int null)  

go
