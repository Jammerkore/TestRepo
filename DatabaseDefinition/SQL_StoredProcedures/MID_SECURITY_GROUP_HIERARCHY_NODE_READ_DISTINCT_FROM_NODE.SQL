--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads distinct security groups from a node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_NODE]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
		ug.GROUP_RID, 
		ug.GROUP_NAME, 
		ug.GROUP_DESCRIPTION
	FROM SECURITY_GROUP_HIERARCHY_NODE sghn, 
		 USER_GROUP ug
	WHERE sghn.HN_RID = @HN_RID
		  AND sghn.GROUP_RID = ug.GROUP_RID

END
GO