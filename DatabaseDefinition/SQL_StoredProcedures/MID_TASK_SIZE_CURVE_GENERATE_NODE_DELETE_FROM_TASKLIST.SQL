--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes size curve generate task nodes from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_SIZE_CURVE_GENERATE_NODE_DELETE_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM TASK_SIZE_CURVE_GENERATE_NODE 
	WHERE TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;
END
GO