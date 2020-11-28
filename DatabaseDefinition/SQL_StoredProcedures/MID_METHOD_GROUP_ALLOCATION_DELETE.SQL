--dv =============================================
--dv Create date:	8/15/2014
--dv Description:	Deletes a group allocation method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GROUP_ALLOCATION_DELETE]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_GROUP_ALLOCATION
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO
