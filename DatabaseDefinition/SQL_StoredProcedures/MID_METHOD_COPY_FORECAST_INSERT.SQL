--dv =============================================
--dv Create date:	12/10/2013
--dv Description:	Inserts a row into the method copy forecast table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_COPY_FORECAST_INSERT]
	@METHOD_RID int,
	@HN_RID int, 
	@FV_RID int, 
	@CDR_RID int, 
	@PLAN_TYPE int, 
	@STORE_FILTER_RID int, 
	@MULTI_LEVEL_IND bit, 
    @FROM_LEVEL_TYPE int, 
	@FROM_LEVEL_SEQ int, 
	@FROM_LEVEL_OFFSET int, 
	@TO_LEVEL_TYPE int, 
	@TO_LEVEL_SEQ int, 
	@TO_LEVEL_OFFSET int, 
	@OLL_RID int, 
	@PREINIT_VALUES_IND char(1)
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_COPY_FORECAST
		(
		METHOD_RID, 
		HN_RID, 
		FV_RID, 
		CDR_RID, 
		PLAN_TYPE, 
		STORE_FILTER_RID, 
        MULTI_LEVEL_IND, 
		FROM_LEVEL_TYPE, 
		FROM_LEVEL_SEQ, 
		FROM_LEVEL_OFFSET, 
		TO_LEVEL_TYPE, 
		TO_LEVEL_SEQ, 
		TO_LEVEL_OFFSET, 
		OLL_RID, 
		PREINIT_VALUES_IND
		) 
	VALUES 
		(
		@METHOD_RID, 
		@HN_RID, 
		@FV_RID, 
		@CDR_RID, 
		@PLAN_TYPE, 
		@STORE_FILTER_RID, 
		@MULTI_LEVEL_IND, 
        @FROM_LEVEL_TYPE, 
		@FROM_LEVEL_SEQ, 
		@FROM_LEVEL_OFFSET, 
		@TO_LEVEL_TYPE, 
		@TO_LEVEL_SEQ, 
		@TO_LEVEL_OFFSET, 
		@OLL_RID, 
		@PREINIT_VALUES_IND
		)

	SELECT @@ROWCOUNT
END
GO