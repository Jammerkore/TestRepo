--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads a job
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_READ]
	@JOB_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM JOB 
	WHERE JOB_RID = @JOB_RID

END
GO