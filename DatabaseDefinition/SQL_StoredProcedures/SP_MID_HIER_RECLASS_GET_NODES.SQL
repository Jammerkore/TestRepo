--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description:	Reads nodes for reclass
--dv History: TT#3344 - JSmith - Modify Hierarchy Reclass to process color level
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_HIER_RECLASS_GET_NODES] 
	@HierarchyId VARCHAR(50)
AS
BEGIN
	DECLARE @Return_Code INT
	DECLARE @PH_RID INT
	DECLARE @PH_TYPE INT
	DECLARE @delimiter CHAR

	SELECT @delimiter = PRODUCT_LEVEL_DELIMITER
	FROM dbo.SYSTEM_OPTIONS

	SET @PH_RID = - 1

	SELECT @PH_RID = PH_RID,
		@PH_TYPE = PH_TYPE
	FROM dbo.PRODUCT_HIERARCHY
	WHERE PH_ID = @HierarchyId


	SELECT @HierarchyId AS HIERARCHY_ID,
		parentID.BN_ID AS PARENT_ID,
		CASE child.HN_TYPE
			WHEN 800203
				THEN style.BN_ID + @delimiter + cc.COLOR_CODE_ID
			ELSE childID.BN_ID
			END AS PRODUCT_ID,
		CASE child.HN_TYPE
			WHEN 800203
				THEN cc.COLOR_CODE_NAME
			ELSE childID.BN_NAME
			END AS BN_NAME,
		CASE child.HN_TYPE
			WHEN 800203
				THEN cn.COLOR_DESCRIPTION
			ELSE childID.BN_DESCRIPTION
			END AS BN_DESCRIPTION,
		child.HOME_PH_RID,
		child.HOME_LEVEL,
		prodHier.PH_TYPE
	FROM dbo.HIER_NODE_JOIN hnj WITH (NOLOCK)
	JOIN dbo.HIERARCHY_NODE parent WITH (NOLOCK) ON hnj.PARENT_HN_RID = parent.HN_RID
	LEFT OUTER JOIN dbo.BASE_NODE parentID WITH (NOLOCK) ON parent.HN_RID = parentID.HN_RID
	JOIN dbo.HIERARCHY_NODE child WITH (NOLOCK) ON hnj.HN_RID = child.HN_RID
	LEFT OUTER JOIN dbo.BASE_NODE childID WITH (NOLOCK) ON child.HN_RID = childID.HN_RID
	JOIN PRODUCT_HIERARCHY prodHier ON child.HOME_PH_RID = prodHier.PH_RID
	LEFT OUTER JOIN dbo.COLOR_NODE cn WITH (NOLOCK) ON cn.HN_RID = child.HN_RID
	LEFT OUTER JOIN dbo.COLOR_CODE cc WITH (NOLOCK) ON cc.COLOR_CODE_RID = cn.COLOR_CODE_RID
	LEFT OUTER JOIN dbo.HIER_NODE_JOIN stylejoin WITH (NOLOCK) ON stylejoin.HN_RID = child.HN_RID
		AND stylejoin.PH_RID = child.HOME_PH_RID
	LEFT OUTER JOIN dbo.BASE_NODE style WITH (NOLOCK) ON style.HN_RID = stylejoin.PARENT_HN_RID
	WHERE hnj.PH_RID = @PH_RID
		AND (
			(child.HN_TYPE < 800203)
			OR (
				child.HN_TYPE = 800203
				AND @PH_TYPE = 800000    -- MERCH-4010  Changed to '=' from '<>'
				)
			)
		AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

END
GO


