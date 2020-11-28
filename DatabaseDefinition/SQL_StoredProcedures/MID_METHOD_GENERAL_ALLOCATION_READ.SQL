--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Reads a general allocation method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GENERAL_ALLOCATION_READ] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		BEGIN_CDR_RID, 
		SHIP_TO_CDR_RID, 
		MERCH_HN_RID,
		MERCH_PH_RID, 
		MERCH_PHL_SEQUENCE, 
		MERCH_TYPE,
		GEN_ALLOC_HDR_RID, 
		RESERVE, 
		PERCENT_IND, 
		COALESCE(RESERVE_AS_BULK,0) as RESERVE_AS_BULK, 
		COALESCE(RESERVE_AS_PACKS,0) as RESERVE_AS_PACKS
	FROM METHOD_GENERAL_ALLOCATION 
	WHERE METHOD_RID = @METHOD_RID

END
GO






