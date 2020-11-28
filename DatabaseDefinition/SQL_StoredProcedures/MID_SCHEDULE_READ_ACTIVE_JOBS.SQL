--dv =============================================
--dv Create date: 02/06/2014
--dv Description: Get all active scheduled jobs.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_READ_ACTIVE_JOBS]
	@JOB_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eProcessExecutionStatus_Running INT = 801602;
	DECLARE @eProcessExecutionStatus_Queued INT = 801610;
	DECLARE @eProcessExecutionStatus_Waiting INT = 801601;
	DECLARE @eProcessExecutionStatus_Executed INT = 801606;


	SELECT sjj.*,
		jt.SYSTEM_GENERATED_IND JOB_SYSTEM_GENERATED_IND
	FROM SCHEDULE st,
		SCHEDULE_JOB_JOIN sjj,
		JOB jt
	WHERE st.SCHED_RID = sjj.SCHED_RID
		AND jt.JOB_RID = sjj.JOB_RID
		AND sjj.JOB_RID = @JOB_RID
		AND (
			sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Running
			OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Queued
			OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Waiting
			OR sjj.EXECUTION_STATUS = @eProcessExecutionStatus_Executed
			)
END
GO

