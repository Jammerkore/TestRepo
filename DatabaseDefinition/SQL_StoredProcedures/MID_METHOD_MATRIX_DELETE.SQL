--dv =============================================
--dv Create date:	12/9/2013
--dv Description:	Deletes rows from the method matrix table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MATRIX_DELETE]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM METHOD_MATRIX	
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT

END
GO