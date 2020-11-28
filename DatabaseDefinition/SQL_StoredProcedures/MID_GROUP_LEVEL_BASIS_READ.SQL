--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Reads group level basis by method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GROUP_LEVEL_BASIS_READ] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eMerchandiseType_Node int = 1;
	DECLARE @NoRID int = -1;

	SELECT 
		METHOD_RID, 
		SGL_RID, 
		BASIS_SEQ, 
		HN_RID, 
		FV_RID, 
		CDR_RID, 
		WEIGHT, 
		INC_EXC_IND, 
		TYLY_TYPE_ID,																										 
		COALESCE(MERCH_TYPE, @eMerchandiseType_Node) MERCH_TYPE,
		COALESCE(MERCH_PH_RID, @NoRID) MERCH_PH_RID,
		COALESCE(MERCH_PHL_SEQUENCE, 0) MERCH_PHL_SEQUENCE,
		COALESCE(MERCH_OFFSET, 0) MERCH_OFFSET
	FROM GROUP_LEVEL_BASIS 
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY SGL_RID, BASIS_SEQ

END
GO