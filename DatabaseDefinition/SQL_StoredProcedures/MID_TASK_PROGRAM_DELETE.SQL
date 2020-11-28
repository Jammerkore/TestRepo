--dv =============================================
--dv Create date:	2/13/2013
--dv Description:	Deletes program tasks from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_PROGRAM_DELETE]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE
	FROM TASK_PROGRAM 
	WHERE TASKLIST_RID = @TASKLIST_RID 

	SELECT @@ROWCOUNT;
END
GO

