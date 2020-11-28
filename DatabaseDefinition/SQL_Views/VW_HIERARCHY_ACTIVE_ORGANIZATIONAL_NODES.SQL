--dv =============================================
--dv Create date: 1/15/2013
--dv Description:	Retrieves all active organizational hierarchy nodes
--dv =============================================
CREATE VIEW [dbo].[VW_HIERARCHY_ACTIVE_ORGANIZATIONAL_NODES]
AS

	SELECT 
		HN_RID, 
		HN_TYPE, 
		HN_RID % 10 as HN_MOD 
	FROM dbo.HIERARCHY_NODE WITH (NOLOCK, INDEX(HIERARCHY_NODE_PK))  
	WHERE HOME_PH_RID = (SELECT PH_RID FROM dbo.PRODUCT_HIERARCHY WITH (NOLOCK) WHERE PH_TYPE = 800000) 
		  AND COALESCE(ACTIVE_IND, 1)=1

GO