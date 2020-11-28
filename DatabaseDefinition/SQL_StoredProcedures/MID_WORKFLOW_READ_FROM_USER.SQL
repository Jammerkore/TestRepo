--dv =============================================
--dv Create date:	12/10/2013
--dv Description:	Reads a worflow based on the user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_READ_FROM_USER]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @GlobalUserRID INT = 4;

	SELECT * 
	FROM VW_WORKFLOW_READ
	WHERE (WORKFLOW_USER = @GlobalUserRID
           OR WORKFLOW_USER = @USER_RID
		   OR WORKFLOW_USER_RID = @USER_RID
		   )
		   AND WORKFLOW_RID > 1

END
GO