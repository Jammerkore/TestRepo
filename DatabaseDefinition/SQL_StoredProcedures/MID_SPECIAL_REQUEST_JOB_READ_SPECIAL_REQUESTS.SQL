--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads special request jobs
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_JOB_READ_SPECIAL_REQUESTS]
	@CHILD_ITEM_TYPE int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		srj.*, 
		COALESCE(fjt.PARENT_FOLDER_RID, -1) PARENT_FOLDER_RID
	FROM SPECIAL_REQUEST_JOB srj 
		 LEFT OUTER JOIN FOLDER_JOIN fjt ON srj.SPECIAL_REQ_RID = fjt.CHILD_ITEM_RID
    WHERE fjt.CHILD_ITEM_TYPE = @CHILD_ITEM_TYPE

END
GO