CREATE PROCEDURE [dbo].[SP_MID_GET_ALL_AFFECTED_DESCENDANTS] 
	@HN_RID INT,
	@IMO INT, --	IMO
	@SE INT, --	Store Eligibility
	@CHAR INT, --	Characteristics
	@SC INT, --	Store Capacity
	@DP INT, --	Daily Percentages
	@PC INT, --	Purge Criteria
	@CSP INT --	Chain Set Percentage
	WITH RECOMPILE
AS
BEGIN
	DECLARE @Loop INT,
		@LoopCount INT,
		@NextLoopCount INT,
		@rowCount INT,
		@HOME_PH_RID INT

	SET NOCOUNT ON
	SET @LoopCount = 0

	SELECT @HOME_PH_RID = HOME_PH_RID
	FROM HIERARCHY_NODE
	WHERE HN_RID = @HN_RID

	CREATE TABLE #TREE (
		LOOPCOUNT INT NOT NULL,
		PARENT_HN_RID INT NOT NULL,
		HOME_PH_RID INT,
		HN_RID INT NOT NULL,
		HOME_LEVEL INT NOT NULL,
		LEVEL_TYPE INT,
		VIRTUAL_IND CHAR(1)
		)

	-- insert the children of the node into the temp table
	INSERT #TREE (
		LOOPCOUNT,
		PARENT_HN_RID,
		HOME_PH_RID,
		HN_RID,
		HOME_LEVEL,
		VIRTUAL_IND
		)
	SELECT @LoopCount AS LOOPCOUNT,
		@HN_RID AS PARENT_HN_RID,
		hn.HOME_PH_RID,
		hnj.HN_RID,
		hn.HOME_LEVEL,
		hn.VIRTUAL_IND
	FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
	JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID
	WHERE @HN_RID = hnj.PARENT_HN_RID
		AND coalesce(hn.VIRTUAL_IND, 0) = 0
		AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy
	OPTION (MAXDOP 1)

	SET @rowCount = @@ROWCOUNT

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

	-- chase all paths until you get the correct level offset
	SET @NextLoopCount = @LoopCount + 1

	WHILE @rowCount > 0
	BEGIN
		INSERT #TREE
		SELECT @NextLoopCount AS LOOPCOUNT,
			hnj.PARENT_HN_RID,
			hn.HOME_PH_RID,
			hnj.HN_RID,
			hn.HOME_LEVEL,
			NULL,
			hn.VIRTUAL_IND
		FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
		JOIN #TREE t ON hnj.PARENT_HN_RID = t.HN_RID
			AND hnj.PH_RID = t.HOME_PH_RID
		JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID
		WHERE t.LOOPCOUNT = @LoopCount
			AND coalesce(hn.VIRTUAL_IND, 0) = 0
			AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy
		OPTION (MAXDOP 1)

		SET @rowCount = @@ROWCOUNT
		-- update counters
		SET @LoopCount = @LoopCount + 1
		SET @NextLoopCount = @LoopCount + 1

		-- remove nodes from other hierarchies and add level type
		BEGIN TRANSACTION

		DELETE
		FROM #TREE
		WHERE HOME_PH_RID <> @HOME_PH_RID

		UPDATE #TREE
		SET LEVEL_TYPE = (
				SELECT phl.PHL_TYPE
				FROM [dbo].[PRODUCT_HIERARCHY_LEVELS] phl WITH (NOLOCK)
				WHERE #TREE.LOOPCOUNT = @LoopCount
					AND #TREE.HOME_PH_RID = phl.PH_RID
					AND #TREE.HOME_LEVEL = phl.PHL_SEQUENCE
				)
		WHERE LEVEL_TYPE IS NULL
			AND #TREE.LOOPCOUNT = @LoopCount

		COMMIT
	END

	-- output the results - only the descendants
	SELECT a.HOME_PH_RID,
		0 AS PH_TYPE,
		a.HN_RID,
		a.HOME_LEVEL,
		a.PARENT_HN_RID
	FROM (
		SELECT t.HOME_PH_RID,
			0 AS PH_TYPE,
			t.HN_RID,
			t.HOME_LEVEL,
			t.PARENT_HN_RID
		FROM #TREE t
		INNER JOIN NODE_IMO no ON (
				(t.HN_RID = no.HN_RID)
				AND (@IMO = 1)
				)
		
		UNION
		
		SELECT t.HOME_PH_RID,
			0,
			t.HN_RID,
			t.HOME_LEVEL,
			t.PARENT_HN_RID
		FROM #TREE t
		INNER JOIN STORE_ELIGIBILITY se ON (
				(t.HN_RID = se.HN_RID)
				AND (@SE = 1)
				)
		
		UNION
		
		SELECT t.HOME_PH_RID,
			0,
			t.HN_RID,
			t.HOME_LEVEL,
			t.PARENT_HN_RID
		FROM #TREE t
		INNER JOIN STORE_CAPACITY sc ON (
				(t.HN_RID = sc.HN_RID)
				AND (@SC = 1)
				)
		
		UNION
		
		SELECT t.HOME_PH_RID,
			0,
			t.HN_RID,
			t.HOME_LEVEL,
			t.PARENT_HN_RID
		FROM #TREE t
		INNER JOIN DAILY_PERCENTAGES_DEFAULTS dpd ON (
				(t.HN_RID = dpd.HN_RID)
				AND (@DP = 1)
				)
		
		UNION
		
		SELECT t.HOME_PH_RID,
			0,
			t.HN_RID,
			t.HOME_LEVEL,
			t.PARENT_HN_RID
		FROM #TREE t
		INNER JOIN DAILY_PERCENTAGES dp ON (
				(t.HN_RID = dp.HN_RID)
				AND (@DP = 1)
				)
		
		UNION
		
		SELECT t.HOME_PH_RID,
			0,
			t.HN_RID,
			t.HOME_LEVEL,
			t.PARENT_HN_RID
		FROM #TREE t
		INNER JOIN PURGE_CRITERIA pc ON (
				(t.HN_RID = pc.HN_RID)
				AND (@PC = 1)
				)
		
		UNION
		
		SELECT t.HOME_PH_RID,
			0,
			t.HN_RID,
			t.HOME_LEVEL,
			t.PARENT_HN_RID
		FROM #TREE t
		INNER JOIN CHAIN_SET_PERCENT_SET csp ON (
				(t.HN_RID = csp.HN_RID)
				AND (@CSP = 1)
				)
		) a
	WHERE a.HN_RID <> @HN_RID

	-- clean up the temp table
	IF (
			SELECT object_id('tempdb.dbo.#TREE')
			) > 0
		DROP TABLE #TREE
END
GO


