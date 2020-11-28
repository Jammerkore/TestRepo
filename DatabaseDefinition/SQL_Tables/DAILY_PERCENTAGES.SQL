--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[DAILY_PERCENTAGES] ( 
	"HN_RID" int not null,
	"ST_RID" int not null,
	"CDR_RID" int not null,
	"DAY1" float not null,
	"DAY2" float not null,
	"DAY3" float not null,
	"DAY4" float not null,
	"DAY5" float not null,
	"DAY6" float not null,
	"DAY7" float not null)  

go
