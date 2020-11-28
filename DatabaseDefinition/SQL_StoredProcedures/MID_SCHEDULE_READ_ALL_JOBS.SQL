--dv =============================================
--dv Create date: 02/06/2014
--dv Description: Get all scheduled jobs.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_READ_ALL_JOBS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT st.SCHED_NAME,
		jt.JOB_NAME,
		sjj.*
	FROM SCHEDULE st,
		SCHEDULE_JOB_JOIN sjj,
		JOB jt
	WHERE st.SCHED_RID = sjj.SCHED_RID
		AND jt.JOB_RID = sjj.JOB_RID
	ORDER BY st.SCHED_RID

END
GO


