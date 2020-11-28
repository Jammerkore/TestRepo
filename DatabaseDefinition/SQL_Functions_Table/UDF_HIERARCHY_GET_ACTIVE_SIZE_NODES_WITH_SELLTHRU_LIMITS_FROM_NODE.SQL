--dv Create date: 5/23/2013
--dv Modified:	5/23/2013
--dv Description:	Retrieves active descendant size nodes with SELLTHRU_LIMIT (inheritable) from table [NODE_SIZE_SELLTHRU] from a starting node
--dv				Excludes size descendants from the "unknown color" color nodes
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_ACTIVE_SIZE_NODES_WITH_SELLTHRU_LIMITS_FROM_NODE] 
(	
	@SELECTED_NODE_RID INT
)
RETURNS TABLE 
AS
RETURN 
(
	WITH cte_descendants(HN_RID, HOME_LEVEL, PH_RID, PARENT_HN_RID)
	AS 
	(
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

	SELECT 
		cte_descendants.HN_RID, 
		cte_descendants.HOME_LEVEL, 
		cte_descendants.PH_RID, 
		cte_descendants.PARENT_HN_RID, 
		inherited_node.SELLTHRU_LIMIT_FINAL,
		inherited_node.SELLTHRU_LIMIT_INHERITED_HN_RID,
		inherited_node.SELLTHRU_LIMIT_IS_INHERITED
	FROM cte_descendants
	CROSS APPLY [dbo].[UDF_HIERARCHY_GET_INHERITED_SELLTHRU_LIMIT_FROM_NODE](cte_descendants.HN_RID) inherited_node
	INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl ON phl.PHL_SEQUENCE=cte_descendants.HOME_LEVEL AND phl.PHL_TYPE=800204
	--Exclude sizes from hidden "Unknown Color" colors
	WHERE cte_descendants.PARENT_HN_RID NOT IN 
	(
		SELECT hn.HN_RID
		FROM [dbo].[HIERARCHY_NODE] hn
		INNER JOIN [dbo].[PRODUCT_HIERARCHY_LEVELS] phl ON phl.PHL_SEQUENCE=hn.HOME_LEVEL
		INNER JOIN [dbo].[COLOR_NODE] cn ON cn.HN_RID=hn.HN_RID AND cn.COLOR_CODE_RID = 0
		WHERE coalesce(hn.ACTIVE_IND, 1)=1 AND phl.PHL_TYPE=800203 
	)

)
GO
