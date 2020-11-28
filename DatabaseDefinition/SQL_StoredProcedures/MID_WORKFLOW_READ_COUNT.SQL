--dv =============================================
--dv Create date: 1/31/2014
--dv Description: Reads the workflow count based on the parameters passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_COUNT]
		@WORKFLOW_NAME varchar(50), 
		@WORKFLOW_TYPE_ID int, 
		@WORKFLOW_USER_RID int,
        @WORKFLOW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT COUNT(*) AS MyCount FROM WORKFLOW
	WHERE WORKFLOW_NAME = @WORKFLOW_NAME
		  AND WORKFLOW_TYPE_ID = @WORKFLOW_TYPE_ID
		  AND WORKFLOW_USER_RID = @WORKFLOW_USER_RID
		  AND WORKFLOW_RID != @WORKFLOW_RID

END
GO
