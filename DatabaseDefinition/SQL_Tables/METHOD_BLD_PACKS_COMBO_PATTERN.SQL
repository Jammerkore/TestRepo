--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	This table identifies the custome pack combinations that are valid for a Work Up Buy
--dv Associated Tables:  METHOD, METHOD_BLD_PACKS, METHOD_BLD_PACKS_COMBO, METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN
--dv Fields: COMBO_RID:  Identifies the name of the Combination.
--dv Fields: PATTERN_PACK_RID:  Identifies a pack multiple and other pack characteristics within this combination
--dv Fields: COMBO_MAX_PATTERN_PACKS:  The maximum number of packs with these characteristics that may be generated for a Work Up Buy (this value will be 1 when there is a size run associated with the pack configuration.)
--dv History: 
--dv =============================================
create table [dbo].[METHOD_BLD_PACKS_COMBO_PATTERN] ( 
	"METHOD_RID" int not null,
	"COMBO_RID" int not null,
	"PATTERN_PACK_RID" int identity(100,1) not null,
	"PATTERN_NAME" varchar(50) not null,
	"PATTERN_PACK_MULT" int not null,
	"COMBO_MAX_PATTERN_PACKS" int not null)
create clustered index "METHOD_BLD_PACKS_COMBO_PATTERN_IDX" ON "METHOD_BLD_PACKS_COMBO_PATTERN" ("METHOD_RID", "COMBO_RID", "PATTERN_PACK_RID")
go	
