--dv =============================================
--dv Create date: 12/11/2013
--dv Description:	Delete workflow step component by workflow RID, step number, component type and component RID.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ALLOCATION_STEP_COMPONENT_DELETE_FROM_CRITERIA]
	@WORKFLOW_RID INT,
	@STEP_NUMBER INT,
	@COMP_TYPE_ID INT,
	@COMP_CRIT_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM ALLOCATION_STEP_COMPONENT
	WHERE WORKFLOW_RID = @WORKFLOW_RID
		AND STEP_NUMBER = @STEP_NUMBER
		AND COMP_TYPE_ID = @COMP_TYPE_ID
		AND COMP_CRIT_RID = @COMP_CRIT_RID

	SELECT @@ROWCOUNT

END
GO





















