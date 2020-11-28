--dv =============================================
--dv Create date: 11/13/2013
--dv Description:	Deletes a workflow step based on workflow RID and Step Number. 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_STEP_OTSPLAN_DELETE]
	@WORKFLOW_RID INT,
	@STEP_NUMBER INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM WORKFLOW_STEP_OTSPLAN
	WHERE WORKFLOW_RID = @WORKFLOW_RID
		AND STEP_NUMBER = @STEP_NUMBER

	SELECT @@ROWCOUNT;
END
GO
