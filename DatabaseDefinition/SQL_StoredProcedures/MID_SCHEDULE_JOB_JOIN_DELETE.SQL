--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes a schedule job join
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_JOB_JOIN_DELETE]
	@SCHED_RID int, 
	@JOB_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM SCHEDULE_JOB_JOIN 
	WHERE SCHED_RID = @SCHED_RID
		  AND JOB_RID = @JOB_RID

	SELECT @@ROWCOUNT;

END
GO