--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[METHOD_ROLLUP_BASIS_DETAIL] ( 
	"METHOD_RID" int not null,
	"DETAIL_SEQ" int not null,
	"FROM_LEVEL_HRID" int null,
	"FROM_LEVEL_TYPE" int null,
	"FROM_LEVEL_SEQ" int null,
	"FROM_LEVEL_OFFSET" int null,
	"TO_LEVEL_HRID" int null,
	"TO_LEVEL_TYPE" int null,
	"TO_LEVEL_SEQ" int null,
	"TO_LEVEL_OFFSET" int null,
	STORE_IND CHAR(1) not null,
	CHAIN_IND CHAR(1) not null,
	STORE_TO_CHAIN_IND CHAR(1) default '0' not null)

go
