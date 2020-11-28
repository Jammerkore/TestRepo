--dv =============================================
--dv Created date: 4/28/2015
--dv Notes:	Ties groups to levels based on version
--dv =============================================  
create table [dbo].[STORE_GROUP_JOIN_HISTORY] ( 
	"SGJ_RID" int identity not null,
	"SG_RID" int not null,
	"SG_VERSION" int not null,
	"SGL_RID" int not null,
	"SGL_VERSION" int not null,
	"STORE_COUNT" int not null,
	"SGL_OVERRIDE_ID" varchar(50) null,
	"SGL_OVERRIDE_SEQUENCE" int null,
	)  

go
