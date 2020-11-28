--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Returns Method Size Curve Merchadise Basis Detail
--dv Modifed:   	2/19/2013 - TT#1557-MD - RMatelic
--dv                Change RID COALESCE values from 1 to -1
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_READ]
	@METHOD_RID int,
	@MERCH_TYPE int

AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		BASIS_SEQ,
		COALESCE(HN_RID, -1) HN_RID,
		COALESCE(FV_RID, -1) FV_RID,
		COALESCE(CDR_RID, -1) CDR_RID,
		COALESCE(WEIGHT,1) WEIGHT,
		COALESCE(MERCH_TYPE, @MERCH_TYPE) MERCH_TYPE,
		COALESCE(OLL_RID, -1) OLL_RID,
		COALESCE(CUSTOM_OLL_RID, -1) CUSTOM_OLL_RID 
    FROM METHOD_SIZE_CURVE_MRCH_BAS_DET 
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY BASIS_SEQ

END
GO