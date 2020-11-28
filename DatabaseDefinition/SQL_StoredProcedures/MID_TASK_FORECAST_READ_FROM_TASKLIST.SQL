--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads forecast tasks by task list
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_FORECAST_READ_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM TASK_FORECAST 
	WHERE TASKLIST_RID = @TASKLIST_RID

END
GO