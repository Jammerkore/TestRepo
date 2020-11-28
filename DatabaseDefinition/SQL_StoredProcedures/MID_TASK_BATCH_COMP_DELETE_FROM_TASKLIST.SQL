--dv =============================================
--dv Create date:	5/114/2015
--dv Description:	Deletes Batch Comp tasks from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_BATCH_COMP_DELETE_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM TASK_BATCH_COMP 
	WHERE TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;

END
GO
