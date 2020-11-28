CREATE PROCEDURE [dbo].[SP_MID_GET_INTRANSIT_READ_NODES] 
	@HN_RID INT,
	@LEVEL_TYPE INT
	WITH RECOMPILE
AS
SET NOCOUNT ON

DECLARE @Loop INT,
	@LoopCount INT,
	@NextLoopCount INT,
	@StyleLevel INT,
	@UndefinedLevelType INT,
	@ParentOfStyleLevelType INT,
	@StyleLevelType INT,
	@ColorLevelType INT,
	@SizeLevelType INT

-- level type values
SET @UndefinedLevelType = 800200
SET @ParentOfStyleLevelType = 800201
SET @StyleLevelType = 800202
SET @ColorLevelType = 800203
SET @SizeLevelType = 800204

-- reset parent of style so values are in sequence
IF @LEVEL_TYPE = 800205
BEGIN
	SET @LEVEL_TYPE = @ParentOfStyleLevelType
END

SELECT @StyleLevel = PHL_SEQUENCE
FROM [dbo].[PRODUCT_HIERARCHY_LEVELS] WITH (NOLOCK)
WHERE PHL_TYPE = @StyleLevelType

-- create work table
SET @LoopCount = 0

CREATE TABLE #TREE (
	LOOPCOUNT INT NOT NULL,
	PARENT_HN_RID INT NOT NULL,
	HOME_PH_RID INT,
	PH_TYPE INT,
	HN_RID INT NOT NULL,
	HOME_LEVEL INT NOT NULL,
	LEVEL_TYPE INT,
	COLOR_CODE_RID INT,
	SIZE_CODE_RID INT,
	STYLE_IN_PATH CHAR
	)

-- insert the node into the temp table
INSERT #TREE (
	LOOPCOUNT,
	PARENT_HN_RID,
	HOME_PH_RID,
	PH_TYPE,
	HN_RID,
	HOME_LEVEL,
	COLOR_CODE_RID,
	SIZE_CODE_RID,
	STYLE_IN_PATH
	)
SELECT @LoopCount AS LOOPCOUNT,
	- 1 AS PARENT_HN_RID,
	hn.HOME_PH_RID,
	ph.PH_TYPE,
	@HN_RID,
	hn.HOME_LEVEL,
	- 1,
	- 1,
	'0'
