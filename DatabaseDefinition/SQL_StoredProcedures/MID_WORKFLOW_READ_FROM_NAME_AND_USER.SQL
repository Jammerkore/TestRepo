--dv =============================================
--dv Create date: 12/09/2013
--dv Description: Get workflow info from name and user.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_FROM_NAME_AND_USER] 
	@WORKFLOW_NAME VARCHAR(50),
	@WORKFLOW_USER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eProfileType_Workflow INT = 55;

	SELECT *
	FROM VW_WORKFLOW_READ vw
	WHERE vw.WORKFLOW_NAME = @WORKFLOW_NAME
		AND vw.WORKFLOW_USER_RID = @WORKFLOW_USER_RID
		AND vw.ITEM_TYPE = @eProfileType_Workflow

END
GO

