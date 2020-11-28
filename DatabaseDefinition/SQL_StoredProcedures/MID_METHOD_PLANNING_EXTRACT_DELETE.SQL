--dv =============================================
--dv Create date: 2/23/2018
--dv Description: Deletes an Planning Extract method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_PLANNING_EXTRACT_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM METHOD_PLANNING_EXTRACT
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO