--dv =============================================
--dv Create date:	11/27/2013
--dv Description:	Reads rows with global user from the grid view for the given layout and list of users
--dv =============================================
CREATE PROCEDURE [dbo].[MID_GRID_VIEW_READ_WITH_GLOBAL_USER] 
	@LAYOUT_ID int,
	@USER_RID_LIST USER_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @GlobalUserRID int = 4;

	SELECT 
		VIEW_RID, 
		gv.USER_RID, 
		LAYOUT_ID,
        CASE gv.USER_RID WHEN @GlobalUserRID THEN VIEW_ID ELSE VIEW_ID + ' (' + au.USER_NAME + ')' END AS VIEW_ID,
	    VIEW_ID AS RAW_VIEW_ID, 
		SHOW_DETAILS, 
		GROUP_BY, 
		GROUP_BY_SECONDARY, 
		IS_SEQUENTIAL,
		WORKSPACE_FILTER_RID
	FROM GRID_VIEW gv
    INNER JOIN APPLICATION_USER au on gv.USER_RID = au.USER_RID
    WHERE LAYOUT_ID = @LAYOUT_ID
          AND gv.USER_RID IN (SELECT USER_RID FROM @USER_RID_LIST) 
	ORDER BY VIEW_ID

END
GO