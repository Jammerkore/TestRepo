--dv =============================================
--dv Create date:	2/11/2014
--dv Description:	Deletes a schedule
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SCHEDULE_DELETE]
	@SCHED_RID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE 
	FROM SCHEDULE 
	WHERE SCHED_RID = @SCHED_RID

	SELECT @@ROWCOUNT;
END
GO