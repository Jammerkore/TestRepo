--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[CALENDAR_MODEL] ( 
	"CM_RID" int identity not null,
	"CM_ID" varchar(50) not null,
	"START_DATE" datetime not null,
	"FISCAL_YEAR" int not null)  

go
