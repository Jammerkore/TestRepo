--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads the job for the user provided
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_READ_FROM_USER]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		st.SCHED_NAME, 
		jt.JOB_NAME, 
		sjj.*
	FROM SCHEDULE st, 
		 SCHEDULE_JOB_JOIN sjj, 
		 JOB jt
	WHERE st.SCHED_RID = sjj.SCHED_RID 
		  AND sjj.USER_RID = @USER_RID 
	ORDER BY st.SCHED_RID

END
GO