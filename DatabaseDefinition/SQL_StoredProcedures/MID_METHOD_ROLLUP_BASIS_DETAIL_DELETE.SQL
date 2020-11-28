--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Deletes rollup method basis details
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_ROLLUP_BASIS_DETAIL_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_ROLLUP_BASIS_DETAIL 
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
END
GO