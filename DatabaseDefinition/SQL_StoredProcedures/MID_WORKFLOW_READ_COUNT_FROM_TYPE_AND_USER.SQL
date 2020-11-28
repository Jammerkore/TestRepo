--dv =============================================
--dv Create date: 12/03/2013
--dv Description: Get the number of workflows based on the user and type.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_COUNT_FROM_TYPE_AND_USER]
	@WORKFLOW_TYPE_ID int,
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eProfileType_Workflow int = 55;

	SELECT 
		COUNT(*) AS MyCount
	FROM WORKFLOW w
	INNER JOIN USER_ITEM ui ON ui.ITEM_RID = w.WORKFLOW_RID
	WHERE WORKFLOW_TYPE_ID = @WORKFLOW_TYPE_ID
		AND ui.USER_RID = @USER_RID
		AND ui.ITEM_TYPE = @eProfileType_Workflow
		AND WORKFLOW_RID > 1

END
GO


