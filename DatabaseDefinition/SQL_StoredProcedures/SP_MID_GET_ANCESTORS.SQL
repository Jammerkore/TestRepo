CREATE PROCEDURE [dbo].[SP_MID_GET_ANCESTORS] 
	@HN_RID INT,
	@PH_RID INT,
	@HierarchySearchType INT, -- follows eHierarchySearchType
	@debug BIT = 0
AS
SET NOCOUNT ON

DECLARE @Loop INT,
	@SaveCount INT,
	@HOME_PH_RID INT

CREATE TABLE #TEMP (
	LOOPCOUNT INT NOT NULL,
	HN_RID INT NOT NULL,
	HOME_PH_RID INT NOT NULL,
	HOME_LEVEL INT NOT NULL
	)

SELECT @Loop = 0

SELECT @HOME_PH_RID = HOME_PH_RID
FROM [dbo].[HIERARCHY_NODE] WITH (NOLOCK)
WHERE HN_RID = @HN_RID

-- insert the node into the temp table
INSERT #TEMP (
	LOOPCOUNT,
	HN_RID,
	HOME_PH_RID,
	HOME_LEVEL
	)
SELECT @Loop,
	@HN_RID,
	hn.HOME_PH_RID,
	hn.HOME_LEVEL
FROM [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK)
WHERE hn.HN_RID = @HN_RID

SET @SaveCount = @@ROWCOUNT

-- chase all paths
WHILE @SaveCount > 0
BEGIN
	PRINT @Loop

	IF @HierarchySearchType = 0 -- home hierarchy only
	BEGIN
		INSERT #TEMP (
			LOOPCOUNT,
			HN_RID,
			HOME_PH_RID,
			HOME_LEVEL
			)
		SELECT @Loop + 1,
			hnj.PARENT_HN_RID,
			hn.HOME_PH_RID,
			hn.HOME_LEVEL
		FROM #TEMP t WITH (NOLOCK)
		JOIN [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK) ON hnj.HN_RID = t.HN_RID
		JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.PARENT_HN_RID
		--JOIN [dbo].[PRODUCT_HIERARCHY] ph WITH (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
		WHERE t.LOOPCOUNT = @Loop
			AND hnj.PH_RID = @HOME_PH_RID
			AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

		SET @SaveCount = @@ROWCOUNT
	END
	ELSE IF @HierarchySearchType = 1 -- alternates only
	BEGIN
		INSERT #TEMP (
			LOOPCOUNT,
			HN_RID,
			HOME_PH_RID,
			HOME_LEVEL
			)
		SELECT @Loop + 1,
			hnj.PARENT_HN_RID,
			hn.HOME_PH_RID,
			hn.HOME_LEVEL
		FROM #TEMP t WITH (NOLOCK)
		JOIN [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK) ON hnj.HN_RID = t.HN_RID
		JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.PARENT_HN_RID
		JOIN [dbo].[PRODUCT_HIERARCHY] ph WITH (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
		WHERE t.LOOPCOUNT = @Loop
			AND ph.PH_TYPE = 800001
			AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

		SET @SaveCount = @@ROWCOUNT
	END
	ELSE IF @HierarchySearchType = 2 -- specific hierarchy
	BEGIN
		INSERT #TEMP (
			LOOPCOUNT,
			HN_RID,
			HOME_PH_RID,
			HOME_LEVEL
			)
		SELECT @Loop + 1,
			hnj.PARENT_HN_RID,
			hn.HOME_PH_RID,
			hn.HOME_LEVEL
		FROM #TEMP t WITH (NOLOCK)
		JOIN [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK) ON hnj.HN_RID = t.HN_RID
		JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.PARENT_HN_RID
		--JOIN [dbo].[PRODUCT_HIERARCHY] ph WITH (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
		WHERE t.LOOPCOUNT = @Loop
			AND hnj.PH_RID = @PH_RID
			AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

		SET @SaveCount = @@ROWCOUNT
	END
	ELSE -- all hierarchies
	BEGIN
		INSERT #TEMP (
			LOOPCOUNT,
			HN_RID,
			HOME_PH_RID,
			HOME_LEVEL
			)
		SELECT @Loop + 1,
			hnj.PARENT_HN_RID,
			hn.HOME_PH_RID,
			hn.HOME_LEVEL
		FROM #TEMP t WITH (NOLOCK)
		JOIN [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK) ON hnj.HN_RID = t.HN_RID
		JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.PARENT_HN_RID
		--JOIN [dbo].[PRODUCT_HIERARCHY] ph WITH (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
		WHERE t.LOOPCOUNT = @Loop
			AND hn.HOME_PH_RID != 0 -- TT#3424 - JSmith - ROLLUP_ITEM has 'bogus' entries with HN_RID = 0
			AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

		SET @SaveCount = @@ROWCOUNT
	END

	SET @Loop = @Loop + 1
END

SELECT DISTINCT HN_RID,
	HOME_PH_RID,
	HOME_LEVEL
FROM #TEMP
ORDER BY HOME_LEVEL DESC
GO


