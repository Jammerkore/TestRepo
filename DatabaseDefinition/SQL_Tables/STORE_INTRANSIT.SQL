--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[STORE_INTRANSIT] ( 
	"HN_RID" int not null,
	"TIME_ID" int not null,
	"ST_RID" int not null,
	"UNITS" int null) ON 'ALLOCATION'  

go
