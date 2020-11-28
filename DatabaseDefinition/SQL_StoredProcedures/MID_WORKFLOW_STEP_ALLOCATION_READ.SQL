--dv =============================================
--dv Create date: 12/11/2013
--dv Description:	Get the workflow step allocation based on a workflow RID.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_STEP_ALLOCATION_READ]
	@WORKFLOW_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @IncludeUndefinedMethodRID INT = 1;
	DECLARE @IncludeUseSystemTolerancePercent INT = - 1;
	DECLARE @IncludeUndefinedStoreFilter INT = 1;

	SELECT 
		ws.WORKFLOW_RID,
		ws.STEP_NUMBER,
		ws.ACTION_METHOD_TYPE,
		COALESCE(ws.METHOD_RID, @IncludeUndefinedMethodRID) METHOD_RID,
		COALESCE(ws.PCT_TOLERANCE, @IncludeUseSystemTolerancePercent) PCT_TOLERANCE,
		COALESCE(ws.STORE_FILTER_RID, @IncludeUndefinedStoreFilter) STORE_FILTER_RID,
		COALESCE(ws.REVIEW_IND, '0') REVIEW_IND
	FROM WORKFLOW_STEP_ALLOCATION ws
	WHERE WORKFLOW_RID = @WORKFLOW_RID

END
GO





















