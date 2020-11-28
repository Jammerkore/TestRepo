--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv =============================================  
create table [dbo].[HEADER_PACK] ( 
	"HDR_PACK_RID" int identity not null,
	"HDR_RID" int not null,
	"HDR_PACK_NAME" varchar(100) not null,
	"PACKS" int default 0 not null,
	"MULTIPLE" int default 1 not null,
	"RESERVE_PACKS" int null,
	"GENERIC_IND" char(1) default '1' not null,
	"ASSOCIATED_PACK_RID" int null,
	"SEQ" int null) ON 'ALLOCATION'  

go
