--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Deletes enqueue data based on the type, RID, user, owning thread and owning transaction
--dv Usage:			Used only for headers
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_DELETE_FOR_HEADER]
	@ENQUEUE_TYPE int,
	@RID int,
	@USER_RID int,
	@OWNING_THREADID int,
	@OWNING_TRANID bigint
AS
BEGIN
	SET NOCOUNT ON;

	DELETE
	FROM MID_ENQUEUE WITH (ROWLOCK)
	WHERE ENQUEUE_TYPE = @ENQUEUE_TYPE
		  AND RID = @RID
		  AND USER_RID = @USER_RID
		  AND OWNING_THREADID = @OWNING_THREADID
		  AND OWNING_TRANID = @OWNING_TRANID

	SELECT @@ROWCOUNT;

END
GO
