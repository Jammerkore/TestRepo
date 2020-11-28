--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Inserts a row into the special request info table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_INFO_INSERT]
	@PROCESS_RID int,
	@TOTAL_JOBS int, 
	@JOBS_PROCESSED int, 
	@JOBS_WITH_ERRORS int, 
	@SUCCESSFUL_JOBS int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO SPECIAL_REQUEST_INFO
		(
		PROCESS_RID, 
		TOTAL_JOBS, 
		JOBS_PROCESSED, 
		JOBS_WITH_ERRORS, 
		SUCCESSFUL_JOBS
		)
	VALUES
		(
		@PROCESS_RID, 
		@TOTAL_JOBS, 
		@JOBS_PROCESSED, 
		@JOBS_WITH_ERRORS, 
		@SUCCESSFUL_JOBS
		)

	SELECT @@ROWCOUNT;

END
GO
