--dv =============================================
--dv Create date:	3/26/2021
--dv Description:	Deletes splitters for a grid view
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GRID_VIEW_SPLITTER_DELETE] 
	@VIEW_RID int,
	@USER_RID int,
	@LAYOUT_ID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM GRID_VIEW_SPLITTER 
	WHERE VIEW_RID = @VIEW_RID
	  AND USER_RID = @USER_RID
	  AND LAYOUT_ID = @LAYOUT_ID

	SELECT @@ROWCOUNT;

END
GO