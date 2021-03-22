--dv =============================================
--dv Modified date: 3/19/2021
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[PLAN_VIEW_SPLITTER] ( 
	"VIEW_RID" int not null,
	"USER_RID" int not null,
	"PLAN_SESSION_TYPE" int not null,
	"SPLITTER_TYPE_IND" char(1) not null,
	"SPLITTER_SEQUENCE" int not null,
	"SPLITTER_PERCENTAGE" float not null)  

go
