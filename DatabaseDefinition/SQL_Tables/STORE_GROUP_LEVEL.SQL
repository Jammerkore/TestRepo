--dv =============================================
--dv Created date: 3/26/2014
--dv Notes:	
--dv History: 2015-02-19 TT#1414-MD -jsobek -Attribute Set Filter
--dv =============================================  
create table [dbo].[STORE_GROUP_LEVEL] ( 
	"SGL_RID" int identity not null,
	"SGL_SEQUENCE" int not null,
	"SG_RID" int not null,
	"SGL_ID" varchar(50) not null,
	"IS_ACTIVE" bit default 1 null,
	"SGL_VERSION" int null,
	"LEVEL_TYPE" int null
	)  

go
