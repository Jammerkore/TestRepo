--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads taskslist that have forecast balance tasks which contain the node provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_FORECAST_BALANCE_READ_FROM_NODE]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		TASKLIST_NAME, 
		TL.TASKLIST_RID, 
		TL.USER_RID, 
		AU.USER_NAME
	FROM TASKLIST TL, 
		 APPLICATION_USER AU, 
		 TASK_FORECAST_BALANCE TFB
	WHERE TFB.TASKLIST_RID = TL.TASKLIST_RID 
		  AND AU.USER_RID = TL.USER_RID 
		  AND TFB.HN_RID = @HN_RID

END
GO