--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Deletes all folder shortcuts for the given shortcut rid and type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_SHORTCUT_DELETE_ALL]
	@CHILD_SHORTCUT_RID int,
	@CHILD_SHORTCUT_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM FOLDER_SHORTCUT 
	WHERE CHILD_SHORTCUT_RID = @CHILD_SHORTCUT_RID
		  AND CHILD_SHORTCUT_TYPE = @CHILD_SHORTCUT_TYPE
	
	SELECT @@ROWCOUNT;

END
GO