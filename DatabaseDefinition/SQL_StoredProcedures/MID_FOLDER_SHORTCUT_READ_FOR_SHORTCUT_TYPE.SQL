--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Reads folder shortcuts for the given shortcut type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_SHORTCUT_READ_FOR_SHORTCUT_TYPE]
	@CHILD_SHORTCUT_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT fst.* 
	FROM FOLDER_SHORTCUT fst, 
		 FOLDER ft
	WHERE fst.CHILD_SHORTCUT_TYPE = @CHILD_SHORTCUT_TYPE
		  AND ft.FOLDER_RID = fst.PARENT_FOLDER_RID

END
GO