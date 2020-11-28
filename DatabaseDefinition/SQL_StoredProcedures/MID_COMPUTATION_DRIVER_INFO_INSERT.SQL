--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the computation driver info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_DRIVER_INFO_INSERT]
	@PROCESS_RID int,
	@TOTAL_ITEMS int, 
	@CONCURRENT_PROCESSES int, 
	@RECS_WITH_ERRORS int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO COMPUTATION_DRIVER_INFO
		(
		PROCESS_RID, 
		TOTAL_ITEMS, 
		CONCURRENT_PROCESSES, 
		RECS_WITH_ERRORS
		)
	VALUES
		(
		@PROCESS_RID, 
		@TOTAL_ITEMS, 
		@CONCURRENT_PROCESSES, 
		@RECS_WITH_ERRORS
		)

	SELECT @@ROWCOUNT;

END
GO
