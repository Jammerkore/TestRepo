--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[WORKFLOW_STEP_ALLOCATION] ( 
	"WORKFLOW_RID" int not null,
	"STEP_NUMBER" int not null,
	"ACTION_METHOD_TYPE" int not null,
	"METHOD_RID" int null,
	"PCT_TOLERANCE" float null,
	"REVIEW_IND" char(1) null,
	"STORE_FILTER_RID" int null)  

go
