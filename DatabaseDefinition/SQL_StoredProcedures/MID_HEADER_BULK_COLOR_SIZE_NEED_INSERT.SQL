--dv =============================================
--dv Create date:	10/17/2013
--dv Modified date: 2/19/2014
--dv Description:	Inserts a header color curve aka bulk color size need for the header
--dv Reminder:		Nulls passed in will be nulls on the row
--dv History:		TT#848-MD -jsobek -Fill to Size Plan Presentation
--dv =============================================
CREATE PROCEDURE [dbo].[MID_HEADER_BULK_COLOR_SIZE_NEED_INSERT]
	@HDR_RID int,
	@HDR_BC_RID int,
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
	@VSW_SIZE_CONSTRAINTS int,
	@FILL_SIZES_TO_TYPE int
AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO HEADER_BULK_COLOR_SIZE_NEED WITH (ROWLOCK) 
		(
		HDR_RID, 
		HDR_BC_RID,
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
		VSW_SIZE_CONSTRAINTS,
		FILL_SIZES_TO_TYPE
		)
	VALUES
		(
		@HDR_RID, 
		@HDR_BC_RID,
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
		@VSW_SIZE_CONSTRAINTS,
		@FILL_SIZES_TO_TYPE
		)

	SELECT @@ROWCOUNT;

END
GO
