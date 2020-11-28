--dv =============================================
--dv Create date:	11/25/2013
--dv Description:	Reads folder shortcuts for specific folder types
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FOLDER_SHORTCUT_READ_FOR_TYPES]
	@FOLDER_TYPE_LIST FOLDER_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		fst.* 
	FROM FOLDER_SHORTCUT fst, 
		 FOLDER fpt, 
		 FOLDER fct
	WHERE fct.FOLDER_TYPE IN (SELECT FOLDER_TYPE FROM @FOLDER_TYPE_LIST)
		  AND fst.CHILD_SHORTCUT_TYPE = fct.FOLDER_TYPE
		  AND fpt.FOLDER_RID = fst.PARENT_FOLDER_RID
		  AND fct.FOLDER_RID = fst.CHILD_SHORTCUT_RID
	ORDER BY PARENT_FOLDER_RID


END
GO