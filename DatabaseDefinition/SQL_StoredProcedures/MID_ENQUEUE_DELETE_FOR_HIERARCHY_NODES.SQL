--dv =============================================
--dv Create date:	10/16/2013
--dv Description:	Deletes enqueue data for the hierarchy nodes based on the user and node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_DELETE_FOR_HIERARCHY_NODES]
	@HN_RID int,
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HIERARCHY_NODE_TYPE int
	SET @HIERARCHY_NODE_TYPE = 6

	DELETE 
	FROM MID_ENQUEUE 
	WHERE ENQUEUE_TYPE = @HIERARCHY_NODE_TYPE
          AND HN_RID = @HN_RID
          AND USER_RID = @USER_RID

	SELECT @@ROWCOUNT;

END
GO
