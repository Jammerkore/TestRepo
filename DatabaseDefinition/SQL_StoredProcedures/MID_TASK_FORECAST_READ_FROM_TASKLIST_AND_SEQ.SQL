--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads forecast tasks by task list and sequence
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_FORECAST_READ_FROM_TASKLIST_AND_SEQ]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM TASK_FORECAST 
	WHERE TASKLIST_RID = @TASKLIST_RID
		  AND TASK_SEQUENCE = @TASK_SEQUENCE
	ORDER BY FORECAST_SEQUENCE

END
GO