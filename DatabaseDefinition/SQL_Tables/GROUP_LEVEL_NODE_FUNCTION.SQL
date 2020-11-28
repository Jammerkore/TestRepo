--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[GROUP_LEVEL_NODE_FUNCTION] ( 
	"METHOD_RID" int not null,
	"SGL_RID" int not null,
	"HN_RID" int not null,
	"APPLY_MIN_MAXES_IND" char(1) null,
	"MIN_MAXES_INHERIT_TYPE" int null)  

go
