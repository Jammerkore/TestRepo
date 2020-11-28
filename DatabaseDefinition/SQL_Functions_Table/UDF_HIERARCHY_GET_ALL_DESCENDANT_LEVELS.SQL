--dv Create date:	9/15/2015
--dv Modified:		9/15/2015
--dv Description:	Retrieves descendant node level count from a starting node
--dv				Excludes hidden color nodes
--dv                TT#1721-MD - RMatelic - Data Layer Request - Add CTE and Stored Procedure that will return the levels under an alternate hierarchy
--dv =============================================
CREATE FUNCTION [dbo].[UDF_HIERARCHY_GET_ALL_DESCENDANT_LEVELS] 
(      
       @SELECTED_NODE_RID INT
)
RETURNS TABLE 
AS
RETURN 
(
       --Using a common table expression
       WITH cte_descendants(HN_RID, HOME_LEVEL, PH_RID, PARENT_HN_RID, ACTIVE_IND)
       AS (
              SELECT
                     hn.HN_RID, hn.HOME_LEVEL, hnj.PH_RID, hnj.PARENT_HN_RID, hn.ACTIVE_IND
              FROM [dbo].[HIER_NODE_JOIN] hnj 
              INNER JOIN [dbo].[HIERARCHY_NODE] hn ON hn.HN_RID = hnj.HN_RID --AND coalesce(hn.ACTIVE_IND, 1)=1 --AND hn.HOME_PH_RID=hnj.PH_RID 
              WHERE hn.HN_RID=@SELECTED_NODE_RID
                AND hnj.NODE_DELETE_IND = '0' 

              UNION ALL
              SELECT 
                     hn.HN_RID, hn.HOME_LEVEL, hn.HOME_PH_RID, hnj.PARENT_HN_RID, hn.ACTIVE_IND
              FROM [dbo].[HIER_NODE_JOIN] hnj 
              INNER JOIN [dbo].[HIERARCHY_NODE] hn ON hn.HN_RID = hnj.HN_RID --AND coalesce(hn.ACTIVE_IND, 1)=1 --AND hn.HOME_PH_RID=hnj.PH_RID 
              INNER JOIN cte_descendants AS cte ON hnj.PARENT_HN_RID= cte.HN_RID AND hnj.PH_RID=cte.PH_RID
              WHERE hnj.NODE_DELETE_IND = '0'
              )


select distinct cte.PH_RID, cte.HOME_LEVEL 
FROM cte_descendants cte 
)

GO
