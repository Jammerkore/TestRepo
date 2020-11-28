--dv =============================================
--dv Create date:	2/17/2014
--dv Description:	Deletes a rollup method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_ROLLUP_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM METHOD_ROLLUP
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
END
GO