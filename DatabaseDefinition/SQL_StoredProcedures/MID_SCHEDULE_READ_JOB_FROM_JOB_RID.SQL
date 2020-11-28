--dv =============================================
--dv Create date: 02/06/2014
--dv Description: Get a particular job that is also scheduled.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_READ_JOB_FROM_JOB_RID]
	@JOB_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT sjj.*,
		jt.SYSTEM_GENERATED_IND JOB_SYSTEM_GENERATED_IND
	FROM SCHEDULE st,
		SCHEDULE_JOB_JOIN sjj,
		JOB jt
	WHERE st.SCHED_RID = sjj.SCHED_RID
		AND jt.JOB_RID = sjj.JOB_RID
		AND sjj.JOB_RID = @JOB_RID
END
GO


