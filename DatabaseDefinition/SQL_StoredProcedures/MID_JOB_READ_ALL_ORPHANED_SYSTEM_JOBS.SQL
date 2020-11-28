--dv =============================================
--dv Create date: 02/06/2014
--dv Description: Get all orphaned system jobs.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_READ_ALL_ORPHANED_SYSTEM_JOBS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT jtj.*,
		jt.SYSTEM_GENERATED_IND JOB_SYSTEM_GENERATED_IND
	FROM JOB jt,
		JOB_TASKLIST_JOIN jtj,
		TASKLIST tt
	WHERE jt.JOB_RID = jtj.JOB_RID
		AND tt.TASKLIST_RID = jtj.TASKLIST_RID
		AND jt.JOB_RID NOT IN (
			SELECT JOB_RID
			FROM SCHEDULE_JOB_JOIN
			)
		AND jt.SYSTEM_GENERATED_IND = '1'
END
GO
