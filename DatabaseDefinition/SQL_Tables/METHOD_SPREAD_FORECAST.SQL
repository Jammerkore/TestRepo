--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[METHOD_SPREAD_FORECAST] ( 
	"METHOD_RID" int not null,
	"HN_RID" int not null,
	"FV_RID" int not null,
	"CDR_RID" int not null,
	"SPREAD_OPTION" int not null,
	"IGNORE_LOCKS" char(1) not null,
	"MULTI_LEVEL_IND" char(1) null,
	"FROM_LEVEL_TYPE" int null,
	"FROM_LEVEL_SEQ" int null,
	"FROM_LEVEL_OFFSET" int null,
	"TO_LEVEL_TYPE" int null,
	"TO_LEVEL_SEQ" int null,
	"TO_LEVEL_OFFSET" int null,	
	"LOW_LEVEL_TYPE" int null,
	"LOW_LEVEL_SEQ" int null,
	"LOW_LEVEL_OFFSET" int null,
	"EQUALIZE_WEIGHTING" char(1) null,
	"OLL_RID" int null)  

go
