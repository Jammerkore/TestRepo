--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Reads folders for users where folders are assigned and owned
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_READ_ASSIGNED_AND_OWNED_FOR_USERS]
	@FOLDER_TYPE int,
	@ITEM_TYPE int,
	@USER_RID_LIST USER_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		f.FOLDER_RID, 
		ui.USER_RID, 
		f.FOLDER_ID, 
		f.FOLDER_TYPE,
		ui.ITEM_TYPE, 
		ui.OWNER_USER_RID
	FROM FOLDER f
    INNER JOIN USER_ITEM ui ON ui.ITEM_RID = f.FOLDER_RID AND ui.ITEM_TYPE = f.FOLDER_TYPE
	WHERE FOLDER_TYPE = @FOLDER_TYPE
		  AND ui.ITEM_TYPE = @ITEM_TYPE
		  AND ui.USER_RID IN (SELECT USER_RID FROM @USER_RID_LIST)
	ORDER BY FOLDER_ID

END
GO