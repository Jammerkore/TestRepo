--dv =============================================
--dv Create date: 12/06/2013
--dv Description: Get the workflow RID from the workflow name.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_RID_FROM_NAME]
	@WORKFLOW_NAME VARCHAR(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT WORKFLOW_RID
	FROM WORKFLOW WITH (NOLOCK)
	WHERE WORKFLOW_NAME = @WORKFLOW_NAME
END
GO









