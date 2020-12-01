--dv =============================================
--dv Modified date: 10/17/2020
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[PLAN_VIEW_FORMAT] ( 
	"VIEW_RID" int not null,
	"PLAN_BASIS_TYPE" int not null,
	"VARIABLE_NUMBER" int not null,
	"QUANTITY_VARIABLE_KEY" int null,
	"TIME_PERIOD_TYPE" int null,
	"TIME_PERIOD_KEY" int null,
	"VARIABLE_TOTAL_KEY" int null,
	"WIDTH" int not null)  

go
