--dv =============================================
--dv Create date: 12/06/2013
--dv Description: Get workflow key info from user and name.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_KEY_FROM_USER_AND_NAME] 
	@USER_RID INT,
	@WORKFLOW_NAME VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM WORKFLOW
	WHERE WORKFLOW_USER_RID = @USER_RID
		AND WORKFLOW_NAME = @WORKFLOW_NAME
END
GO



