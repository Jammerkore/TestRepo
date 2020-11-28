--dv =============================================
--dv Create date: 12/05/2013
--dv Description: Delete workflow change history.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_CHANGE_HISTORY_DELETE] 
	@WORKFLOW_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM WORKFLOW_CHANGE_HISTORY
	WHERE WORKFLOW_RID = @WORKFLOW_RID

	RETURN @@ROWCOUNT;
END
GO



