--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads the count of user security hierarchy nodes
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_HIERARCHY_NODE_READ_COUNT]
	@USER_RID int,
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM SECURITY_USER_HIERARCHY_NODE
	WHERE USER_RID = @USER_RID
		  AND HN_RID = @HN_RID

END
GO