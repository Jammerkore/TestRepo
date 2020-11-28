--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================
create table [dbo].[SECURITY_USER_HIERARCHY_NODE] ( 
	"USER_RID" int not null,
	"HN_RID" int not null,
	"ACTION_ID" int default 0 not null,
	"SEC_TYPE" int default 0 not null,
	"SEC_LVL_ID" int null)  

go
