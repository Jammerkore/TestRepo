--dv =============================================
--dv Create date:	10/16/2013
--dv Modified date: 2/19/2014
--dv Description:	Gets bulk color size curve data for the header
--dv History: TT#848-MD -jsobek -Fill to Size Plan Presentation
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_SIZE_NEED_READ]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @MerchandiseType_Undefined int = 3;
	DECLARE @VSWSizeConstraints_None int = 802370;
	DECLARE @eFillSizesToType_Holes int = 802850;

	SELECT 
		hbc.HDR_RID AS HDR_RID, 
		hbc.HDR_BC_RID AS HDR_BC_RID, 
		hbc.COLOR_CODE_RID AS COLOR_CODE_RID,
        COALESCE(hbcsn.SIZE_CURVE_GROUP_RID, -1) AS SIZE_CURVE_GROUP_RID, 
		hbcsn.MERCH_TYPE AS MERCH_TYPE,
        COALESCE(hbcsn.HN_RID, -1) AS HN_RID, 
		COALESCE(hbcsn.PH_RID, -1) AS PH_RID, 
		COALESCE(hbcsn.PHL_SEQUENCE, 0) AS PHL_SEQUENCE,
        COALESCE(hbcsn.SIZE_CONSTRAINT_RID, -1) AS SIZE_CONSTRAINT_RID, 
		COALESCE(hbcsn.SIZE_ALTERNATE_RID, -1) AS SIZE_ALTERNATE_RID,
        COALESCE(hbcsn.NORMALIZE_SIZE_CURVES_IND, 1) AS NORMALIZE_SIZE_CURVES_IND,
        COALESCE(IB_MERCH_TYPE, @MerchandiseType_Undefined) AS IB_MERCH_TYPE,
		COALESCE(IB_MERCH_HN_RID, -1) AS IB_MERCH_HN_RID,
        COALESCE(IB_MERCH_PH_RID, -1) AS IB_MERCH_PH_RID,
        COALESCE(IB_MERCH_PHL_SEQUENCE, 0) AS IB_MERCH_PHL_SEQUENCE,
        COALESCE(VSW_SIZE_CONSTRAINTS, @VSWSizeConstraints_None) AS VSW_SIZE_CONSTRAINTS,
		COALESCE(FILL_SIZES_TO_TYPE, @eFillSizesToType_Holes) AS FILL_SIZES_TO_TYPE
	FROM HEADER_BULK_COLOR hbc WITH (NOLOCK), 
	     HEADER_BULK_COLOR_SIZE_NEED hbcsn WITH (NOLOCK)
    WHERE hbc.HDR_RID = @HDR_RID
          AND hbc.HDR_RID = hbcsn.HDR_RID 
		  AND hbc.HDR_BC_RID = hbcsn.HDR_BC_RID

END
GO
