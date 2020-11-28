--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads forecast task details from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_FORECAST_DETAIL_READ_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM TASK_FORECAST_DETAIL 
	WHERE TASKLIST_RID = @TASKLIST_RID

END
GO