--dv Create date:	9/15/2013
--dv Description:	Gets the hidden color nodes and descendant size nodes to always be excluded (aka Unknown Color, aka Color Size Total)
--dv				This color node is present for every style that has sizes
--dv				This color node is never shown to the end user
--dv				This color node is used to hold a sum of all sizes for a style
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_ALL_DESCENDANT_HIDDEN_COLOR_AND_SIZE_NODE_RIDS_TO_EXCLUDE]
(	
	@SELECTED_NODE_RID INT
)
RETURNS TABLE 
AS
RETURN 
(
	--Using a common table expression
	WITH cte_ancestors(HN_RID, HOME_LEVEL, PH_RID)
	AS (
		SELECT
			hn.HN_RID, hn.HOME_LEVEL, hnj.PH_RID
		FROM [dbo].[HIER_NODE_JOIN] hnj 
		INNER JOIN [dbo].[HIERARCHY_NODE] hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID AND hn.HOME_PH_RID=hnj.PH_RID-- AND coalesce(hn.ACTIVE_IND, 1)=1
		WHERE hn.HN_RID=@SELECTED_NODE_RID
		UNION ALL
		SELECT 
			hn.HN_RID, hn.HOME_LEVEL, hnj.PH_RID
		FROM [dbo].[HIER_NODE_JOIN] hnj 
		INNER JOIN [dbo].[HIERARCHY_NODE] hn (NOLOCK) ON hn.HN_RID = hnj.HN_RID AND hn.HOME_PH_RID=hnj.PH_RID-- AND coalesce(hn.ACTIVE_IND, 1)=1
		INNER JOIN cte_ancestors AS cte ON hnj.PARENT_HN_RID= cte.HN_RID AND hnj.PH_RID=cte.PH_RID 
		)
	--Get just the color nodes to exclude here
	SELECT cte.HN_RID
	FROM cte_ancestors cte 
	INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl (NOLOCK) on phl.PHL_SEQUENCE=cte.HOME_LEVEL
	INNER JOIN [dbo].[COLOR_NODE] cn (NOLOCK) ON cn.HN_RID=cte.HN_RID AND COLOR_CODE_RID=0
	WHERE phl.PHL_TYPE=800203 
	--Rejoin with the hierarchy to get the size nodes to exclude
	UNION ALL
	SELECT hnj.HN_RID FROM [dbo].[HIER_NODE_JOIN] hnj (NOLOCK) WHERE hnj.PARENT_HN_RID
	IN (SELECT cte.HN_RID
	FROM cte_ancestors cte 
	INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl (NOLOCK) on phl.PHL_SEQUENCE=cte.HOME_LEVEL
	INNER JOIN [dbo].[COLOR_NODE] cn (NOLOCK) ON cn.HN_RID=cte.HN_RID AND COLOR_CODE_RID=0
	WHERE phl.PHL_TYPE=800203 
	)
)
GO
