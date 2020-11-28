--dv =============================================
--dv Modified date:	11/24/2015
--dv History:	    TT#1688-MD - RMatelic - Data Layer Request - SP_MID_BUILD_ALT_ROLLUP_ITEMS needs to build proper items for alternate hierarchy rollup
--dv                TT#1760-MD - RMatelic - Data Layer Request - Stored Procedure SP_MID_BUILD_ALT_ROLLUP_ITEMS is incorrect
--dv Modified date:	2/27/2017
--dv                TT#5485 - JSmith - Performance
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_BUILD_ALT_ROLLUP_ITEMS] 
	@PROCESS INT,
	@PH_RID INT,
	@HN_RID INT,
	@FV_RID INT,
	--@TIME_ID INT,
	@TIME_ID VARCHAR(MAX),
	@ITEM_TYPE INT,
	@FROM_LEVEL INT,
	@TO_LEVEL INT,
	@FIRST_DAY_OF_WEEK INT,
	@LAST_DAY_OF_WEEK INT,
	@FIRST_DAY_OF_NEXT_WEEK INT
AS
SET NOCOUNT ON

DECLARE @HN_TYPE INT,
	@Loop INT,
	@LoopCount INT,
	@NextLoopCount INT,
	@main_PH_RID INT

SELECT @main_PH_RID = PH_RID
FROM PRODUCT_HIERARCHY
WHERE PH_TYPE = 800000

CREATE TABLE #TEMP (
	tmpLOOPCOUNT INT NOT NULL,
	tmpPARENT_HN_RID INT NOT NULL,
	tmpHOME_PH_RID INT,
	tmpHOME_LEVEL INT NOT NULL,
	tmpPH_TYPE INT,
	tmpHN_RID INT NOT NULL,
	tmpLEVEL_TYPE INT
	)

CREATE TABLE #TIME_IDS (
	tmpTIME_ID INT NOT NULL
	)

INSERT INTO #TIME_IDS SELECT * FROM dbo.UDF_MID_SPLIT_CSV_INT (@TIME_ID, ',')

-- insert the node into the temp table
SET @LoopCount = 0

INSERT #TEMP (
	tmpLOOPCOUNT,
	tmpPARENT_HN_RID,
	tmpHOME_PH_RID,
	tmpHOME_LEVEL,
	tmpPH_TYPE,
	tmpHN_RID
	)
SELECT @LoopCount AS tmpLOOPCOUNT,
	0 AS tmpPARENT_HN_RID,
	hn.HOME_PH_RID,
	hn.HOME_LEVEL,
	ph.PH_TYPE,
	hn.HN_RID
