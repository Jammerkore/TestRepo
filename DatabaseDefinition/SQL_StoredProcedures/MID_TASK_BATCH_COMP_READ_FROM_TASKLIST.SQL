--dv =============================================
--dv Create date:	5/14/2015
--dv Description:	Reads Batch Comp tasks from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_BATCH_COMP_READ_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM TASK_BATCH_COMP 
	WHERE TASKLIST_RID = @TASKLIST_RID

END
GO
