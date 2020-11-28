--dv =============================================
--dv Modified date: 12/17/2014
--dv Notes:	
--dv History: Add SHOW_SIGNOFF_PROMPT column
--dv =============================================   
create table [dbo].[USER_OPTIONS] ( 
	"USER_RID" int not null,
	"MY_HIERARCHY" varchar(250) null,
	"MY_HIERARCHY_COLOR" varchar(30) null,
	"MY_WORKFLOWMETHODS" varchar(50) null,
	"THEME_RID" int null,
	"SHOW_LOGIN" char(1) default '1' null,
	"FORECAST_MONITOR_ACTIVE" char(1) null,
	"FORECAST_MONITOR_DIRECTORY" varchar(250) null,
	"MODIFY_SALES_MONITOR_ACTIVE" char(1) null,
	"MODIFY_SALES_MONITOR_DIRECTORY" varchar(250) null,
	"AUDIT_LOGGING_LEVEL" int null,
    "SHOW_SIGNOFF_PROMPT" char(1) null,
	"DCFULFILLMENT_MONITOR_ACTIVE" char(1) null,
	"DCFULFILLMENT_MONITOR_DIRECTORY" varchar(250) null) 

go
