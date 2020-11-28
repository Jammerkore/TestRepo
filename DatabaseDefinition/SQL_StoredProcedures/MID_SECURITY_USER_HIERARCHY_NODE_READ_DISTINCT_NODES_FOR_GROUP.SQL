--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Reads distinct hierarchy base nodes for the user group obtained from the user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_HIERARCHY_NODE_READ_DISTINCT_NODES_FOR_GROUP]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		hn.HN_RID HN_RID, 
		BN_ID, 
		HOME_PH_RID, 
		HOME_LEVEL
	FROM SECURITY_GROUP_HIERARCHY_NODE sghn, 
		 USER_GROUP_JOIN ugj, 
		 USER_GROUP ug, 
		 HIERARCHY_NODE hn, 
		 BASE_NODE bn
	WHERE sghn.GROUP_RID = ugj.GROUP_RID 
		  AND ug.GROUP_RID = ugj.GROUP_RID 
		  AND ug.GROUP_ACTIVE_IND = '1'
		  AND USER_RID = @USER_RID
		  AND sghn.HN_RID = hn.HN_RID 
		  AND bn.HN_RID = hn.HN_RID
	ORDER BY HOME_PH_RID, HOME_LEVEL DESC

END
GO

