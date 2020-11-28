--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[TREND_CAPS] ( 
	"METHOD_RID" int not null,
	"TREND_CAP_ID" int not null,
	"TOL_PCT" float null,
	"HIGH_LIMIT" float null,
	"LOW_LIMIT" float null,
	"SGL_RID" int not null)  

go
