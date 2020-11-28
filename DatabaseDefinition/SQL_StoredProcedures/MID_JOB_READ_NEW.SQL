--dv =============================================
--dv Create date: 02/11/2014
--dv Description: Get new job row.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_READ_NEW]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT * 
	FROM JOB 
	WHERE JOB_RID = -1

END
GO
