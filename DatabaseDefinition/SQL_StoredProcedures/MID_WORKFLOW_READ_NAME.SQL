--dv =============================================
--dv Create date: 12/06/2013
--dv Description: Get the workflow name.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_NAME]
	@WORKFLOW_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT WORKFLOW_NAME
	FROM WORKFLOW
	WHERE WORKFLOW_RID = @WORKFLOW_RID
END
GO






