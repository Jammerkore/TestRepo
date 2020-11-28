--dv =============================================
--dv Create date: 12/09/2013
--dv Description: Get the workflow info from the type and user.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_FROM_TYPE_AND_USER] 
	@WORKFLOW_TYPE_ID INT,
	@USER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eProfileType_Workflow INT = 55;

	SELECT *
	FROM VW_WORKFLOW_READ ui
	WHERE ui.WORKFLOW_TYPE_ID = @WORKFLOW_TYPE_ID
		AND ui.WORKFLOW_USER_RID = @USER_RID
		AND ui.ITEM_TYPE = @eProfileType_Workflow
		AND WORKFLOW_RID > 1
	ORDER BY WORKFLOW_NAME

END
GO

