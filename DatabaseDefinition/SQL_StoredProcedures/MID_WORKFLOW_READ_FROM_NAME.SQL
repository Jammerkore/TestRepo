--dv =============================================
--dv Create date: 12/9/2013
--dv Description:	Get workflow info from name.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_FROM_NAME] 
	@WORKFLOW_NAME VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM VW_WORKFLOW_READ
	WHERE WORKFLOW_NAME = @WORKFLOW_NAME

END
GO

