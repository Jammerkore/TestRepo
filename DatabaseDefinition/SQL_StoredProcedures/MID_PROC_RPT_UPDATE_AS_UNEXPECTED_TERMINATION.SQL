--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Updates all rows in PROC_RPT table that are marked as running when unexpected termination occurs
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PROC_RPT_UPDATE_AS_UNEXPECTED_TERMINATION]
	@COMPLETION_STATUS_CODE int,
	@EXECUTION_STATUS_CODE int,
	@SUMMARY_CODE int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @EXECUTION_STATUS_RUNNING int
	SET @EXECUTION_STATUS_RUNNING = 801602

	UPDATE PROC_HDR 
	SET 
		COMPLETION_STATUS_CODE = @COMPLETION_STATUS_CODE,
		EXECUTION_STATUS_CODE = @EXECUTION_STATUS_CODE,
		SUMMARY_CODE = @SUMMARY_CODE
    WHERE EXECUTION_STATUS_CODE = @EXECUTION_STATUS_RUNNING
	  AND PROCESS_ID not in (800413,800414,800415,800419,800421)  -- update all processes except for services other than control service

	SELECT @@ROWCOUNT;

END
GO
