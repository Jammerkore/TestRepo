--dv =============================================
--dv Create date: 02/06/2014
--dv Description: Get the active jobs by tasklist.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_READ_ACTIVE_JOBS_BY_TASKLIST]
	@TASKLIST_RID INT
AS
BEGIN
	DECLARE @eProcessExecutionStatus_Running INT = 801602;
	DECLARE @eProcessExecutionStatus_Queued INT = 801610;
	DECLARE @eProcessExecutionStatus_Waiting INT = 801601;
	DECLARE @eProcessExecutionStatus_Executed INT = 801606;

	SET NOCOUNT ON;

	SELECT sjj.*,
		jtj.*,
		jt.SYSTEM_GENERATED_IND JOB_SYSTEM_GENERATED_IND
	FROM SCHEDULE st,
		SCHEDULE_JOB_JOIN sjj,
		JOB jt,
		JOB_TASKLIST_JOIN jtj,
		TASKLIST tt
	WHERE st.SCHED_RID = sjj.SCHED_RID
		AND jt.JOB_RID = sjj.JOB_RID
		AND jt.JOB_RID = jtj.JOB_RID
		AND tt.TASKLIST_RID = jtj.TASKLIST_RID
		AND tt.TASKLIST_RID = @TASKLIST_RID
		AND (
			sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Running
			OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Queued
			OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Waiting
			OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Executed
			)
END
GO


