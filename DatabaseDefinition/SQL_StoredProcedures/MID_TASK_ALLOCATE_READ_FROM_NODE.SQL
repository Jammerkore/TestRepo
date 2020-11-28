--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads taskslist that have allocate tasks which contain the node provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_ALLOCATE_READ_FROM_NODE]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		TASKLIST_NAME, 
		TL.TASKLIST_RID, 
		TL.USER_RID, 
		AU.USER_NAME
	FROM TASKLIST TL, APPLICATION_USER AU, TASK_ALLOCATE TA
	WHERE TA.TASKLIST_RID = TL.TASKLIST_RID 
		  AND AU.USER_RID = TL.USER_RID
		  AND TA.HN_RID = @HN_RID

END
GO