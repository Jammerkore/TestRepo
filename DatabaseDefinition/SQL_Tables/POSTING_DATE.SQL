--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[POSTING_DATE] ( 
	"PH_RID" int not null,
	"POSTING_DATE" smalldatetime not null,
	"POSTING_DATE_YYYYDDD" int null,
	"CURR_DATE" smalldatetime null,
	"CURR_DATE_YYYYDDD" int null,
	"POSTING_DATE_OFFSET" int null,
	"CURR_DATE_OFFSET" int null)  

go
