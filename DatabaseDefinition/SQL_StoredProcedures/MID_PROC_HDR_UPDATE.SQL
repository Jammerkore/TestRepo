--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Updates the process audit header table based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PROC_HDR_UPDATE]
	@PROCESS_RID int,
	@COMPLETION_STATUS_CODE int,
	@EXECUTION_STATUS_CODE int,
	@STOP_TIME datetime,
	@SUMMARY_CODE int,
	@HIGHEST_LEVEL int,
	@PROC_DESC char
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE PROC_HDR 
	SET 
		COMPLETION_STATUS_CODE = @COMPLETION_STATUS_CODE,
		EXECUTION_STATUS_CODE = @EXECUTION_STATUS_CODE,
		STOP_TIME = @STOP_TIME,
		SUMMARY_CODE = @SUMMARY_CODE,
		HIGHEST_LEVEL = @HIGHEST_LEVEL,
		PROC_DESC = @PROC_DESC
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
