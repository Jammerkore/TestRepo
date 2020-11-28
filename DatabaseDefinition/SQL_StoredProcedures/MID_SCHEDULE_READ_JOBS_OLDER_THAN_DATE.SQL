--dv =============================================
--dv Create date: 02/06/2014
--dv Description: Get all jobs that ran that are older than a certain date.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_READ_JOBS_OLDER_THAN_DATE]
	@LAST_COMPLETION_DATETIME DATETIME
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eProcessExecutionStatus_Cancelled INT = 801605;
	DECLARE @eProcessExecutionStatus_Completed INT = 801604;

	SELECT sjj.*,
		jtj.*,
		jt.SYSTEM_GENERATED_IND JOB_SYSTEM_GENERATED_IND,
		tt.SYSTEM_GENERATED_IND TASKLIST_SYSTEM_GENERATED_IND
	FROM SCHEDULE st,
		SCHEDULE_JOB_JOIN sjj,
		JOB jt,
		JOB_TASKLIST_JOIN jtj,
		TASKLIST tt
	WHERE st.SCHED_RID = sjj.SCHED_RID
		AND jt.JOB_RID = sjj.JOB_RID
		AND jt.JOB_RID = jtj.JOB_RID
		AND tt.TASKLIST_RID = jtj.TASKLIST_RID
		AND sjj.LAST_COMPLETION_DATETIME < @LAST_COMPLETION_DATETIME
		AND (
			sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Cancelled
			OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Completed
			)
END
GO



