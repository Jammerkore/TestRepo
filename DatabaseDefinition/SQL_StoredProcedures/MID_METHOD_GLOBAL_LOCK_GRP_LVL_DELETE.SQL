--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Deletes a global lock methods group levels
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GLOBAL_LOCK_GRP_LVL_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_GLOBAL_LOCK_GRP_LVL
    WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO
















