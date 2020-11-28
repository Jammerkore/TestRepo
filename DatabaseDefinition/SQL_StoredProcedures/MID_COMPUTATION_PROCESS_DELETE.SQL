--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Deletes rows from the computation process table based on the process rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_PROCESS_DELETE]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM COMPUTATION_PROCESS
	WHERE PROCESS_RID = @PROCESS_RID

	SELECT @@ROWCOUNT;

END
GO
