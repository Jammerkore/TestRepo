--dv =============================================
--dv Create date:	2/13/2013
--dv Description:	Deletes allocation task details from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_ALLOCATE_DETAIL_DELETE_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM TASK_ALLOCATE_DETAIL 
	WHERE TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;

END
GO
