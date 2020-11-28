--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Reads folder shortcuts
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_SHORTCUT_READ]
	@CHILD_SHORTCUT_TYPE int,
	@CHILD_SHORTCUT_RID int,
	@PARENT_FOLDER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM FOLDER_SHORTCUT
	WHERE CHILD_SHORTCUT_TYPE = @CHILD_SHORTCUT_TYPE
		  AND CHILD_SHORTCUT_RID = @CHILD_SHORTCUT_RID
		  AND PARENT_FOLDER_RID = @PARENT_FOLDER_RID

END
GO