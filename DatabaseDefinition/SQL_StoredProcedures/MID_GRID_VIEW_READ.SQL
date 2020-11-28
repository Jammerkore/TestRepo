--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Reads the grid view for the given view rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GRID_VIEW_READ] 
	@VIEW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM GRID_VIEW 
	WHERE VIEW_RID = @VIEW_RID

END
GO