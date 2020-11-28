--dv Create date: 5/8/2013
--dv Modified:	1/15/2014
--dv Description:	Retrieves active descendant size nodes from a starting node
--dv				Excludes the hidden "Unknown Colors" colors (COLOR_CODE_RID=0)
--dv History: 1/15/2014 Added HN_MOD to the return set, using organizational view, removed PRODUCT_HIERARCHY_LEVELS join
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_ACTIVE_SIZE_DESCENDANT_NODE_RIDS] 
(	
	@SELECTED_NODE_RID INT
)
RETURNS TABLE 
AS
RETURN 
(
	--Using a common table expression
	WITH cte_descendants(HN_RID, PH_RID, PARENT_HN_RID, HN_TYPE)
	AS (
		SELECT
			hn.HN_RID, hnj.PH_RID, hnj.PARENT_HN_RID, hn.HN_TYPE
		FROM [dbo].[HIER_NODE_JOIN] hnj 
		INNER JOIN [dbo].[VW_HIERARCHY_ACTIVE_ORGANIZATIONAL_NODES] hn ON hn.HN_RID = hnj.HN_RID --AND hn.HOME_PH_RID=hnj.PH_RID AND coalesce(hn.ACTIVE_IND, 1)=1
		WHERE hn.HN_RID=@SELECTED_NODE_RID
		UNION ALL
		SELECT 
			hn.HN_RID, hnj.PH_RID, hnj.PARENT_HN_RID, hn.HN_TYPE
		FROM [dbo].[HIER_NODE_JOIN] hnj 
		INNER JOIN [dbo].[VW_HIERARCHY_ACTIVE_ORGANIZATIONAL_NODES] hn ON hn.HN_RID = hnj.HN_RID --AND hn.HOME_PH_RID=hnj.PH_RID AND coalesce(hn.ACTIVE_IND, 1)=1
		INNER JOIN cte_descendants AS cte ON hnj.PARENT_HN_RID= cte.HN_RID AND hnj.PH_RID=cte.PH_RID
		)
	SELECT cte.HN_RID, cte.HN_RID % 10 as HN_MOD
	FROM cte_descendants cte 
	--INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl ON phl.PHL_SEQUENCE=cte.HOME_LEVEL
	--WHERE phl.PHL_TYPE=800204
	WHERE cte.HN_TYPE=800204 --sizes only
	--Exclude sizes from hidden "Unknown Color" colors
	AND cte.PARENT_HN_RID NOT IN 
	(
		SELECT hn.HN_RID
		FROM [dbo].[HIERARCHY_NODE] hn
		--INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl ON phl.PHL_SEQUENCE=hn.HOME_LEVEL
		INNER JOIN [dbo].[COLOR_NODE] cn ON cn.HN_RID=hn.HN_RID AND cn.COLOR_CODE_RID = 0
		WHERE coalesce(hn.ACTIVE_IND, 1)=1 
	)
 
	
)
GO

