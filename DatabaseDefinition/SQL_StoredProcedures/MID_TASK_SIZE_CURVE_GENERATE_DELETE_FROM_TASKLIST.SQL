--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes a size curve generate task from the task list provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_SIZE_CURVE_GENERATE_DELETE_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eSizeCurveGenerateType_Method INT = 1;

	DELETE 
	FROM TASK_SIZE_CURVE_GENERATE 
	WHERE METHOD_NODE_TYPE = @eSizeCurveGenerateType_Method 
	AND TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;
END
GO