FROM [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK)
JOIN [dbo].[PRODUCT_HIERARCHY] ph WITH (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
WHERE @HN_RID = hn.HN_RID

SET @Loop = @@ROWCOUNT

BEGIN TRANSACTION

-- add level type
UPDATE #TREE
SET LEVEL_TYPE = (
		SELECT phl.PHL_TYPE
		FROM [dbo].[PRODUCT_HIERARCHY_LEVELS] phl WITH (NOLOCK)
		WHERE #TREE.LOOPCOUNT = @LoopCount
			AND #TREE.HOME_PH_RID = phl.PH_RID
			AND #TREE.HOME_LEVEL = phl.PHL_SEQUENCE
		)

UPDATE #TREE
SET LEVEL_TYPE = @ParentOfStyleLevelType
WHERE #TREE.LOOPCOUNT = @LoopCount
	AND PH_TYPE = 800000
	AND #TREE.HOME_LEVEL = @StyleLevel - 1

-- set flag if style in path
UPDATE #TREE
SET STYLE_IN_PATH = '1'
WHERE LEVEL_TYPE = @StyleLevelType

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
		t.COLOR_CODE_RID,
		- 1,
		t.STYLE_IN_PATH
	FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
	JOIN #TREE t ON hnj.PARENT_HN_RID = t.HN_RID
		AND hnj.PH_RID = t.HOME_PH_RID
	JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID
	JOIN [dbo].[PRODUCT_HIERARCHY] ph WITH (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
	WHERE t.LOOPCOUNT = @LoopCount
		AND (
			t.LEVEL_TYPE IS NULL
			OR t.LEVEL_TYPE < @LEVEL_TYPE
			)
	OPTION (MAXDOP 1)

	SET @Loop = @@ROWCOUNT
	-- update counters
	SET @LoopCount = @LoopCount + 1
	SET @NextLoopCount = @LoopCount + 1

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
	WHERE LEVEL_TYPE IS NULL
		AND #TREE.LOOPCOUNT = @LoopCount

	UPDATE #TREE
	SET LEVEL_TYPE = @ParentOfStyleLevelType
	WHERE LEVEL_TYPE = @UndefinedLevelType
		AND #TREE.LOOPCOUNT = @LoopCount
		AND PH_TYPE = 800000
		AND #TREE.HOME_LEVEL = @StyleLevel - 1

	-- set flag if style in path
	UPDATE #TREE
	SET STYLE_IN_PATH = '1'
	WHERE LEVEL_TYPE = @StyleLevelType

	COMMIT

	-- add code rids for color and size nodes
	--if @LEVEL_TYPE > @StyleLevelType 
	--begin
	BEGIN TRANSACTION

	UPDATE #TREE
	SET COLOR_CODE_RID = (
			SELECT cn.COLOR_CODE_RID
			FROM [dbo].[COLOR_NODE] cn WITH (NOLOCK)
			WHERE #TREE.HN_RID = cn.HN_RID
			)
	WHERE COLOR_CODE_RID = - 1
		AND LEVEL_TYPE = @ColorLevelType

	IF @LEVEL_TYPE = @SizeLevelType
	BEGIN
		UPDATE #TREE
		SET COLOR_CODE_RID = (
				SELECT cn.COLOR_CODE_RID
				FROM [dbo].[COLOR_NODE] cn WITH (NOLOCK)
				WHERE cn.HN_RID = @HN_RID
				)
		WHERE COLOR_CODE_RID = - 1
			AND LEVEL_TYPE = @SizeLevelType
	END

	--if @LEVEL_TYPE > @ColorLevelType 
	--begin
	-- add size code rids
	UPDATE #TREE
	SET SIZE_CODE_RID = (
			SELECT sn.SIZE_CODE_RID
			FROM [dbo].[SIZE_NODE] sn WITH (NOLOCK)
			WHERE #TREE.HN_RID = sn.HN_RID
			)
	WHERE SIZE_CODE_RID = - 1
		AND LEVEL_TYPE = @SizeLevelType

	--end
	COMMIT
		--end
END -- While

-- set parent of style back to correct code
IF @LEVEL_TYPE = @ParentOfStyleLevelType
BEGIN
	UPDATE #TREE
	SET LEVEL_TYPE = 800205
	WHERE LEVEL_TYPE = @ParentOfStyleLevelType
END

-- get color code rid for sizes where color code is not known
UPDATE #TREE
SET COLOR_CODE_RID = (
		SELECT cn.COLOR_CODE_RID
		FROM [dbo].[COLOR_NODE] cn WITH (NOLOCK)
		WHERE cn.HN_RID = (
				SELECT PARENT_HN_RID
				FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
				INNER JOIN [dbo].[HIERARCHY_NODE] hn ON hnj.HN_RID = hn.HN_RID
					AND hn.HOME_PH_RID = hnj.PH_RID
				WHERE #TREE.HN_RID = hnj.HN_RID
				)
		)
WHERE COLOR_CODE_RID = - 1
	AND SIZE_CODE_RID > - 1

SELECT LOOPCOUNT,
	HN_RID,
	LEVEL_TYPE,
	COLOR_CODE_RID,
	SIZE_CODE_RID,
	STYLE_IN_PATH
FROM #TREE
WHERE LEVEL_TYPE >= @LEVEL_TYPE
ORDER BY LOOPCOUNT,
	HN_RID

IF (
		SELECT object_id('tempdb.dbo.#TREE')
		) > 0
	DROP TABLE #TREE
GO


