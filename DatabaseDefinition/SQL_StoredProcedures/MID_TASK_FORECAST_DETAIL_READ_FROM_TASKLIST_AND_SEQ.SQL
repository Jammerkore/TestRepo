--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads forecast task details from the tasklist and sequences provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST_AND_SEQ]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int,
	@FORECAST_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM TASK_FORECAST_DETAIL 
	WHERE TASKLIST_RID = @TASKLIST_RID
		  AND TASK_SEQUENCE = @TASK_SEQUENCE 
		  AND FORECAST_SEQUENCE = @FORECAST_SEQUENCE
	ORDER BY DETAIL_SEQUENCE

END
GO