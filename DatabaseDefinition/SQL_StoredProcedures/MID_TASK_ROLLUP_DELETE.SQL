--dv =============================================
--dv Create date:	2/13/2013
--dv Description:	Deletes rollup tasks from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_ROLLUP_DELETE]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM TASK_ROLLUP 
	WHERE TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;
END
GO
