--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description:	Reads descendant node RIDs
--dv =============================================
CREATE PROCEDURE [dbo].[SP_GET_DESCENDANTS_BY_LEVEL_REPORT] 
	@SELECTED_NODE_RID INT,
	@LEVEL INT
AS
BEGIN
	DECLARE @Loop INT
	DECLARE @LoopCount INT
	DECLARE @NextLoopCount INT

	SET @LoopCount = 0

	CREATE TABLE #TREE (
		LOOPCOUNT INT NOT NULL,
		PARENT_HN_RID INT NOT NULL,
		HOME_PH_RID INT,
		HN_RID INT NOT NULL,
		HOME_LEVEL INT NOT NULL,
		LEVEL_TYPE INT
		)

	-- insert current node & the children of the node into the temp table
	INSERT #TREE (
		LOOPCOUNT,
		PARENT_HN_RID,
		HOME_PH_RID,
		HN_RID,
		HOME_LEVEL
		)
	SELECT @LoopCount AS LOOPCOUNT,
		hnj.PARENT_HN_RID,
		hn.HOME_PH_RID,
		@SELECTED_NODE_RID AS HN_RID,
		hn.HOME_LEVEL
	FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
	JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID
	WHERE @SELECTED_NODE_RID = hnj.HN_RID
	  AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

	SET @Loop = @@ROWCOUNT
	-- chase all paths until you get the correct level type
	SET @NextLoopCount = @LoopCount + 1

	WHILE @Loop > 0
	BEGIN
		INSERT #TREE
		SELECT @NextLoopCount AS LOOPCOUNT,
			hnj.PARENT_HN_RID,
			hn.HOME_PH_RID,
			hnj.HN_RID,
			hn.HOME_LEVEL,
			NULL
		FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
		JOIN #TREE t ON hnj.PARENT_HN_RID = t.HN_RID
			AND hnj.PH_RID = t.HOME_PH_RID
		JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID
		WHERE t.LOOPCOUNT = @LoopCount
			AND (t.HOME_LEVEL < (@LEVEL))
			AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

		SET @Loop = @@ROWCOUNT
		-- update counters
		SET @LoopCount = @LoopCount + 1
		SET @NextLoopCount = @LoopCount + 1
	END

	SELECT DISTINCT HN_RID
	FROM #TREE
	WHERE HOME_LEVEL = @LEVEL

	IF (SELECT object_id('tempdb.dbo.#TREE')) > 0 DROP TABLE #TREE

END
GO


