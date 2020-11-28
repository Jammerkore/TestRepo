--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads forecast task details from the method provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_FORECAST_DETAIL_READ_FROM_METHOD]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DECLARE @eWorkflowMethodType_Method INT = 801701;

	SELECT 
		TASKLIST_NAME, 
		TL.TASKLIST_RID, 
		TL.USER_RID, 
		AU.USER_NAME 
	FROM TASKLIST TL, 
		 APPLICATION_USER AU, 
		 TASK_FORECAST_DETAIL TFD 
	WHERE TFD.TASKLIST_RID = TL.TASKLIST_RID 
		  AND AU.USER_RID = TL.USER_RID 
		  AND TFD.WORKFLOW_METHOD_IND = @eWorkflowMethodType_Method 
		  AND TFD.METHOD_RID = @METHOD_RID

END
GO