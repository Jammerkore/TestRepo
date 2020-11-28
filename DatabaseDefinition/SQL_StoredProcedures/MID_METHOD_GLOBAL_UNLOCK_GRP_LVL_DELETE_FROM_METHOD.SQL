--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Deletes global unlocks for group levels based on the method provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GLOBAL_UNLOCK_GRP_LVL_DELETE_FROM_METHOD] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_GLOBAL_UNLOCK_GRP_LVL
    WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO