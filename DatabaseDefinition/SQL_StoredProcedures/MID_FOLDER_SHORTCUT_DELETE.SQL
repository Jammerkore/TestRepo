--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Deletes a folder shortcut
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_SHORTCUT_DELETE]
	@PARENT_FOLDER_RID int,
	@CHILD_SHORTCUT_RID int,
	@CHILD_SHORTCUT_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM FOLDER_SHORTCUT 
	WHERE PARENT_FOLDER_RID = @PARENT_FOLDER_RID
		  AND CHILD_SHORTCUT_RID = @CHILD_SHORTCUT_RID
		  AND CHILD_SHORTCUT_TYPE = @CHILD_SHORTCUT_TYPE
	
	SELECT @@ROWCOUNT;

END
GO