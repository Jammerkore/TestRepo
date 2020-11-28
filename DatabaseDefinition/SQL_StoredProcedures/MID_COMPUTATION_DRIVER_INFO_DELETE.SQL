--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Deletes rows from the computation driver info table based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_DRIVER_INFO_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM COMPUTATION_DRIVER_INFO 
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
