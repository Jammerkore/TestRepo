--dv =============================================
--dv Create date: 02/11/2014
--dv Description: Get a new schedule row.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_READ_NEW]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM SCHEDULE
	WHERE SCHED_RID = - 1
END
GO

