--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads a count of security hierarchy nodes for the group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_HIERARCHY_NODE_READ_COUNT]
	@GROUP_RID int,
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM SECURITY_GROUP_HIERARCHY_NODE
	WHERE GROUP_RID = @GROUP_RID
		  AND HN_RID = @HN_RID

END
GO


