--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes a tasklist job join
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_TASKLIST_JOIN_DELETE]
	@JOB_RID int,
	@TASKLIST_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM JOB_TASKLIST_JOIN 
	WHERE JOB_RID = @JOB_RID
		  AND TASKLIST_RID = @TASKLIST_RID

	SELECT @@ROWCOUNT;
END
GO