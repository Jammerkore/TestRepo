--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: TT#667 - stodd
--dv =============================================    
create table [dbo].[METHOD_GENERAL_ALLOCATION] ( 
	"METHOD_RID" int not null,
	"BEGIN_CDR_RID" int null,
	"SHIP_TO_CDR_RID" int null,
	"MERCH_TYPE" int not null,
	"MERCH_HN_RID" int null,
	"MERCH_PH_RID" int null,
	"MERCH_PHL_SEQUENCE" int null,
	"GEN_ALLOC_HDR_RID" int null,
	"RESERVE" float null,
	"PERCENT_IND" char(1) null,
	"RESERVE_AS_BULK" float null,
	"RESERVE_AS_PACKS" float null)	
  
go
