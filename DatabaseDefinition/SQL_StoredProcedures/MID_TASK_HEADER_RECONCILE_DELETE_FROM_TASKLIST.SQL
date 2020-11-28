--dv =============================================
--dv Create date:	5/29/2015
--dv Description:	Deletes header reconcile tasks from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_HEADER_RECONCILE_DELETE_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM TASK_HEADER_RECONCILE
	WHERE TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;

END
GO
