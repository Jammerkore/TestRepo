--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	This table is a sub-table belonging to METHOD, METHOD_BLD_PACKS, METHOD_BLD_PACKS_COMBO and METHOD_BLD_PACKS_COMBO_PATTERN.  Its purpose is to record the size run for a pattern pack.
--dv Associated Tables:  METHOD, METHOD_BLD_PACKS, METHOD_BLD_PACKS_COMBO_PATTERN
--dv Fields: PATTERN_SIZE_CODE_RID:  Identifies a size within the size run of a pack pattern
--dv Fields: PATTERN_SIZE_UNITS: Number of units of the size within the size run of a pack pattern
--dv History: 
--dv ============================================= 
create table [dbo].[METHOD_BLD_PACKS_COMBO_PATTERN_SZRUN] (
    "METHOD_RID" int not null,
    "COMBO_RID" int not null,
    "PATTERN_PACK_RID" int not null,
    "SIZE_CODE_RID" int not null,
    "PATTERN_SIZE_UNITS" int not null )
go
