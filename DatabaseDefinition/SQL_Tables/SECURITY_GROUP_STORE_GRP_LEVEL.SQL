--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[SECURITY_GROUP_STORE_GRP_LEVEL] ( 
	"GROUP_RID" int not null,
	"SGL_RID" int not null,
	"ACTION_ID" int default 0 not null,
	"SEC_LVL_ID" int not null)  

go
