--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads distinct assignment nodes for the group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_GROUP_HIERARCHY_NODE_READ_DISTINCT_FROM_GROUP]
	@GROUP_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		hn.HN_RID HN_RID, 
		BN_ID, 
		HOME_PH_RID, 
		HOME_LEVEL
    FROM SECURITY_GROUP_HIERARCHY_NODE SGHN, 
		 HIERARCHY_NODE hn, 
		 BASE_NODE bn
	WHERE SGHN.HN_RID = hn.HN_RID 
		  AND bn.HN_RID = hn.HN_RID 
		  AND GROUP_RID = @GROUP_RID
    ORDER BY HOME_PH_RID, HOME_LEVEL

END
GO