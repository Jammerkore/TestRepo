--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Deletes all enqueue data for the user and client session thread id
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_DELETE_FOR_USER_AND_THREAD]
	@USER_RID int,
	@OWNING_THREADID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM MID_ENQUEUE WITH (ROWLOCK)
	WHERE USER_RID = @USER_RID
		  AND OWNING_THREADID = @OWNING_THREADID

	SELECT @@ROWCOUNT;

END
GO
