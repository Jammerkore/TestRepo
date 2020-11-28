--dv =============================================
--dv Create date:	12/02/2013
--dv Description:	Deletes size basis substitutes for the given method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SIZE_BASIS_SUBSTITUTES_DELETE]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_SIZE_BASIS_SUBSTITUTES 
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
END
GO