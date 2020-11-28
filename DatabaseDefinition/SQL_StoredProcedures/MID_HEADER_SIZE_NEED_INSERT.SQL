--dv =============================================
--dv Create date:	10/17/2013
--dv Description:	Inserts a header detail curve aka size need for the header
--dv Reminder:		Nulls passed in will be nulls on the row
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_SIZE_NEED_INSERT]
	@HDR_RID int,
	@SIZE_CURVE_GROUP_RID int, 
	@MERCH_TYPE int, 
	@HN_RID int, 
	@PH_RID int, 
	@PHL_SEQUENCE int, 
	@SIZE_CONSTRAINT_RID int, 
	@SIZE_ALTERNATE_RID int, 
	@NORMALIZE_SIZE_CURVES_IND char, 
	@IB_MERCH_TYPE int, 
	@IB_MERCH_HN_RID int, 
	@IB_MERCH_PH_RID int, 
	@IB_MERCH_PHL_SEQUENCE int, 
	@VSW_SIZE_CONSTRAINTS int
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO HEADER_SIZE_NEED WITH (ROWLOCK) 
		(
		HDR_RID, 
		SIZE_CURVE_GROUP_RID, 
		MERCH_TYPE, 
		HN_RID, 
		PH_RID, 
		PHL_SEQUENCE, 
		SIZE_CONSTRAINT_RID, 
		SIZE_ALTERNATE_RID, 
		NORMALIZE_SIZE_CURVES_IND, 
		IB_MERCH_TYPE, 
		IB_MERCH_HN_RID, 
		IB_MERCH_PH_RID, 
		IB_MERCH_PHL_SEQUENCE, 
		VSW_SIZE_CONSTRAINTS
		)
	VALUES
		(
		@HDR_RID, 
		@SIZE_CURVE_GROUP_RID, 
		@MERCH_TYPE, 
		@HN_RID, 
		@PH_RID, 
		@PHL_SEQUENCE, 
		@SIZE_CONSTRAINT_RID, 
		@SIZE_ALTERNATE_RID, 
		@NORMALIZE_SIZE_CURVES_IND, 
		@IB_MERCH_TYPE, 
		@IB_MERCH_HN_RID, 
		@IB_MERCH_PH_RID, 
		@IB_MERCH_PHL_SEQUENCE, 
		@VSW_SIZE_CONSTRAINTS
		)
	
	SELECT @@ROWCOUNT;

END
GO
