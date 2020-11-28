--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Deletes a grid view detail
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GRID_VIEW_DETAIL_DELETE] 
	@VIEW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM GRID_VIEW_DETAIL 
	WHERE VIEW_RID = @VIEW_RID

	SELECT @@ROWCOUNT;

END
GO