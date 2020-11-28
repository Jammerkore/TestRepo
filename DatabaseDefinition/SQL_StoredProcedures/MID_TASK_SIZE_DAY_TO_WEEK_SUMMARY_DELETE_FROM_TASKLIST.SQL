--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes tasks for size day to week summary from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_DELETE_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM TASK_SIZE_DAY_TO_WEEK_SUMMARY 
	WHERE TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;

END
GO