--dv =============================================
--dv Create date: 11/19/2014
--dv Description:	Retrieves all nodes for organizational and alternate hierarchies from a starting node
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_FROM_NODE] 
(	
	@SELECTED_NODE_RID INT,
	@SELECTED_PH_RID INT
)
RETURNS TABLE 
AS
RETURN 
(
	WITH cte_descendants(HN_RID, HN_TYPE, HOME_PH_RID, HOME_LEVEL, ACTIVE_IND, PH_RID, PARENT_HN_RID, PARENT_PH_RID)
		AS (
			SELECT
				hn.HN_RID, 
				hn.HN_TYPE, 
				hn.HOME_PH_RID,
				hn.HOME_LEVEL, 
				hn.ACTIVE_IND, 
				hnj.PH_RID, 
				hnj.PARENT_HN_RID,
				hn.HOME_PH_RID as PARENT_PH_RID
			FROM HIER_NODE_JOIN hnj WITH (NOLOCK)
			INNER JOIN HIERARCHY_NODE hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID
			WHERE hn.HN_RID=@SELECTED_NODE_RID AND PH_RID=@SELECTED_PH_RID
		
			UNION ALL
			SELECT 
				hn.HN_RID, 
				hn.HN_TYPE, 
				hn.HOME_PH_RID,
				hn.HOME_LEVEL, 
				hn.ACTIVE_IND, 
				hnj.PH_RID, 
				hnj.PARENT_HN_RID,
				cte.HOME_PH_RID AS PARENT_PH_RID
			FROM HIER_NODE_JOIN hnj WITH (NOLOCK)
			INNER JOIN HIERARCHY_NODE hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID
			INNER JOIN cte_descendants cte ON hnj.PARENT_HN_RID = cte.HN_RID 
			)

	SELECT HN_RID 
	FROM cte_descendants cte 
	

)
GO