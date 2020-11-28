--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Reads store filters for the users passed in
--dv History: TT#2115-MD - JSmith - Save As Header Filter from Global to User saves with the incorrect User ID appended in the lists
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FILTER_READ_FROM_USERS]
	@FILTER_TYPE INT,
	@FILTER_PROFILE_TYPE INT,
	@USER_RID_LIST USER_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @GlobalUserRID int = 4;
	
	SELECT 
		f.FILTER_RID, 
		ui.USER_RID, 
		CASE f.OWNER_USER_RID WHEN @GlobalUserRID THEN f.FILTER_NAME ELSE f.FILTER_NAME + ' (' + au.USER_NAME + ')' END AS FILTER_NAME,
		ui.OWNER_USER_RID, 
		f.USER_RID AS FILTER_USER_RID
	FROM FILTER f
	INNER JOIN USER_ITEM ui WITH (NOLOCK) ON f.FILTER_RID = ui.ITEM_RID  
	/* Begin TT#2115-MD - JSmith - Save As Header Filter from Global to User saves with the incorrect User ID appended in the lists */
	--INNER JOIN APPLICATION_USER au on f.USER_RID = au.USER_RID
	INNER JOIN APPLICATION_USER au on f.OWNER_USER_RID = au.USER_RID
	/* Begin TT#2115-MD - JSmith - Save As Header Filter from Global to User saves with the incorrect User ID appended in the lists */
	WHERE f.FILTER_TYPE = @FILTER_TYPE
		  AND ui.ITEM_TYPE = @FILTER_PROFILE_TYPE
		  AND ui.USER_RID IN (SELECT USER_RID FROM @USER_RID_LIST)
	ORDER BY f.FILTER_NAME

END
GO

