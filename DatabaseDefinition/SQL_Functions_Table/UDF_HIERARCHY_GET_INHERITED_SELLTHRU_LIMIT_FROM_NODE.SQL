--dv Create date: 5/23/2013
--dv Modified:	5/23/2013
--dv Description:	Retrieves inherited or current node SELLTHRU_LIMIT from table [NODE_SIZE_SELLTHRU] from a starting node
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_INHERITED_SELLTHRU_LIMIT_FROM_NODE] 
(	
	@SELECTED_NODE_RID INT
)
RETURNS TABLE 
AS
RETURN 
(
	WITH cte_ancestors(HN_RID, HOME_LEVEL, PH_RID, PARENT_HN_RID)
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
		INNER JOIN cte_ancestors AS cte ON hnj.HN_RID= cte.PARENT_HN_RID AND hnj.PH_RID=cte.PH_RID
	)
	SELECT	current_node.HN_RID, 
			current_node.HOME_LEVEL, 
			current_node.PH_RID, 
			current_node.PARENT_HN_RID,
			CASE 
				WHEN current_node.SELLTHRU_LIMIT IS NULL AND inherited_node.SELLTHRU_LIMIT_INHERITED IS NULL THEN NULL
				WHEN current_node.SELLTHRU_LIMIT IS NOT NULL THEN current_node.SELLTHRU_LIMIT
				ELSE inherited_node.SELLTHRU_LIMIT_INHERITED
			END AS SELLTHRU_LIMIT_FINAL,
			CASE 
				WHEN current_node.SELLTHRU_LIMIT IS NULL AND inherited_node.SELLTHRU_LIMIT_INHERITED IS NULL THEN 0
				WHEN current_node.SELLTHRU_LIMIT IS NOT NULL THEN 0
				ELSE 1
			END AS SELLTHRU_LIMIT_IS_INHERITED,
			inherited_node.SELLTHRU_LIMIT_INHERITED_HN_RID
	FROM
	(
	SELECT	cte_ancestors.HN_RID, 
			cte_ancestors.HOME_LEVEL, 
			cte_ancestors.PH_RID, 
			cte_ancestors.PARENT_HN_RID, 
			nsst.SELLTHRU_LIMIT
	FROM cte_ancestors
	LEFT OUTER JOIN [dbo].[NODE_SIZE_SELLTHRU] nsst on nsst.HN_RID = cte_ancestors.HN_RID
	WHERE cte_ancestors.HN_RID=@SELECTED_NODE_RID
	) current_node
	INNER JOIN 
	(
			SELECT TOP 1 
			nsst.SELLTHRU_LIMIT AS SELLTHRU_LIMIT_INHERITED,
			nsst.HN_RID AS SELLTHRU_LIMIT_INHERITED_HN_RID 
			FROM cte_ancestors
			LEFT OUTER JOIN [dbo].[NODE_SIZE_SELLTHRU] nsst on nsst.HN_RID = cte_ancestors.HN_RID
			WHERE nsst.SELLTHRU_LIMIT is not null
	) inherited_node ON 1=1

)
GO
