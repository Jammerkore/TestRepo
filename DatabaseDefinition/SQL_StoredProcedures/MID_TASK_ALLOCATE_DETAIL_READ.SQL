--dv =============================================
--dv Create date:	2/13/2013
--dv Description:	Reads allocation task details from the tasklist and sequences provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_ALLOCATE_DETAIL_READ]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int,
	@ALLOCATE_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM TASK_ALLOCATE_DETAIL 
	WHERE TASKLIST_RID = @TASKLIST_RID
		  AND TASK_SEQUENCE = @TASK_SEQUENCE
		  AND ALLOCATE_SEQUENCE = @ALLOCATE_SEQUENCE
	ORDER BY DETAIL_SEQUENCE

END
GO

