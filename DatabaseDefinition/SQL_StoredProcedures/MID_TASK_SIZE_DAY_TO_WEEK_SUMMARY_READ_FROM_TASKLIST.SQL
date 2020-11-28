--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads tasks for size day to week summary from the tasklist provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_READ_FROM_TASKLIST]
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		TASKLIST_RID, 
		TASK_SEQUENCE, 
		coalesce(CDR_RID, 1) CDR_RID, 
		coalesce(HN_RID, -1) HN_RID 
	FROM TASK_SIZE_DAY_TO_WEEK_SUMMARY 
	WHERE TASKLIST_RID = @TASKLIST_RID

END
GO