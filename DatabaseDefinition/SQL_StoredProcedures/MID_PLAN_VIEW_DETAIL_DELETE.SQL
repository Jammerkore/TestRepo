--dv =============================================
--dv Create date:	2/13/2014
--dv Description:	Deletes a plan view detail
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PLAN_VIEW_DETAIL_DELETE] 
	@VIEW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM PLAN_VIEW_DETAIL 
	WHERE VIEW_RID = @VIEW_RID

	SELECT @@ROWCOUNT;

END
GO