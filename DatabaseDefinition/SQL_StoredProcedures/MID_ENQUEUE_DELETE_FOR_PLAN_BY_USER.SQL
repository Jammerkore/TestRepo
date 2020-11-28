--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Deletes enqueue data for a store week/chain week for planning for the user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_DELETE_FOR_PLAN_BY_USER]
	@ENQUEUE_TYPE int,
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM MID_ENQUEUE 
	WHERE ENQUEUE_TYPE = @ENQUEUE_TYPE
		  AND USER_RID = @USER_RID

	SELECT @@ROWCOUNT;

END
GO
