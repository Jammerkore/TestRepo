--dv =============================================
--dv Create date: 1/10/2014
--dv Description: Returns a table of child size rids, excluding hidden color/sizes
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_SIZE_DESCENDANT_NODE_RIDS_NOLOCK] 
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
			hn.HN_RID, hn.HOME_LEVEL, hn.HOME_PH_RID, hnj.PARENT_HN_RID
		FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
		INNER JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID AND hn.HOME_PH_RID=hnj.PH_RID --AND coalesce(hn.ACTIVE_IND, 1)=1
		WHERE hn.HN_RID=@SELECTED_NODE_RID
		UNION ALL
		SELECT 
			hn.HN_RID, hn.HOME_LEVEL, hn.HOME_PH_RID, hnj.PARENT_HN_RID
		FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK) 
		INNER JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID --AND hn.HOME_PH_RID=hnj.PH_RID --AND coalesce(hn.ACTIVE_IND, 1)=1
		INNER JOIN cte_descendants AS cte ON hnj.PARENT_HN_RID= cte.HN_RID AND hnj.PH_RID=cte.PH_RID
		)
	SELECT cte.HN_RID
	FROM cte_descendants cte 
	INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl WITH (NOLOCK) ON phl.PHL_SEQUENCE=cte.HOME_LEVEL
	WHERE phl.PHL_TYPE=800204
	--Exclude sizes from hidden "Unknown Color" colors
	AND cte.PARENT_HN_RID NOT IN 
	(
		SELECT hn.HN_RID
		FROM [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK)
		INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl WITH (NOLOCK) ON phl.PHL_SEQUENCE=hn.HOME_LEVEL
		INNER JOIN [dbo].[COLOR_NODE] cn WITH (NOLOCK) ON cn.HN_RID=hn.HN_RID AND cn.COLOR_CODE_RID = 0
		WHERE phl.PHL_TYPE=800203 --AND coalesce(hn.ACTIVE_IND, 1)=1
	)
)
GO

