--dv Create date: 5/2/2013
--dv Modified:	5/9/2013
--dv Description:	Retrieves active descendant color node RIDs from a starting node
--dv				Excludes the hidden "Unknown Colors" colors (COLOR_CODE_RID=0)
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_ACTIVE_COLOR_DESCENDANT_NODE_RIDS] 
(	
	@SELECTED_NODE_RID INT
)
RETURNS TABLE 
AS
RETURN 
(
	--Using a common table expression
	WITH cte_descendants(HN_RID, HOME_LEVEL, PH_RID, PARENT_HN_RID)
	AS (
		SELECT
			hn.HN_RID, hn.HOME_LEVEL, hnj.PH_RID, hnj.PARENT_HN_RID
		FROM [dbo].[HIER_NODE_JOIN] hnj 
		INNER JOIN [dbo].[HIERARCHY_NODE] hn ON hn.HN_RID = hnj.HN_RID AND hn.HOME_PH_RID=hnj.PH_RID AND coalesce(hn.ACTIVE_IND, 1)=1
		WHERE hn.HN_RID=@SELECTED_NODE_RID

		UNION ALL
		SELECT 
			hn.HN_RID, hn.HOME_LEVEL, hnj.PH_RID, hnj.PARENT_HN_RID
		FROM [dbo].[HIER_NODE_JOIN] hnj 
		INNER JOIN [dbo].[HIERARCHY_NODE] hn ON hn.HN_RID = hnj.HN_RID AND hn.HOME_PH_RID=hnj.PH_RID AND coalesce(hn.ACTIVE_IND, 1)=1
		INNER JOIN cte_descendants AS cte ON hnj.PARENT_HN_RID= cte.HN_RID AND hnj.PH_RID=cte.PH_RID
		)
	SELECT cte.HN_RID
	FROM cte_descendants cte 
	INNER JOIN [dbo].[COLOR_NODE] cn ON cn.HN_RID=cte.HN_RID AND cn.COLOR_CODE_RID <> 0
	INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl on phl.PHL_SEQUENCE=cte.HOME_LEVEL
	WHERE phl.PHL_TYPE=800203
)
GO
