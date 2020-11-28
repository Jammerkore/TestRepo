--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads taskslists that have forecast detail tasks which contain the workflow provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_FORECAST_DETAIL_READ_FROM_WORKFLOW]
	@WORKFLOW_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eWorkflowMethodType_Workflow INT = 801702;

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
		  AND TFD.WORKFLOW_METHOD_IND = @eWorkflowMethodType_Workflow 
		  AND TFD.WORKFLOW_RID = @WORKFLOW_RID

END
GO