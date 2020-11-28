--dv =============================================
--dv Create date: 02/06/2014
--dv Description: Get a particular scheduled job.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_READ_JOB]
	@SCHED_RID INT,
	@JOB_RID INT
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
		AND sjj.SCHED_RID = @SCHED_RID
		AND sjj.JOB_RID = @JOB_RID
	ORDER BY st.SCHED_RID

END
GO

