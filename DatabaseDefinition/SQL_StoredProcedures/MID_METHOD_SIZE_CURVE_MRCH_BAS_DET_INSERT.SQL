--dv =============================================
--dv Create date:	6/24/2014
--dv Description:	Inserts merchandise basis details for a size curve method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SIZE_CURVE_MRCH_BAS_DET_INSERT] 
	@METHOD_RID int,
	@BASIS_SEQ int, 
	@HN_RID int, 
	@FV_RID int, 
	@CDR_RID int,
	@WEIGHT float, 
	@MERCH_TYPE int, 
	@OLL_RID int, 
	@CUSTOM_OLL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_SIZE_CURVE_MRCH_BAS_DET 
	(
		METHOD_RID, 
		BASIS_SEQ, 
		HN_RID, 
		FV_RID, 
		CDR_RID,
		WEIGHT, 
		MERCH_TYPE, 
		OLL_RID, 
		CUSTOM_OLL_RID
	)
	VALUES 
	(
		@METHOD_RID, 
		@BASIS_SEQ, 
		@HN_RID, 
		@FV_RID, 
		@CDR_RID,
		@WEIGHT, 
		@MERCH_TYPE, 
		@OLL_RID, 
		@CUSTOM_OLL_RID
	)

	SELECT @@ROWCOUNT;
END
GO