CREATE PROCEDURE [dbo].[SP_MID_SET_NODE_ACTIVITY] 
	@fromOffset INT = - 2,
	@toOffset INT = 0,
	@intransitOffset INT = 2,
	@forecastOffset INT = 0,
	@table INT = 0,
	@Return_Code INT = 0 OUTPUT,
	@debug BIT = 0
AS
DECLARE @fromTimeID INT,
	@toTimeID INT,
	@currentTimeID INT,
	@intransitTimeID INT,
	@forecastTimeID INT,
	@postingTimeID INT
DECLARE @rowSequence INT,
	@loop INT,
	@count INT,
	@RID INT,
	@c INT,
	@HN_TYPE INT,
	@HN_MOD INT
DECLARE @POSTING_DATE_YYYYDDD INT,
	@Tables INT
DECLARE @t TABLE (
	RID INT,
	OFFSET INT
	) -- table to hold active nodes
DECLARE @n TABLE (
	RID INT,
	HN_TYPE INT,
	HN_MOD INT
	)
DECLARE @sql NVARCHAR(4000),
	@params NVARCHAR(4000)

SET @Return_Code = 0

SELECT @postingTimeID = CURR_DATE_YYYYDDD,
	@POSTING_DATE_YYYYDDD = POSTING_DATE_YYYYDDD
FROM POSTING_DATE

SELECT @Tables = STORE_TABLE_COUNT
FROM SYSTEM_OPTIONS

SELECT TOP 1 @rowSequence = ROW_SEQUENCE,
	@currentTimeID = FIRST_DAY_OF_WEEK
FROM FISCAL_WEEKS
WHERE FIRST_DAY_OF_WEEK <= @postingTimeID
ORDER BY FIRST_DAY_OF_WEEK DESC

SELECT TOP 1 @fromTimeID = FIRST_DAY_OF_WEEK
FROM FISCAL_WEEKS
WHERE ROW_SEQUENCE = @rowSequence + @fromOffset
ORDER BY FIRST_DAY_OF_WEEK

SELECT TOP 1 @toTimeID = FIRST_DAY_OF_WEEK
FROM FISCAL_WEEKS
WHERE ROW_SEQUENCE = @rowSequence + @toOffset
ORDER BY FIRST_DAY_OF_WEEK

SELECT TOP 1 @intransitTimeID = FIRST_DAY_OF_WEEK
FROM FISCAL_WEEKS
WHERE ROW_SEQUENCE = @rowSequence + @intransitOffset
ORDER BY FIRST_DAY_OF_WEEK

IF @forecastOffset <> 0
BEGIN
	SELECT TOP 1 @forecastTimeID = FIRST_DAY_OF_WEEK
	FROM FISCAL_WEEKS
	WHERE ROW_SEQUENCE = @rowSequence + @forecastOffset
	ORDER BY FIRST_DAY_OF_WEEK
END

IF @debug <> 0
BEGIN
	PRINT 'Posting Time ID=' + convert(NVARCHAR, @postingTimeID)
	PRINT 'Current Time ID=' + convert(NVARCHAR, @currentTimeID)
	PRINT 'From Time ID=' + convert(NVARCHAR, @fromTimeID)
	PRINT 'To Time ID=' + convert(NVARCHAR, @toTimeID)
	PRINT 'Intransit Time ID=' + convert(NVARCHAR, @intransitTimeID)

	IF @forecastOffset <> 0
	BEGIN
		PRINT 'Forecast Time ID=' + convert(NVARCHAR, @forecastTimeID)
	END
END

-- get list of all nodes
INSERT INTO @n (
	RID,
	HN_TYPE,
	HN_MOD
	)
SELECT HN_RID,
	HN_TYPE,
	HN_RID % @Tables
FROM HIERARCHY_NODE
WHERE HN_RID % @Tables = @table

DECLARE C1 CURSOR FORWARD_ONLY
FOR
SELECT RID,
	HN_TYPE,
	HN_MOD
FROM @n

OPEN C1

FETCH NEXT
FROM C1
INTO @RID,
	@HN_TYPE,
	@HN_MOD

