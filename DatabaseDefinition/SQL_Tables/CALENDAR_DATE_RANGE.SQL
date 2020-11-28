--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[CALENDAR_DATE_RANGE] ( 
	"CDR_RID" int identity not null,
	"CDR_NAME" varchar(50) null,
	"CDR_START" int null,
	"CDR_END" int null,
	"CDR_RANGE_TYPE_ID" int null,
	"CDR_DATE_TYPE_ID" int not null,
	"CDR_RELATIVE_TO" smallint not null,
	"CDR_DYNAMIC_SWITCH" char(1) default '0' not null,
	"CDR_DYNAMIC_SWITCH_DATE" int null)  

go
