--dv =============================================
--dv Create date:	2/13/2013
--dv Description:	Reads rollup tasks from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_ROLLUP_READ_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM TASK_ROLLUP 
	WHERE TASKLIST_RID = @TASKLIST_RID

END
GO
