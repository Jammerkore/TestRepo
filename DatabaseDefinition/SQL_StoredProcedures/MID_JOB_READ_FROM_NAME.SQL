--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Reads the job from the job name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_READ_FROM_NAME]
	@JOB_NAME varchar(100)
AS
BEGIN
	SET NOCOUNT ON;
	
	SELECT * 
	FROM JOB 
	WHERE JOB_NAME = @JOB_NAME

END
GO