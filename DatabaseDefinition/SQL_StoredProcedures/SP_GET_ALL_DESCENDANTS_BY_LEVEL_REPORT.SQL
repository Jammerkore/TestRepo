--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description:	Reads descendant nodes
--dv =============================================
CREATE PROCEDURE [dbo].[SP_GET_ALL_DESCENDANTS_BY_LEVEL_REPORT] 
	@SELECTED_NODE_RID INT,
	@LEVEL AS INT
AS
BEGIN
	DECLARE @Loop INT
	DECLARE @LoopCount INT
	DECLARE @NextLoopCount INT
	DECLARE @main_PH_RID INT

	SELECT @main_PH_RID = PH_RID
	FROM PRODUCT_HIERARCHY
	WHERE PH_TYPE = 800000

	SET @LoopCount = 0

	CREATE TABLE #TREE (
		LOOPCOUNT INT NOT NULL,
		PARENT_HN_RID INT NOT NULL,
		HOME_PH_RID INT,
		PH_TYPE INT,
		HN_RID INT NOT NULL,
		HOME_LEVEL INT NOT NULL,
		LEVEL_TYPE INT,
		BN_ID CHAR(356) COLLATE Latin1_General_CS_AS,
		BN_NAME CHAR(50) COLLATE Latin1_General_CS_AS,
		BN_DESCRIPTION CHAR(250) COLLATE Latin1_General_CS_AS
		)

	-- insert current node & the children of the node into the temp table
	INSERT #TREE (
		LOOPCOUNT,
		PARENT_HN_RID,
		HOME_PH_RID,
		PH_TYPE,
		HN_RID,
		HOME_LEVEL,
		BN_ID,
		BN_NAME,
		BN_DESCRIPTION
		)
	SELECT @LoopCount AS LOOPCOUNT,
		hnj.PARENT_HN_RID,
		hn.HOME_PH_RID,
		ph.PH_TYPE,
		@SELECTED_NODE_RID AS HN_RID,
		hn.HOME_LEVEL,
		--bn.BN_ID,
		dbo.UDF_MID_GET_NODE_DISPLAY (hn.HN_RID),
		bn.BN_NAME,
		bn.BN_DESCRIPTION
	FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
	JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID
		AND hn.HOME_PH_RID = hnj.PH_RID
	JOIN [dbo].[PRODUCT_HIERARCHY] ph WITH (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
	LEFT OUTER JOIN [dbo].[BASE_NODE] bn WITH (NOLOCK) ON hn.HN_RID = bn.HN_RID
	WHERE @SELECTED_NODE_RID = hnj.HN_RID
	  AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

	SET @Loop = @@ROWCOUNT

	-- add level type
	BEGIN TRANSACTION

	UPDATE #TREE
	SET LEVEL_TYPE = (
			SELECT phl.PHL_TYPE
			FROM [dbo].[PRODUCT_HIERARCHY_LEVELS] phl WITH (NOLOCK)
			WHERE #TREE.LOOPCOUNT = @LoopCount
				AND #TREE.HOME_PH_RID = phl.PH_RID
				AND #TREE.HOME_LEVEL = phl.PHL_SEQUENCE
			)

	COMMIT

	-- chase all paths until you get the correct level type
	SET @NextLoopCount = @LoopCount + 1

	WHILE @Loop > 0
	BEGIN
		INSERT #TREE
		SELECT @NextLoopCount AS LOOPCOUNT,
			hnj.PARENT_HN_RID,
			hn.HOME_PH_RID,
			ph.PH_TYPE,
			hnj.HN_RID,
			hn.HOME_LEVEL,
			NULL,
			--bn.BN_ID,
			dbo.UDF_MID_GET_NODE_DISPLAY (hn.HN_RID),
			bn.BN_NAME,
			bn.BN_DESCRIPTION
		FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
		JOIN #TREE t ON hnj.PARENT_HN_RID = t.HN_RID
			AND hnj.PH_RID = t.HOME_PH_RID
		JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID
			AND hn.HOME_PH_RID = hnj.PH_RID
		JOIN [dbo].[PRODUCT_HIERARCHY] ph WITH (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
		LEFT OUTER JOIN [dbo].[BASE_NODE] bn WITH (NOLOCK) ON hn.HN_RID = bn.HN_RID
		WHERE t.LOOPCOUNT = @LoopCount
			AND (
				t.HOME_PH_RID = hn.HOME_PH_RID
				AND t.HOME_LEVEL < (@LEVEL + 1)
				)
			AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

		SET @Loop = @@ROWCOUNT
		-- update counters
		SET @LoopCount = @LoopCount + 1
		SET @NextLoopCount = @LoopCount + 1
	END

	SELECT PARENT_HN_RID,
		HN_RID,
		BN_ID,
		BN_NAME,
		BN_DESCRIPTION
	FROM #TREE

	IF (SELECT object_id('tempdb.dbo.#TREE')) > 0 DROP TABLE #TREE

END
GO


