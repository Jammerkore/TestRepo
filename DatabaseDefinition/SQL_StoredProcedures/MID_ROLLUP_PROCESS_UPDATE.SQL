--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Updates a rollup process
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_PROCESS_UPDATE]
	@ROLLUP_RID int,
	@STATUS_CODE int,
	@STOP_TIME datetime
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE ROLLUP_PROCESS 
	SET
		STATUS_CODE = @STATUS_CODE,
		STOP_TIME = @STOP_TIME 
	WHERE ROLLUP_RID = @ROLLUP_RID

	SELECT @@ROWCOUNT;
END
GO