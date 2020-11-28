--dv =============================================
--dv Create date: 02/07/2014
--dv Description: Get schedule based on key.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_READ_KEY]
	@SCHED_NAME varchar(100)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM SCHEDULE
	WHERE SCHED_NAME = @SCHED_NAME

END
GO