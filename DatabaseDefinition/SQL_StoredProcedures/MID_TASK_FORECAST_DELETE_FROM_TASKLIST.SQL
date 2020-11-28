--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes forecast tasks by task list
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_FORECAST_DELETE_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM TASK_FORECAST 
	WHERE TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;

END
GO