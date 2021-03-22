--dv =============================================
--dv Create date:	3/19/2021
--dv Description:	Deletes splitters for a plan view
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PLAN_VIEW_SPLITTER_DELETE] 
	@VIEW_RID int,
	@USER_RID int,
	@PLAN_SESSION_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM PLAN_VIEW_SPLITTER 
	WHERE VIEW_RID = @VIEW_RID
	  AND USER_RID = @USER_RID
	  AND PLAN_SESSION_TYPE = @PLAN_SESSION_TYPE

	SELECT @@ROWCOUNT;

END
GO