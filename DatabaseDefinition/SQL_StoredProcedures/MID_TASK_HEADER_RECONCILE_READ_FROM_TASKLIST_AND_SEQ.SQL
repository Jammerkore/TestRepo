--dv =============================================
--dv Create date:	5/29/2015
--dv Description:	Reads header reconcile tasks from the tasklist and sequence provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_HEADER_RECONCILE_READ_FROM_TASKLIST_AND_SEQ]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM TASK_HEADER_RECONCILE
	WHERE TASKLIST_RID = @TASKLIST_RID 
	AND TASK_SEQUENCE = @TASK_SEQUENCE

END
GO
