--dv =============================================
--dv Create date:	11/17/2020
--dv Description:	Deletes a plan view format
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PLAN_VIEW_FORMAT_DELETE] 
	@VIEW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM PLAN_VIEW_FORMAT 
	WHERE VIEW_RID = @VIEW_RID

	SELECT @@ROWCOUNT;

END
GO