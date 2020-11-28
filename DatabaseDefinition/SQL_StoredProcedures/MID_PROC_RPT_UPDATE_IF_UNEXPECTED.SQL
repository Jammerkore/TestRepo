--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Updates PROC_RPT table when unexpected conditions occur
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PROC_RPT_UPDATE_IF_UNEXPECTED]
	@PROCESS_RID int,
	@COMPLETION_STATUS_CODE int,
	@EXECUTION_STATUS_CODE int,
	@SUMMARY_CODE int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE PROC_HDR 
	SET 
		COMPLETION_STATUS_CODE = @COMPLETION_STATUS_CODE,
		EXECUTION_STATUS_CODE = @EXECUTION_STATUS_CODE,
		SUMMARY_CODE = @SUMMARY_CODE
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
