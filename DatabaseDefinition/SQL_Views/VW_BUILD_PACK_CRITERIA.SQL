--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Reads build pack criteria
--dv =============================================
CREATE VIEW [dbo].[VW_BUILD_PACK_CRITERIA]
AS

	SELECT 
		bpc.BPC_RID as BPC_RID, 
		bpc.BPC_NAME as BPC_NAME, 
		bpc.BPC_COMP_MIN as PACK_MIN, 
		bpc.BPC_SIZE_MULT as SIZE_MULT, 
        bpco.BPC_COMBO_RID as COMBO_RID, 
		bpco.BPC_COMBO_NAME as COMBO_NAME,
        bpcf.BPC_PACK_RID as PATTERN_RID, 
		bpcf.BPC_PACK_MULT as PATTERN_MULT, 
		bpcf.BPC_COMBO_MAX_PACKS as COMBO_MAX_PACKS
	FROM BUILD_PACK_CRITERIA bpc
    INNER JOIN BUILD_PACK_COMBO bpco on bpc.BPC_RID = bpco.BPC_RID
    INNER JOIN BUILD_PACK_CONFIG bpcf on bpco.BPC_COMBO_RID = bpcf.BPC_COMBO_RID

GO
