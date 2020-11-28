--dv =============================================
--dv Modified date: 3/26/2014
--dv Notes:	This table identifies which of the standard BUILD_PACK_CRITERIA Combos associated with this method are SELECTED as options to consider when
--dv Notes: generating packs in a Work Up Buy.  Only the selected combos from the standard table are present in this table.
--dv Associated Tables:  METHOD, METHOD_BLD_PACKS, BUILD_PACK_COMBO_NAME
--dv Fields: BPC_COMBO_RID:  Identifies the standard combination selected. 
--dv History: 
--dv =============================================
create table [dbo].[METHOD_BLD_PACKS_BPC_SELECT] (
    "METHOD_RID" int not null,
    "BPC_COMBO_RID" int not null)	
go
