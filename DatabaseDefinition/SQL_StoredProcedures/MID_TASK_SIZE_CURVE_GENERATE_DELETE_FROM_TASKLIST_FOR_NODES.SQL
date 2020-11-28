--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes size curve generate tasks from the task list provided for nodes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST_FOR_NODES]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eSizeCurveGenerateType_Node INT = 2;

	DELETE
	FROM TASK_SIZE_CURVE_GENERATE 
	WHERE METHOD_NODE_TYPE = @eSizeCurveGenerateType_Node 
	AND TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;
END
GO