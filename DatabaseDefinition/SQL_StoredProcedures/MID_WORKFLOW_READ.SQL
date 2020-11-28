--dv =============================================
--dv Create date: 12/10/2013
--dv Description: Reads a workflow
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ] 
	@WORKFLOW_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eProfileType_Workflow INT = 55;

	SELECT *
	FROM VW_WORKFLOW_READ
	WHERE WORKFLOW_RID = @WORKFLOW_RID
		  AND ITEM_TYPE = @eProfileType_Workflow

END
GO




