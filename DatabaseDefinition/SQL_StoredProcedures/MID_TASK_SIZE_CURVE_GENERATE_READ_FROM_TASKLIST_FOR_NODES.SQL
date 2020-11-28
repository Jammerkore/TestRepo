--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads size curve generate tasks from the task list provided for nodes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_SIZE_CURVE_GENERATE_READ_FROM_TASKLIST_FOR_NODES]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eSizeCurveGenerateType_Node INT = 2;

	SELECT 
		TASK_SEQUENCE, 
		TASKLIST_RID, 
		GENERATE_SEQUENCE, 
		HN_RID, 
		METHOD_RID, 
		EXECUTE_CDR_RID 
	FROM TASK_SIZE_CURVE_GENERATE 
	WHERE METHOD_NODE_TYPE = @eSizeCurveGenerateType_Node 
	AND TASKLIST_RID = @TASKLIST_RID

END
GO