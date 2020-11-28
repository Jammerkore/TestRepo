--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Deletes a spread forecast methods child basis details
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SPREAD_BASIS_DETAIL_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_SPREAD_BASIS_DETAIL 
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO




