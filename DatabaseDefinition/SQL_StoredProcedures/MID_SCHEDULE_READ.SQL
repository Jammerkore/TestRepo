--dv =============================================
--dv Create date: 02/07/2014
--dv Description: Get a particular schedule.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_READ]
	@SCHED_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM SCHEDULE
	WHERE SCHED_RID = @SCHED_RID

END
GO