WHILE @@FETCH_STATUS = 0
BEGIN
	SET @count = 0

	-- check header
	IF @HN_TYPE = 800202 -- check styles only
	BEGIN
		IF EXISTS (
				SELECT TOP 1 *
				FROM VW_GET_HEADERS
				WHERE STYLE_HNRID = @RID
					AND ShippingComplete = 0
				)
		BEGIN
			SET @count = 1
		END
	END

	-- check intransit
	IF @count = 0
		AND (
			@HN_TYPE = 800202
			OR @HN_TYPE = 800203
			OR @HN_TYPE = 800204
			) -- check style, color, size only
	BEGIN
		IF EXISTS (
				SELECT TOP 1 *
				FROM STORE_INTRANSIT WITH (NOLOCK)
				WHERE HN_RID = @RID
					AND TIME_ID < @intransitTimeID
				)
		BEGIN
			SET @count = 1
		END
	END

	-- check external intransit
	IF @count = 0
		AND (
			@HN_TYPE = 800202
			OR @HN_TYPE = 800203
			OR @HN_TYPE = 800204
			) -- check style, color, size only
	BEGIN
		IF EXISTS (
				SELECT TOP 1 *
				FROM STORE_EXTERNAL_INTRANSIT WITH (NOLOCK)
				WHERE HN_RID = @RID
					AND TIME_ID > @POSTING_DATE_YYYYDDD
					AND TIME_ID < @intransitTimeID
				)
		BEGIN
			SET @count = 1
		END
	END

	-- check chain weekly history
	IF @count = 0
	BEGIN
		IF EXISTS (
				SELECT TOP 1 *
				FROM CHAIN_HISTORY_WEEK WITH (NOLOCK)
				WHERE HN_RID = @RID
					AND TIME_ID BETWEEN @fromTimeID
						AND @toTimeID
				)
		BEGIN
			SET @count = 1
		END
	END

	-- check chain weekly history
	IF @count = 0
	BEGIN
		IF EXISTS (
				SELECT TOP 1 *
				FROM CHAIN_FORECAST_WEEK WITH (NOLOCK)
				WHERE HN_RID = @RID
					AND TIME_ID BETWEEN @currentTimeID
						AND @forecastTimeID
				)
		BEGIN
			SET @count = 1
		END
	END

	-- check store daily history
	IF @count = 0
	BEGIN
		SELECT @sql = N'if exists (select top 1 * from STORE_HISTORY_DAY' + cast(@HN_MOD AS VARCHAR) + ' with (nolock) ' + N'           where HN_RID = @RID ' + N'             and TIME_ID between @fromTimeID and @toTimeID) ' + N'begin ' + N'  set @count = 1 ' + N'end'

		SELECT @params = N'@RID int, ' + N'@fromTimeID int, ' + N'@toTimeID   int, ' + N'@count      int      OUTPUT'

		EXEC sp_executesql @sql,
			@params,
			@RID,
			@fromTimeID,
			@toTimeID,
			@count = @count OUTPUT
	END

	-- check store weekly history
	IF @count = 0
	BEGIN
		SELECT @sql = N'if exists (select top 1 * from STORE_HISTORY_WEEK' + cast(@HN_MOD AS VARCHAR) + ' with (nolock) ' + N'           where HN_RID = @RID ' + N'             and TIME_ID between @fromTimeID and @toTimeID) ' + N'begin ' + N'  set @count = 1 ' + N'end'

		SELECT @params = N'@RID int, ' + N'@fromTimeID int, ' + N'@toTimeID   int, ' + N'@count      int      OUTPUT'

		EXEC sp_executesql @sql,
			@params,
			@RID,
			@fromTimeID,
			@toTimeID,
			@count = @count OUTPUT
	END

	IF @forecastOffset <> 0
	BEGIN
		-- check store weekly forecast
		IF @count = 0
		BEGIN
			SELECT @sql = N'if exists (select top 1 * from STORE_FORECAST_WEEK' + cast(@HN_MOD AS VARCHAR) + ' with (nolock) ' + N'           where HN_RID = @RID ' + N'             and TIME_ID between @currentTimeID and @forecastTimeID) ' + N'begin ' + N'  set @count = 1 ' + N'end'

			SELECT @params = N'@RID int, ' + N'@currentTimeID int, ' + N'@forecastTimeID   int, ' + N'@count      int      OUTPUT'

			EXEC sp_executesql @sql,
				@params,
				@RID,
				@currentTimeID,
				@forecastTimeID,
				@count = @count OUTPUT
		END
	END

	-- add to table if found record
	IF @count <> 0
	BEGIN
		INSERT INTO @t (
			RID,
			OFFSET
			)
		SELECT @RID,
			0
	END

	-- get next node
	FETCH NEXT
	FROM C1
	INTO @RID,
		@HN_TYPE,
		@HN_MOD
END

CLOSE C1

DEALLOCATE C1

-- add all ancestors
SELECT @count = COUNT(*)
FROM @t

SET @loop = 0

WHILE @count > 0
BEGIN
	INSERT @t (
		RID,
		OFFSET
		)
	SELECT hnj.PARENT_HN_RID,
		@loop + 1
	FROM [dbo].[HIER_NODE_JOIN] hnj WITH (NOLOCK)
	JOIN @t t ON hnj.HN_RID = t.RID
	WHERE t.OFFSET = @loop
	  AND hnj.NODE_DELETE_IND = '0' -- TT#3630 - JSmith - Delete My Hierarchy

	SET @count = @@ROWCOUNT
	SET @loop = @loop + 1
END

INSERT INTO VIRTUAL_LOCK (
	LOCK_TYPE,
	LOCK_ID
	)
VALUES (
	0,
	'NALock'
	)

--update HIERARCHY_NODE set ACTIVE_IND = '0' where HN_RID in (select RID from @n)
UPDATE HIERARCHY_NODE
SET INACTIVE_DATE = getdate()
WHERE INACTIVE_DATE IS NULL
	AND HN_RID IN (
		SELECT RID
		FROM @n
		)

UPDATE HIERARCHY_NODE
SET ACTIVE_IND = '1',
	INACTIVE_DATE = NULL
WHERE HN_RID IN (
		SELECT RID
		FROM @t
		)

DELETE
FROM VIRTUAL_LOCK
WHERE LOCK_TYPE = 0
	AND LOCK_ID = 'NALock'
GO


