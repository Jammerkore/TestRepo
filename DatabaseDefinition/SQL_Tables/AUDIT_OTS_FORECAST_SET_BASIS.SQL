--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 1/12/2010 -tt#1049 -apicchetti -Forecast Audit and Trend Tab Apply to do not match on the merchandise levels 
--dv =============================================     
create table [dbo].[AUDIT_OTS_FORECAST_SET_BASIS] ( 
	"AUDIT_FORECAST_RID" int not null,
	"SGL_RID" int not null,
	"BASIS_SEQUENCE" int not null,
	"BASIS_HN_RID" int null,
	"BASIS_HN_TEXT" varchar(150) null,			
	"BASIS_FV_RID" int null,
	"BASIS_TIME_PERIOD" varchar(50) null,
	"BASIS_WEIGHT" float null,
	"BASIS_TYPE" varchar(30) default '''''' not null,
	"BASIS_TYPE_SORT_CODE" int default 0 not null) ON 'AUDIT'  

go
