--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Deletes all enqueue data for a Process Id (Unique from SCHEDULE_PROCESS table)
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_DELETE_FOR_PROCESS_ID]
	@OWNING_THREADID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM MID_ENQUEUE WITH (ROWLOCK)
	WHERE OWNING_THREADID = @OWNING_THREADID

	SELECT @@ROWCOUNT;

END
GO
