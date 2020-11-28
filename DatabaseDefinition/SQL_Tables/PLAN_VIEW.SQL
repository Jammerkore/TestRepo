--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[PLAN_VIEW] ( 
	"VIEW_RID" int identity not null,
	"USER_RID" int not null,
	"VIEW_ID" varchar(250) not null,
	"GROUPBY_TYPE" int not null)  

go
