--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Updates a computation process
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_PROCESS_UPDATE]
	@COMPUTATION_RID int,
	@STATUS_CODE int,
	@STOP_TIME datetime
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE COMPUTATION_PROCESS 
	SET 
		STATUS_CODE = @STATUS_CODE,
		STOP_TIME = @STOP_TIME
	WHERE COMPUTATION_RID = @COMPUTATION_RID

	SELECT @@ROWCOUNT;

END
GO
