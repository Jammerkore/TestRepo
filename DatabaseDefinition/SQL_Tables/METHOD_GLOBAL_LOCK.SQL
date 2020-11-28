--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
CREATE TABLE [dbo].[METHOD_GLOBAL_LOCK] ( 
	"METHOD_RID" INT not null,
	"HN_RID" INT not null,
	"FV_RID" INT not null,
	"CDR_RID" INT not null,
	"MULTI_LEVEL_IND" CHAR(1) null,
	"FROM_LEVEL_TYPE" INT null,
	"FROM_LEVEL_SEQ" INT null,
	"FROM_LEVEL_OFFSET" INT null,
	"TO_LEVEL_TYPE" INT null,
	"TO_LEVEL_SEQ" INT null,
	"TO_LEVEL_OFFSET" INT null,
	"STORE_IND" CHAR(1) null,
	"CHAIN_IND" CHAR(1) null,
	"STORE_FILTER_RID" INT null,
	"OLL_RID" int null)  
GO
