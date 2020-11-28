--dv =============================================
--dv Create date: 02/11/2014
--dv Description:	Get new schedule job join row.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_JOB_JOIN_READ_NEW]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM SCHEDULE_JOB_JOIN
	WHERE SCHED_RID = - 1

END
GO


