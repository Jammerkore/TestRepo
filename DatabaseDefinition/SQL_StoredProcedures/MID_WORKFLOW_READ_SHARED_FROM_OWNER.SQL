--dv =============================================
--dv Create date: 12/10/2013
--dv Description: Get workflow info from owner but not shared owners.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_SHARED_FROM_OWNER] 
	@USER_RID INT,
	@OWNER_USER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eProfileType_Workflow INT = 55;

	SELECT *
	FROM VW_WORKFLOW_READ vw
	WHERE vw.WORKFLOW_USER = @USER_RID
		AND vw.ITEM_TYPE = @eProfileType_Workflow
		AND vw.WORKFLOW_RID > 1
		AND vw.WORKFLOW_USER <> vw.OWNER_USER_RID
		AND vw.OWNER_USER_RID = @OWNER_USER_RID

END
GO

