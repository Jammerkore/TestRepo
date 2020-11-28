--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes tasks from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_DELETE_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM TASK 
	WHERE TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;

END
GO