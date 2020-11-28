--dv =============================================
--dv Create date: 12/09/2013
--dv Description: Get workflow info from RID.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_FROM_RID] 
	@WORKFLOW_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM VW_WORKFLOW_READ
	WHERE WORKFLOW_RID = @WORKFLOW_RID

END
GO