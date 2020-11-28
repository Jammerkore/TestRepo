--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Reads folder shortcuts for the given shortcut type and users
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_SHORTCUT_READ_FOLDER_FOR_USERS]
	@USER_RID_LIST USER_RID_TYPE READONLY,
	@CHILD_SHORTCUT_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT fst.* 
	FROM FOLDER_SHORTCUT fst, 
		 FOLDER ft
	WHERE ft.USER_RID IN (SELECT USER_RID FROM @USER_RID_LIST) 
	AND fst.CHILD_SHORTCUT_TYPE = @CHILD_SHORTCUT_TYPE
	AND ft.FOLDER_RID = fst.PARENT_FOLDER_RID

END
GO