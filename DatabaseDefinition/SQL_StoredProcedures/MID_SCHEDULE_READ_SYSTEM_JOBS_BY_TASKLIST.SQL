--dv =============================================
--dv Create date: 02/07/2014
--dv Description: Get the system jobs by tasklist.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_READ_SYSTEM_JOBS_BY_TASKLIST]
	@TASKLIST_RID INT
AS
BEGIN
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
		AND jt.SYSTEM_GENERATED_IND = '1'
END
GO




