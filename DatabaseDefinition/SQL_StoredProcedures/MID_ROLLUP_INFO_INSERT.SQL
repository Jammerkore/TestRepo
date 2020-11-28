--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the rollup info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_INFO_INSERT]
	@PROCESS_RID int,
	@TOTAL_ITEMS int, 
	@BATCH_SIZE int, 
	@CONCURRENT_PROCESSES int,
	@TOTAL_BATCHES int, 
	@RECS_WITH_ERRORS int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO ROLLUP_INFO
		(
		PROCESS_RID, 
		TOTAL_ITEMS, 
		BATCH_SIZE, 
		CONCURRENT_PROCESSES,
		TOTAL_BATCHES, 
		RECS_WITH_ERRORS
		) 
	VALUES
		(
		@PROCESS_RID, 
		@TOTAL_ITEMS, 
		@BATCH_SIZE, 
		@CONCURRENT_PROCESSES,
		@TOTAL_BATCHES, 
		@RECS_WITH_ERRORS
		) 

	SELECT @@ROWCOUNT;

END
GO
