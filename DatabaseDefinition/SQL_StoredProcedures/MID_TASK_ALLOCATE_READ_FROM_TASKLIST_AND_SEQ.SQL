--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads allocation tasks from the task list and sequence provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_ALLOCATE_READ_FROM_TASKLIST_AND_SEQ]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM TASK_ALLOCATE 
	WHERE TASKLIST_RID = @TASKLIST_RID
		  AND TASK_SEQUENCE = @TASK_SEQUENCE 
	ORDER BY ALLOCATE_SEQUENCE

END
GO