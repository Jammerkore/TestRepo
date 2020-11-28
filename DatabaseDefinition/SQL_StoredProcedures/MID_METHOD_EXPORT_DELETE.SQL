--dv =============================================
--dv Create date: 12/10/2013
--dv Description: Deletes an export method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_EXPORT_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM METHOD_EXPORT
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO