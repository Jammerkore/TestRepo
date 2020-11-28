--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Reads a folder rid for the given folder id, type, user and parent folder
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_READ_KEY]
	@FOLDER_TYPE int,
	@USER_RID int,
	@FOLDER_ID varchar(100),
	@PARENT_FOLDER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ft.FOLDER_RID 
	FROM FOLDER_JOIN fjt, 
		 FOLDER ft
	WHERE ft.FOLDER_TYPE = @FOLDER_TYPE
		  AND ft.USER_RID = @USER_RID
		  AND ft.FOLDER_ID = @FOLDER_ID
		  AND ft.FOLDER_RID = fjt.CHILD_ITEM_RID
		  AND fjt.PARENT_FOLDER_RID = @PARENT_FOLDER_RID
	ORDER BY FOLDER_RID

END
GO