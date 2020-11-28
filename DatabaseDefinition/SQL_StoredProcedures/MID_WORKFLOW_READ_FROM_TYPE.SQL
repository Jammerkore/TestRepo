--dv =============================================
--dv Create date: 12/09/2013
--dv Description: Get workflow info from type.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_FROM_TYPE] 
	@WORKFLOW_TYPE_ID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM VW_WORKFLOW_READ
	WHERE WORKFLOW_TYPE_ID = @WORKFLOW_TYPE_ID
		AND WORKFLOW_RID > 1

END
GO
