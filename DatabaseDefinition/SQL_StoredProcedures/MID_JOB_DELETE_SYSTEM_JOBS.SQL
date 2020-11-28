--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes system jobs
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_DELETE_SYSTEM_JOBS]
	@JOB_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM JOB 
	WHERE JOB_RID = @JOB_RID 
		  AND SYSTEM_GENERATED_IND = '1'

	SELECT @@ROWCOUNT;
END
GO