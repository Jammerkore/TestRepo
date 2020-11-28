--dv =============================================
--dv Create date:	10/15/2013
--dv Description:	Reads enqueue data for the hierarchy nodes based on the node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ENQUEUE_READ_FOR_HIERARCHY_NODES]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @HIERARCHY_NODE_TYPE int = 6;

	SELECT 
		me.USER_RID, 
		me.HN_RID,
		CASE au.USER_FULLNAME WHEN '' THEN au.USER_NAME ELSE au.USER_FULLNAME END as USER_FULLNAME
	FROM MID_ENQUEUE me, APPLICATION_USER au
	WHERE ENQUEUE_TYPE = @HIERARCHY_NODE_TYPE
		  AND HN_RID = @HN_RID
		  AND me.USER_RID=au.USER_RID

END
GO
