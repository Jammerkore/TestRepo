--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Delete a workflow step allocation based on a workflow and step number.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_STEP_ALLOCATION_DELETE]
	@WORKFLOW_RID INT,
	@STEP_NUMBER INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM WORKFLOW_STEP_ALLOCATION
	WHERE WORKFLOW_RID = @WORKFLOW_RID
		AND STEP_NUMBER = @STEP_NUMBER

	SELECT @@ROWCOUNT

END
GO























