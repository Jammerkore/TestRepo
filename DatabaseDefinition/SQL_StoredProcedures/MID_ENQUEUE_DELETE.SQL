--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Deletes enqueue data based on the type, RID, and user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_DELETE]
	@ENQUEUE_TYPE int,
	@RID int,
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM MID_ENQUEUE WITH (ROWLOCK)
	WHERE ENQUEUE_TYPE = @ENQUEUE_TYPE
		  AND RID = @RID
		  AND USER_RID = @USER_RID
	
	SELECT @@ROWCOUNT;
END
GO
