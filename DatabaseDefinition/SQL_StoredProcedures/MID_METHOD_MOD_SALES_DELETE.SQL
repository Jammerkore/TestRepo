--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Deletes a modify sales method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MOD_SALES_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_MOD_SALES
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO


