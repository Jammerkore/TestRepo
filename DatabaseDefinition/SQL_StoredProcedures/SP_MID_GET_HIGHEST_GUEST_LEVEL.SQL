--dv =============================================
--dv Modified date:	07/30/2015
--dv History:	    TT#1685-MD - RMatelic - Data Layer Request - SP_MID_GET_HIGHEST_GUEST_LEVEL needs to return the proper levels
--dv	                                    when a child node is added to an alternate hierarchy
--dv Modified date:	04/19/2016
--dv History:	    TT#4921 - JSmith - Alternate Hierarchy level not showing up on forecast view
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_GET_HIGHEST_GUEST_LEVEL] 
	@HN_RID INT,
	@GUEST_LEVEL INT OUTPUT,
	@HOME_PATH_LENGTH INT OUTPUT
AS
SET NOCOUNT ON

DECLARE @Loop INT,
	@LoopCount INT,
	@NextLoopCount INT
	,@HOME_PH_RID INT --TT#4584 - DOConnell - From Levels incorrect in alternate hierarchy

SET @LoopCount = 0

SELECT @HOME_PH_RID = HOME_PH_RID from HIERARCHY_NODE hn where hn.HN_RID = @HN_RID --TT#4584 - DOConnell - From Levels incorrect in alternate hierarchy

CREATE TABLE #TREE (
	LOOPCOUNT INT NOT NULL,
	PARENT_HN_RID INT NOT NULL,
	HOME_PH_RID INT,
	PH_TYPE INT,
	HN_RID INT NOT NULL,
	HOME_LEVEL INT NOT NULL
	)

-- insert the children of the node into the temp table
INSERT #TREE (
	LOOPCOUNT,
	PARENT_HN_RID,
	HOME_PH_RID,
	PH_TYPE,
	HN_RID,
	HOME_LEVEL
	)
SELECT @LoopCount AS LOOPCOUNT,
	@HN_RID AS PARENT_HN_RID,
	hn.HOME_PH_RID,
	ph.PH_TYPE,
	hnj.HN_RID,
	hn.HOME_LEVEL
FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID
JOIN [dbo].[PRODUCT_HIERARCHY] ph WITH (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
WHERE @HN_RID = hnj.PARENT_HN_RID
  AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

SET @Loop = @@ROWCOUNT
-- chase all paths until you get the main hierarchy (type 800000) or the lowest leaf
SET @NextLoopCount = @LoopCount + 1

WHILE @Loop > 0
BEGIN
	INSERT #TREE
	SELECT @NextLoopCount AS LOOPCOUNT,
		hnj.PARENT_HN_RID,
		hn.HOME_PH_RID,
		ph.PH_TYPE,
		hnj.HN_RID,
		hn.HOME_LEVEL
	FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
	JOIN [dbo].[HIERARCHY_NODE] hn WITH (NOLOCK) ON hn.HN_RID = hnj.HN_RID
	JOIN [dbo].[PRODUCT_HIERARCHY] ph WITH (NOLOCK) ON ph.PH_RID = hn.HOME_PH_RID
	JOIN #TREE t ON hnj.PARENT_HN_RID = t.HN_RID
	WHERE t.LOOPCOUNT = @LoopCount
		AND t.PH_TYPE <> 800000
		AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

	SET @Loop = @@ROWCOUNT
	SET @LoopCount = @LoopCount + 1
	SET @NextLoopCount = @LoopCount + 1
	--BEGIN TT#4584 - DOConnell - From Levels incorrect in alternate hierarchy
	--BEGIN TT#4921 - JSmith - Alternate Hierarchy level not showing up on forecast view
	IF (SELECT COUNT(*) FROM #TREE t where t.LOOPCOUNT = @LoopCount - 1 and t.HOME_PH_RID <> @HOME_PH_RID) > 0
	BEGIN
		SET @HOME_PATH_LENGTH = @LoopCount - 1
	END
	--END TT#4921 - JSmith - Alternate Hierarchy level not showing up on forecast view
	--END TT#4584 - DOConnell - From Levels incorrect in alternate hierarchy
END

SELECT @GUEST_LEVEL = min(HOME_LEVEL)
FROM #TREE
--BEGIN TT#4703 - JSmith - Test Forecasting Method Not Working - Parameters Dropped
WHERE HOME_PH_RID <> @HOME_PH_RID
--WHERE PH_TYPE <> 800000
--END TT#4703 - JSmith - Test Forecasting Method Not Working - Parameters Dropped

IF @GUEST_LEVEL IS NULL
BEGIN
	SET @GUEST_LEVEL = - 1
END

--BEGIN TT#4584 - DOConnell - From Levels incorrect in alternate hierarchy
--SELECT @HOME_PATH_LENGTH = max(LOOPCOUNT) + 1
--FROM #TREE
--WHERE PH_TYPE <> 800000
--END TT#4584 - DOConnell - From Levels incorrect in alternate hierarchy

--BEGIN TT#4921 - JSmith - Alternate Hierarchy level not showing up on forecast view
select @HOME_PATH_LENGTH = count(*) from 
(
select distinct HOME_LEVEL from #TREE where HOME_PH_RID = @HOME_PH_RID
) x
--BEGIN TT#4921 - JSmith - Alternate Hierarchy level not showing up on forecast view

IF @HOME_PATH_LENGTH IS NULL
BEGIN
	SET @HOME_PATH_LENGTH = 0
END

GO


