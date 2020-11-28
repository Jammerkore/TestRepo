--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Reads the user for the given workflow
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_USER]
	@WORKFLOW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		WORKFLOW_USER_RID 
	FROM WORKFLOW 
	WHERE WORKFLOW_RID = @WORKFLOW_RID

END
GO


