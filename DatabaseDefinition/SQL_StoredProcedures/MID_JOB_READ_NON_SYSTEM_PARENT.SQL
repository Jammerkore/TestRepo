--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads the parent non-system jobs for the child item type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_READ_NON_SYSTEM_PARENT]
	@CHILD_ITEM_TYPE int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		j.*, 
		COALESCE(fjt.PARENT_FOLDER_RID, -1) PARENT_FOLDER_RID 
	FROM JOB j 
		 LEFT OUTER JOIN FOLDER_JOIN fjt ON j.JOB_RID = fjt.CHILD_ITEM_RID
	WHERE SYSTEM_GENERATED_IND = '0' 
		  AND fjt.CHILD_ITEM_TYPE = @CHILD_ITEM_TYPE

END
GO