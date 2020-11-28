--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[AUDIT_FORECAST] ( 
	"AUDIT_FORECAST_RID" int identity not null,
	"PROCESS_DATE_TIME" datetime null,
	"PROCESS_RID" int null,
	"USER_RID" int null,
	"HN_RID" int null,
	"HN_TEXT" varchar(150) null,
	"METHOD_RID" int null,
	"METHOD_HAS_BEEN_DELETED" char(1) null,
	"METHOD_NAME" varchar(50) null,
	"METHOD_TYPE" int null,
	"STORE_FV_RID" int null,
	"CHAIN_FV_RID" int null,
	"TIME_RANGE_TYPE" char(1) null,
	"TIME_RANGE_NAME" varchar(50) null,
	"TIME_RANGE_DISPLAY" varchar(50) null,
	"TIME_RANGE_BEGIN" int null,
	"TIME_RANGE_END" int null,
	"SG_RID" int null,
	"ATTRIBUTE_NAME" varchar(50) null) ON 'AUDIT'  

go
