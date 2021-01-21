--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#523 - JScott - Duplicate folder when new folder added
--dv =============================================   
create table [dbo].[METHOD] ( 
	"METHOD_RID" int identity not null,
	"METHOD_NAME" varchar(50) not null,
	"METHOD_TYPE_ID" int not null,
	"PROFILE_TYPE_ID" int not null,
	"USER_RID" int not null,
	"METHOD_DESCRIPTION" varchar(250) null,
	"SG_RID" int null,
	"VIRTUAL_IND" char(1) not null,
	"METHOD_STATUS" int default 0 not null,
	"CUSTOM_OLL_RID" int null,
	"TEMPLATE_IND" char(1))  

go
