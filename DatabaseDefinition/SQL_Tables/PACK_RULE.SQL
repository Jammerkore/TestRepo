--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	
--dv History: 
--dv ============================================= 
create table [dbo].[PACK_RULE] ( 
	"HDR_PACK_RID" int not null,
	"LAYER_ID" int not null,
	"ST_RID" int not null,
	"RULE_TYPE_ID" int not null,
	"PACKS" int null)  

go
