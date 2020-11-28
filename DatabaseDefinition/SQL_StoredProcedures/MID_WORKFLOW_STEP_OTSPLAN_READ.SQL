--dv =============================================
--dv Create date:	11/13/2013
--dv Description:	Reads workflow steps based on workflow rid.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_WORKFLOW_STEP_OTSPLAN_READ]
       @WORKFLOW_RID int
AS
BEGIN
    SET NOCOUNT ON;

	DECLARE @UndefinedMethodRID int = 1;
	DECLARE @UseSystemTolerancePercent int = -1;
	DECLARE @UndefinedStoreFilter int = 1;

	SELECT 
		WORKFLOW_RID,
		STEP_NUMBER,
		ACTION_METHOD_TYPE,
		COALESCE(METHOD_RID, @UndefinedMethodRID) METHOD_RID,
		COALESCE(PCT_TOLERANCE, @UseSystemTolerancePercent) PCT_TOLERANCE,
		COALESCE(STORE_FILTER_RID, @UndefinedStoreFilter) STORE_FILTER_RID, 
		COALESCE(REVIEW_IND,'0') REVIEW_IND, 
		COALESCE(VARIABLE_NUMBER,-1) VARIABLE_NUMBER, 
		CALC_MODE, 
		COALESCE(BAL_IND,'0') BAL_IND 
	FROM WORKFLOW_STEP_OTSPLAN 
	WHERE WORKFLOW_RID = @WORKFLOW_RID

END
GO
