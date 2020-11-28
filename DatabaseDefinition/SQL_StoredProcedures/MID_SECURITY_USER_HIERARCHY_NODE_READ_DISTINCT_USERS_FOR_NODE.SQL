--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads distinct users for the hierarchy node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_USERS_FOR_NODE]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		au.USER_RID, 
		au.USER_NAME
	FROM SECURITY_USER_HIERARCHY_NODE suhn, 
		 APPLICATION_USER au
	WHERE suhn.HN_RID = @HN_RID
		  AND suhn.USER_RID = au.USER_RID

END
GO

