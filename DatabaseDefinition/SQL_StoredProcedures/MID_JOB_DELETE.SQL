--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes the job
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_DELETE]
	@JOB_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE JOB 
	WHERE JOB_RID = @JOB_RID

	SELECT @@ROWCOUNT;
END
GO