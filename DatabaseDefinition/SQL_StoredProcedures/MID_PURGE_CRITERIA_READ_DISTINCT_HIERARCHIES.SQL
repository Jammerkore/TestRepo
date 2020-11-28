--dv =============================================
--dv Create date:	07/22/2016
--dv Description:	Determines hierarchies that contain purge criteria
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_CRITERIA_READ_DISTINCT_HIERARCHIES]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT HOME_PH_RID, PH_TYPE
	FROM
	(
	  -- Check for Purge Criteria by Node
	  SELECT hn.HOME_PH_RID
	  FROM HIERARCHY_NODE hn with (nolock)
	  JOIN PURGE_CRITERIA pc with (nolock) on pc.HN_RID = hn.HN_RID
	  UNION
	  -- Check for Header Purge Criteria by Node
	  SELECT hn.HOME_PH_RID
	  FROM HIERARCHY_NODE hn with (nolock)
	  JOIN PURGE_CRITERIA_HEADER_PURGE pchp with (nolock) on pchp.HN_RID = hn.HN_RID
	  UNION
	  SELECT hn.HOME_PH_RID
	  FROM HIERARCHY_NODE hn with (nolock)
	  JOIN HIER_NODE_PROPERTIES hnp with (nolock) on hnp.HN_RID = hn.HN_RID
	  WHERE hnp.APPLY_HN_RID_FROM is not null
	  UNION
	  -- Check for Purge Criteria by Hierarchy
	  SELECT PH_RID as [HOME_PH_RID]
	  FROM PRODUCT_HIERARCHY_LEVELS phl with (nolock)
	  WHERE (
	         phl.PURGE_DAILY_HISTORY is not null
	         or phl.PURGE_WEEKLY_HISTORY is not null
			 or phl.PURGE_PLANS is not null
			)
	  UNION
	  -- Check for Header Purge Criteria by Hierarchy
	  SELECT PH_RID as [HOME_PH_RID]
	  FROM PRODUCT_HIERARCHY_LEVELS_HEADER_PURGE phlhp with (nolock)
	  WHERE (
	         phlhp.PURGE_HEADERS is not null
			)
	) x
	join PRODUCT_HIERARCHY ph with (nolock) on ph.PH_RID = x.HOME_PH_RID
	order by ph.PH_TYPE


END
GO
