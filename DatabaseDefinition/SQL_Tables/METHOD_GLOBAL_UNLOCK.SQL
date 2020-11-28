--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================   
create table [dbo].[METHOD_GLOBAL_UNLOCK] ( 
	"METHOD_RID" int not null,
	"HN_RID" int not null,
	"FV_RID" int not null,
	"CDR_RID" int not null,
	"MULTI_LEVEL_IND" char(1) null,
	"FROM_LEVEL_TYPE" int null,
	"FROM_LEVEL_SEQ" int null,
	"FROM_LEVEL_OFFSET" int null,
	"TO_LEVEL_TYPE" int null,
	"TO_LEVEL_SEQ" int null,
	"TO_LEVEL_OFFSET" int null,
	"STORE_IND" char(1) null,
	"CHAIN_IND" char(1) null,
	"STORE_FILTER_RID" int null,
	"OLL_RID" int null)  

go
