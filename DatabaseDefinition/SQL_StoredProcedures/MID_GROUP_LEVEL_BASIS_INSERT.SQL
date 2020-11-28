--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Insert a row into the group level basis table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GROUP_LEVEL_BASIS_INSERT] 
	@METHOD_RID int, 
	@SGL_RID int, 
	@BASIS_SEQ int, 
	@HN_RID int, 
	@FV_RID int,
	@CDR_RID int, 
	@WEIGHT float, 
	@INC_EXC_IND char(1), 
	@TYLY_TYPE_ID int, 
	@MERCH_TYPE int,
	@MERCH_PH_RID int, 
	@MERCH_PHL_SEQUENCE int, 
	@MERCH_OFFSET int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO GROUP_LEVEL_BASIS
		(
		METHOD_RID, 
		SGL_RID, 
		BASIS_SEQ, 
		HN_RID, 
		FV_RID,
		CDR_RID, 
		WEIGHT, 
		INC_EXC_IND, 
		TYLY_TYPE_ID, 
		MERCH_TYPE,
		MERCH_PH_RID, 
		MERCH_PHL_SEQUENCE,
		MERCH_OFFSET
		) 
	VALUES
		(
		@METHOD_RID, 
		@SGL_RID, 
		@BASIS_SEQ, 
		@HN_RID, 
		@FV_RID,
		@CDR_RID, 
		@WEIGHT, 
		@INC_EXC_IND, 
		@TYLY_TYPE_ID, 
		@MERCH_TYPE,
		@MERCH_PH_RID, 
		@MERCH_PHL_SEQUENCE,
		@MERCH_OFFSET
		)

	SELECT @@ROWCOUNT;
END
GO