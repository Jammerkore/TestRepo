--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Deletes a global lock method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GLOBAL_LOCK_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_GLOBAL_LOCK 
    WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO

















