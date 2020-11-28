--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Reads the grid view details for the given view
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GRID_VIEW_DETAIL_READ] 
	@VIEW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM GRID_VIEW_DETAIL 
	WHERE VIEW_RID = @VIEW_RID 
	ORDER BY VIEW_RID, BAND_KEY, COLUMN_KEY

END
GO