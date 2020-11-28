--dv =============================================
--dv Create date:	12/9/2013
--dv Description:	Deletes rows from the method matrix basis details table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MATRIX_BASIS_DETAILS_DELETE]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_MATRIX_BASIS_DETAILS
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT

END
GO