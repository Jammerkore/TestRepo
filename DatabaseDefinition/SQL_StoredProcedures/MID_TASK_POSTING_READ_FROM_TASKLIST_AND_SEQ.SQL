--dv =============================================
--dv Create date:	2/13/2013
--dv Description:	Reads posting tasks from the tasklist and sequence provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_POSTING_READ_FROM_TASKLIST_AND_SEQ]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM TASK_POSTING 
	WHERE TASKLIST_RID = @TASKLIST_RID 
	AND TASK_SEQUENCE = @TASK_SEQUENCE

END
GO
