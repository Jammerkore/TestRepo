--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads a tasklist by job
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_TASKLIST_JOIN_READ_FROM_JOB]
	@JOB_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM JOB_TASKLIST_JOIN 
	WHERE JOB_RID = @JOB_RID

END
GO