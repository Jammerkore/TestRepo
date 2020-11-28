--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[OVERRIDE_LL_MODEL_HEADER] ( 
	"OLL_RID" int identity not null primary key,
	"NAME" varchar(250) null,
	"HN_RID" int not null,
	"HIGH_LEVEL_HN_RID" int null,
	"HIGH_LEVEL_SEQ" int null,
	"HIGH_LEVEL_OFFSET" int null,
	"HIGH_LEVEL_TYPE" int null,
	"LOW_LEVEL_SEQ" int null,
	"LOW_LEVEL_OFFSET" int  null,
	"LOW_LEVEL_TYPE" int null,
	"ACTIVE_ONLY_IND" char null)
go
