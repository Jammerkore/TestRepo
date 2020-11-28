--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes allocation tasks from the task list provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_ALLOCATE_DELETE_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM TASK_ALLOCATE 
	WHERE TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;

END
GO