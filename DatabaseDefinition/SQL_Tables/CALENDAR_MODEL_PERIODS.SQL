--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[CALENDAR_MODEL_PERIODS] ( 
	"CM_RID" int not null,
	"CMP_TYPE" int not null,
	"CMP_SEQUENCE" int not null,
	"CMP_ID" varchar(50) not null,
	"CMP_ABBREVIATION" varchar(10) not null,
	"NO_OF_TIME_PERIODS" int not null)  

go
