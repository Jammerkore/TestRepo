--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads special request job
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SPECIAL_REQUEST_JOB_JOIN_READ]
	@SPECIAL_REQ_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		jn.SPECIAL_REQ_RID, 
		jn.JOB_RID, 
		jn.JOB_SEQUENCE, 
		job.JOB_NAME
	FROM SPECIAL_REQUEST_JOB_JOIN jn, 
		 JOB job 
	WHERE jn.SPECIAL_REQ_RID = @SPECIAL_REQ_RID
		  AND jn.JOB_RID = job.JOB_RID 
	ORDER BY jn.JOB_SEQUENCE

END
GO