--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads a tasklist by job
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_TASKLIST_JOIN_READ]
	@JOB_RID INT
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT 
		jtj.JOB_RID, 
		jtj.TASKLIST_RID, 
		jtj.TASKLIST_SEQUENCE, 
		tl.TASKLIST_NAME, 
		tl.USER_RID
	FROM JOB_TASKLIST_JOIN jtj, 
		 TASKLIST tl 
	WHERE jtj.JOB_RID = @JOB_RID
		  AND jtj.TASKLIST_RID = tl.TASKLIST_RID 
	ORDER BY jtj.TASKLIST_SEQUENCE

END
GO