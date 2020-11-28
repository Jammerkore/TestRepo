--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Reads the grid view for the given id, layout, and user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GRID_VIEW_READ_FROM_ID_AND_USER] 
	@USER_RID int,
	@LAYOUT_ID int,
	@VIEW_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM GRID_VIEW 
	WHERE USER_RID = @USER_RID 
		  AND LAYOUT_ID = @LAYOUT_ID 
		  AND VIEW_ID = @VIEW_ID

END
GO