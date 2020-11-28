--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Reads rows from the grid view for the given layout and list of users
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GRID_VIEW_READ_FROM_USERS] 
	@LAYOUT_ID int,
	@USER_RID_LIST USER_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM GRID_VIEW gv
    WHERE LAYOUT_ID = @LAYOUT_ID
          AND gv.USER_RID IN (SELECT USER_RID FROM @USER_RID_LIST) 
	ORDER BY VIEW_ID

END
GO