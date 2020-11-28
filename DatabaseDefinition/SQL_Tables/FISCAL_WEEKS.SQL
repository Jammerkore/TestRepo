--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================     
create table [dbo].[FISCAL_WEEKS] ( 
	"ROW_SEQUENCE" int not null,
	"FISCAL_WEEK" int not null,
	"FIRST_DAY_OF_WEEK" int not null,
	"LAST_DAY_OF_WEEK" int not null,
	"FIRST_DAY_OF_WEEK_OFFSET" int not null,
	"LAST_DAY_OF_WEEK_OFFSET" int not null)  

go
