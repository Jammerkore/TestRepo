--dv =============================================
--dv Created date: 3/26/2014
--dv Notes:	
--dv History: 2015-02-19 TT#1414-MD -jsobek -Attribute Set Filter
--dv =============================================  
create table [dbo].[STORE_GROUP] ( 
	"SG_RID" int identity not null,
	"SG_ID" varchar(50) not null,
	"SG_DYNAMIC_GROUP_IND" char(1) default '0' not null,
	"USER_RID" int null,
	"FILTER_RID" int null,
	"IS_ACTIVE" bit default 1 null,
	"SG_VERSION" int null
	)  

go
