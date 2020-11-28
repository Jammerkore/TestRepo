--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Inserts a task for size day to week summary
--dv =============================================
CREATE PROCEDURE [dbo].[MID_TASK_SIZE_DAY_TO_WEEK_SUMMARY_INSERT]
	@TASKLIST_RID int,
	@TASK_SEQUENCE int, 
	@CDR_RID int, 
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO TASK_SIZE_DAY_TO_WEEK_SUMMARY
		(
		TASKLIST_RID, 
		TASK_SEQUENCE, 
		CDR_RID, 
		HN_RID
		)
	VALUES
		(
		@TASKLIST_RID, 
		@TASK_SEQUENCE, 
		@CDR_RID, 
		@HN_RID
		)

	SELECT @@ROWCOUNT;

END
GO