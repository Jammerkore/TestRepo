--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================    
create table [dbo].[ALLOCATION_STEP_COMPONENT] ( 
	"WORKFLOW_RID" int not null,
	"STEP_NUMBER" int not null,
	"COMP_TYPE_ID" int not null,
	"COMP_CRIT_RID" int not null)  

go
