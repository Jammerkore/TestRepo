--dv =============================================
--dv Create date: 12/10/2013
--dv Description: Deletes export method child variables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_EXPORT_VARIABLES_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_EXPORT_VARIABLES 
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO