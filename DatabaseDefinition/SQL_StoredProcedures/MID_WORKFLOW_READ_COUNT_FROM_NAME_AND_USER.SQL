--dv =============================================
--dv Create date: 12/03/2013
--dv Description: Get the count of workflows by name and user.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_COUNT_FROM_NAME_AND_USER]
	@WORKFLOW_TYPE_ID int,
	@WORKFLOW_USER_RID int,
	@WORKFLOW_NAME varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount
	FROM WORKFLOW
	WHERE WORKFLOW_TYPE_ID = @WORKFLOW_TYPE_ID
		AND WORKFLOW_USER_RID = @WORKFLOW_USER_RID
		AND WORKFLOW_NAME = @WORKFLOW_NAME

END
GO