FROM HIERARCHY_NODE hn WITH (NOLOCK)
JOIN PRODUCT_HIERARCHY ph WITH (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
WHERE @HN_RID = hn.HN_RID

BEGIN TRANSACTION

UPDATE #TEMP
SET tmpLEVEL_TYPE = (
		SELECT phl.PHL_TYPE
		FROM [dbo].[PRODUCT_HIERARCHY_LEVELS] phl WITH (NOLOCK)
		WHERE #TEMP.tmpHOME_PH_RID = phl.PH_RID
			AND #TEMP.tmpHOME_LEVEL = phl.PHL_SEQUENCE
		)

COMMIT

SELECT @Loop = tmpHOME_LEVEL
FROM #TEMP
WHERE tmpHN_RID = @HN_RID

SET @Loop = @@ROWCOUNT
-- chase all paths 
SET @NextLoopCount = @LoopCount + 1

WHILE @Loop > 0
BEGIN
	INSERT #TEMP (
		tmpLOOPCOUNT,
		tmpPARENT_HN_RID,
		tmpHOME_PH_RID,
		tmpHOME_LEVEL,
		tmpPH_TYPE,
		tmpHN_RID
		)
	SELECT @NextLoopCount AS tmpLOOPCOUNT,
		hnj.PARENT_HN_RID,
		hn.HOME_PH_RID,
		hn.HOME_LEVEL,
		ph.PH_TYPE,
		hnj.HN_RID
	FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
	JOIN #TEMP t ON hnj.PARENT_HN_RID = t.tmpHN_RID
		AND hnj.PH_RID = t.tmpHOME_PH_RID
	JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID
	JOIN [dbo].[PRODUCT_HIERARCHY] ph WITH (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
	JOIN [dbo].[HIER_NODE_JOIN] parent WITH (NOLOCK) ON hn.HN_RID = parent.HN_RID
		AND parent.PH_RID = hn.HOME_PH_RID
	WHERE t.tmpLOOPCOUNT = @LoopCount
		AND (
			t.tmpHOME_PH_RID <> @main_PH_RID
			OR (
				t.tmpHOME_PH_RID = @main_PH_RID
				AND t.tmpHOME_LEVEL < @TO_LEVEL
				)
			)
		AND hnj.NODE_DELETE_IND = '0'  -- TT#3630 - JSmith - Delete My Hierarchy 
		AND COALESCE(hnj.VIRTUAL_IND, 0) = '0'  -- TT#3630 - JSmith - Delete My Hierarchy

	SET @Loop = @@ROWCOUNT
	-- update counters
	SET @LoopCount = @LoopCount + 1
	SET @NextLoopCount = @LoopCount + 1

	-- add level type
	BEGIN TRANSACTION

	UPDATE #TEMP
	SET tmpLEVEL_TYPE = (
			SELECT phl.PHL_TYPE
			FROM [dbo].[PRODUCT_HIERARCHY_LEVELS] phl WITH (NOLOCK)
			WHERE #TEMP.tmpLOOPCOUNT = @LoopCount
				AND #TEMP.tmpHOME_PH_RID = phl.PH_RID
				AND #TEMP.tmpHOME_LEVEL = phl.PHL_SEQUENCE
			)
	WHERE tmpLEVEL_TYPE IS NULL
		AND #TEMP.tmpLOOPCOUNT = @LoopCount

	COMMIT
END

-- remove all records not in hierarchy
DELETE
FROM #TEMP
WHERE tmpHOME_PH_RID = @main_PH_RID
	AND tmpHOME_LEVEL = @FROM_LEVEL

-- Begin TT#4587 - JSmith - Values Rolling over shortcut level - values overlaid
DELETE
FROM #TEMP
WHERE tmpHOME_PH_RID <> @PH_RID
-- End TT#4587 - JSmith - Values Rolling over shortcut level - values overlaid

-- check to see if there are any records to process
SELECT *
FROM #TEMP

IF @@ROWCOUNT = 0
	RETURN

SET NOCOUNT ON

DECLARE @STORE_TABLE_COUNT int;
SELECT @STORE_TABLE_COUNT=STORE_TABLE_COUNT FROM SYSTEM_OPTIONS

-- delete duplicate records
DELETE #TEMP
FROM #TEMP
INNER JOIN ROLLUP_ITEM ON ROLLUP_ITEM.PROCESS = @PROCESS
	AND ROLLUP_ITEM.HN_RID = tmpHN_RID
	AND ROLLUP_ITEM.TIME_ID = @TIME_ID
	AND ROLLUP_ITEM.ITEM_TYPE = @ITEM_TYPE
	AND ROLLUP_ITEM.FV_RID = @FV_RID

INSERT ROLLUP_ITEM (
	PROCESS,
	HN_RID,
	TIME_ID,
	FV_RID,
	ITEM_TYPE,
	PH_RID,
	HOME_LEVEL,
	FIRST_DAY_OF_WEEK,
	LAST_DAY_OF_WEEK,
	FIRST_DAY_OF_NEXT_WEEK,
	HN_MOD
	)
SELECT DISTINCT @PROCESS AS PROCESS,
	tmpHN_RID,
	--@TIME_ID AS TIME_ID,
	t.tmpTIME_ID AS TIME_ID,
	@FV_RID AS FV_RID,
	@ITEM_TYPE AS ITEM_TYPE,
	tmpHOME_PH_RID,
	tmpHOME_LEVEL,
	@FIRST_DAY_OF_WEEK AS FIRST_DAY_OF_WEEK,
	@LAST_DAY_OF_WEEK AS LAST_DAY_OF_WEEK,
	@FIRST_DAY_OF_NEXT_WEEK AS FIRST_DAY_OF_NEXT_WEEK,
	tmpHN_RID % @STORE_TABLE_COUNT AS HN_MOD
FROM #TEMP
CROSS JOIN #TIME_IDS t
-- Begin TT#4587 - JSmith - Values Rolling over shortcut level - values overlaid
--WHERE tmpHOME_LEVEL > @TO_LEVEL - 1 
-- Begin TT#4829 - DOConnell - Alternate Hierarchy Rollup Executed from Incorrect Level
--WHERE tmpHOME_LEVEL between @TO_LEVEL and @FROM_LEVEL - 1
WHERE tmpHOME_LEVEL > @TO_LEVEL - 1 and tmpHOME_PH_RID = @PH_RID
-- End TT#4829 - DOConnell - Alternate Hierarchy Rollup Executed from Incorrect Level
-- End TT#4587 - JSmith - Values Rolling over shortcut level - values overlaid
GO
