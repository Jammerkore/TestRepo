--dv =============================================
--dv Create date: 12/06/2013
--dv Description: Delete a given workflow.
--dv Review: Ensure child tables are deleted before calling this stored procedure
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_DELETE]
	@WORKFLOW_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM WORKFLOW
	WHERE WORKFLOW_RID = @WORKFLOW_RID

	RETURN @@ROWCOUNT
END
GO




