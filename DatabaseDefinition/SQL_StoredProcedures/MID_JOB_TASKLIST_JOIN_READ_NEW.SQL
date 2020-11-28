--dv =============================================
--dv Create date: 02/11/2014
--dv Description: Get a new job tasklist join row.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_JOB_TASKLIST_JOIN_READ_NEW]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM JOB_TASKLIST_JOIN
	WHERE JOB_RID = - 1
END
GO


