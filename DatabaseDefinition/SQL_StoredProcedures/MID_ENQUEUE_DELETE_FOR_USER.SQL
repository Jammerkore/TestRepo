--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Deletes all enqueue data for the user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_DELETE_FOR_USER]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM MID_ENQUEUE WITH (ROWLOCK)
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;

END
GO
