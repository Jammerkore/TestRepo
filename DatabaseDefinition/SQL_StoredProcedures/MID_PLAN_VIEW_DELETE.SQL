--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Deletes a plan view
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PLAN_VIEW_DELETE]
	@VIEW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM PLAN_VIEW_FORMAT 
	WHERE VIEW_RID = @VIEW_RID

	DELETE 
	FROM PLAN_VIEW_SPLITTER 
	WHERE VIEW_RID = @VIEW_RID

	DELETE 
	FROM PLAN_VIEW 
	WHERE VIEW_RID = @VIEW_RID

	SELECT @@ROWCOUNT;
END
GO