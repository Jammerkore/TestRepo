--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description: Utility stored procedure for client use to fix mistakes they make when they load bad data
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_DELETE_VARIABLES] 
	@Delete_Chain_Values BIT = 0,
	@Delete_Store_Daily_Values BIT = 0,
	@Delete_Store_Weekly_Values BIT = 0,
	@Delete_All_Variables BIT = 0,
	@Delete_Variables_List VARCHAR(250) = ' ',
	@Delete_All_Dates BIT = 0,
	@From_Fiscal_Week INT = 0,
	@To_Fiscal_Week INT = 0,
	@Version VARCHAR(50) = 'Actual',
	@Delete_All_Nodes BIT = 1,
	@Delete_Node_ID VARCHAR(50) = ' ',
	@Delete_From_Offset INT = 0,
	@Delete_To_Offset INT = 999,
	@Delete_All_Stores BIT = 1,
	@Delete_Stores_List VARCHAR(250) = ' ',
	@Chain_Records_Affected INT = 0 OUTPUT,
	@Store_Daily_Records_Affected INT = 0 OUTPUT,
	@Store_Weekly_Records_Affected INT = 0 OUTPUT,
	@Return_Code INT = 0 OUTPUT,
	@debug BIT = 0
AS
SET NOCOUNT ON

-- @Return_Code values
--  1 : Nothing selected to delete
--  2 : No variables to delete
--  3 : From fiscal week not found
--  4 : To fiscal week not found
--  5 : Version not found
--  6 : Node not found
--  7 : Store not found
DECLARE @FV_RID INT,
	@FromDateID INT,
	@ToDateID INT,
	@Actual_FV_RID INT,
	@HN_RID INT,
	@colName VARCHAR(50),
	@CMD NVARCHAR(4000),
	@delim CHAR(1),
	@slice VARCHAR(250),
	@colNameCMD VARCHAR(250),
	@textpos INT,
	@maxlen INT,
	@stoppos INT,
	@LoopCount INT,
	@NextLoopCount INT,
	@PrnLine NVARCHAR(4000),
	@storeID VARCHAR(50)

CREATE TABLE #t (
	LOOPCOUNT INT NOT NULL,
	PARENT_HN_RID INT NOT NULL,
	HOME_PH_RID INT,
	HN_RID INT NOT NULL,
	HOME_LEVEL INT NOT NULL
	)

CREATE TABLE #s (ST_RID INT)

SELECT @Chain_Records_Affected = 0

SELECT @Store_Daily_Records_Affected = 0

SELECT @Store_Weekly_Records_Affected = 0

IF @Delete_Chain_Values = 0
	AND @Delete_Store_Weekly_Values = 0
	AND @Delete_Store_Daily_Values = 0
BEGIN
	SET @Return_Code = 1

	RETURN @Return_Code
END

IF @Delete_All_Variables = 0
	AND @Delete_Variables_List IS NULL
BEGIN
	SET @Return_Code = 2

	RETURN @Return_Code
END

IF @Delete_All_Dates = 0
BEGIN
	SELECT FIRST_DAY_OF_WEEK
	FROM FISCAL_WEEKS(NOLOCK)
	WHERE FISCAL_WEEK = @From_Fiscal_Week

	IF @@ROWCOUNT = 0
	BEGIN
		SET @Return_Code = 3

		RETURN @Return_Code
	END

	SELECT FIRST_DAY_OF_WEEK
	FROM FISCAL_WEEKS(NOLOCK)
	WHERE FISCAL_WEEK = @To_Fiscal_Week

	IF @@ROWCOUNT = 0
	BEGIN
		SET @Return_Code = 4

		RETURN @Return_Code
	END
END

SELECT @Actual_FV_RID = 1

SELECT @delim = '|'

SELECT @FV_RID = FV_RID
FROM FORECAST_VERSION fv(NOLOCK)
WHERE fv.DESCRIPTION = @Version

IF @@ROWCOUNT = 0
BEGIN
	SET @Return_Code = 5

	RETURN @Return_Code
END

SET @Return_Code = 0

-- get date IDs based of fiscal weeks
IF @Delete_All_Dates <> 0
BEGIN
	SELECT @FromDateID = 0

	SELECT @ToDateID = 999999
END
ELSE
BEGIN
	SELECT @FromDateID = FIRST_DAY_OF_WEEK
	FROM FISCAL_WEEKS(NOLOCK)
	WHERE FISCAL_WEEK = @From_Fiscal_Week

	SELECT @ToDateID = LAST_DAY_OF_WEEK
	FROM FISCAL_WEEKS(NOLOCK)
	WHERE FISCAL_WEEK = @To_Fiscal_Week
END

IF @Delete_All_Stores = 1
BEGIN
	INSERT INTO #s
	SELECT ST_RID
	FROM STORES
END
ELSE
BEGIN
	SELECT @textpos = 1,
		@maxlen = 250,
		@stoppos = 1

	WHILE @stoppos > 0
	BEGIN
		SELECT @maxlen = datalength(@Delete_Stores_List) - @textpos + 1

		SELECT @slice = substring(@Delete_Stores_List, @textpos, @maxlen)

		SELECT @stoppos = charindex(@delim, @slice)

		IF @stoppos > 0
			SELECT @storeID = left(@slice, @stoppos - 1)
		ELSE
			SELECT @storeID = @slice

		INSERT INTO #s
		SELECT ST_RID
		FROM STORES
		WHERE ST_ID = @storeID

		IF @@ROWCOUNT = 0
		BEGIN
			SET @Return_Code = 7

			RETURN @Return_Code
		END

		SELECT @textpos = @textpos + @stoppos -- On the other side of the delimeter.
	END
END

IF @debug <> 0
	SELECT st.ST_ID
	FROM #s s
	JOIN STORES st ON s.ST_RID = st.ST_RID

IF @Delete_All_Nodes = 0
BEGIN
	SELECT @HN_RID = HN_RID
	FROM BASE_NODE(NOLOCK)
	WHERE BN_ID = @Delete_Node_ID

	IF @@ROWCOUNT = 0
	BEGIN
		SET @Return_Code = 6

		RETURN @Return_Code
	END

	-- insert the node into the temp table
	SET @LoopCount = 0

	INSERT #t (
		LOOPCOUNT,
		PARENT_HN_RID,
		HOME_PH_RID,
		HN_RID,
		HOME_LEVEL
		)
	SELECT @LoopCount AS LOOPCOUNT,
		0 AS PARENT_HN_RID,
		hn.HOME_PH_RID,
		hn.HN_RID,
		hn.HOME_LEVEL
	FROM [dbo].[HIERARCHY_NODE] hn(NOLOCK)
	WHERE hn.HN_RID = @HN_RID

	-- chase all paths
	SET @NextLoopCount = @LoopCount + 1

	WHILE @NextLoopCount <= @Delete_To_Offset
	BEGIN
		INSERT #t
		SELECT @NextLoopCount AS LOOPCOUNT,
			hnj.PARENT_HN_RID,
			hn.HOME_PH_RID,
			hnj.HN_RID,
			hn.HOME_LEVEL
		FROM [dbo].[HIER_NODE_JOIN] hnj(NOLOCK)
		JOIN #t t ON hnj.PARENT_HN_RID = t.HN_RID
			AND hnj.PH_RID = t.HOME_PH_RID
		JOIN [dbo].[HIERARCHY_NODE] hn(NOLOCK) ON hn.HN_RID = hnj.HN_RID
		JOIN [dbo].[HIER_NODE_JOIN] parent(NOLOCK) ON hn.HN_RID = parent.HN_RID
			AND parent.PH_RID = hn.HOME_PH_RID
		WHERE t.LOOPCOUNT = @LoopCount

		-- update counters
		SET @LoopCount = @LoopCount + 1
		SET @NextLoopCount = @LoopCount + 1
	END

	-- apply offsets
	DELETE
	FROM #t
	WHERE LOOPCOUNT < @Delete_From_Offset
END

IF @Delete_Chain_Values <> 0
BEGIN
	IF @FV_RID = @Actual_FV_RID -- actuals
	BEGIN
		IF @Delete_All_Variables <> 0 -- deleting all variables
		BEGIN
			IF @Delete_All_Dates <> 0 -- deleting all dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					SELECT @Chain_Records_Affected = count(*)
					FROM VW_CHAIN_HISTORY_WEEK

					IF @debug <> 0
						PRINT 'CHAIN_HISTORY_WEEK will be truncated'
					ELSE
						TRUNCATE TABLE CHAIN_HISTORY_WEEK
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_CHAIN_HISTORY_WEEK
						WHERE HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
					ELSE
						DELETE
						FROM VW_CHAIN_HISTORY_WEEK
						WHERE HN_RID IN (
								SELECT HN_RID
								FROM #t
								)

					SELECT @Chain_Records_Affected = @@rowcount
				END
			END
			ELSE -- delete range of dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_CHAIN_HISTORY_WEEK
						WHERE TIME_ID BETWEEN @FromDateID
								AND @ToDateID
					ELSE
						DELETE
						FROM VW_CHAIN_HISTORY_WEEK
						WHERE TIME_ID BETWEEN @FromDateID
								AND @ToDateID

					SELECT @Chain_Records_Affected = @@rowcount
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_CHAIN_HISTORY_WEEK
						WHERE TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
					ELSE
						DELETE
						FROM VW_CHAIN_HISTORY_WEEK
						WHERE TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)

					SELECT @Chain_Records_Affected = @@rowcount
				END
			END
		END
		ELSE -- delete variable list
		BEGIN
			SELECT @textpos = 1,
				@maxlen = 250,
				@stoppos = 1,
				@colNameCMD = ' '

			WHILE @stoppos > 0
			BEGIN
				SELECT @maxlen = datalength(@Delete_Variables_List) - @textpos + 1

				SELECT @slice = substring(@Delete_Variables_List, @textpos, @maxlen)

				SELECT @stoppos = charindex(@delim, @slice)

				IF @stoppos > 0
					SELECT @colName = left(@slice, @stoppos - 1)
				ELSE
					SELECT @colName = @slice

				IF @debug <> 0
					SET @colNameCMD = @colNameCMD + @colName
				ELSE
					SET @colNameCMD = @colNameCMD + ' ' + @colName + ' = null'

				SELECT @textpos = @textpos + @stoppos -- On the other side of the delimeter.

				IF @stoppos <> 0
					SET @colNameCMD = @colNameCMD + ', '
			END

			IF @Delete_All_Dates <> 0 -- deleting all dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, TIME_ID, ' + @colNameCMD + ' from VW_CHAIN_HISTORY_WEEK'
					ELSE
						SET @CMD = 'update VW_CHAIN_HISTORY_WEEK set ' + @colNameCMD
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, TIME_ID, ' + @colNameCMD + ' from VW_CHAIN_HISTORY_WEEK where HN_RID in (select HN_RID from #t)'
					ELSE
						SET @CMD = 'update VW_CHAIN_HISTORY_WEEK set ' + @colNameCMD + ' where HN_RID in (select HN_RID from #t)'
				END
			END
			ELSE -- delete range of dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, TIME_ID, ' + @colNameCMD + ' from VW_CHAIN_HISTORY_WEEK where TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999')
					ELSE
						SET @CMD = 'update VW_CHAIN_HISTORY_WEEK set ' + @colNameCMD + ' where TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999')
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, TIME_ID, ' + @colNameCMD + ' from VW_CHAIN_HISTORY_WEEK where TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and HN_RID in (select HN_RID from #t)'
					ELSE
						SET @CMD = 'update VW_CHAIN_HISTORY_WEEK set ' + @colNameCMD + ' where TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and HN_RID in (select HN_RID from #t)'
				END
			END

			IF @debug <> 0
				PRINT @CMD

			EXEC (@CMD)

			SELECT @Chain_Records_Affected = @@rowcount
		END
	END
	ELSE -- forecasts
	BEGIN
		IF @Delete_All_Variables <> 0 -- deleting all variables
		BEGIN
			IF @Delete_All_Dates <> 0 -- deleting all dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_CHAIN_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
					ELSE
						DELETE
						FROM VW_CHAIN_FORECAST_WEEK
						WHERE FV_RID = @FV_RID

					SELECT @Chain_Records_Affected = @@rowcount
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_CHAIN_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
					ELSE
						DELETE
						FROM VW_CHAIN_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)

					SELECT @Chain_Records_Affected = @@rowcount
				END
			END
			ELSE -- delete range of dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_CHAIN_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND TIME_ID BETWEEN @FromDateID
								AND @ToDateID
					ELSE
						DELETE
						FROM VW_CHAIN_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND TIME_ID BETWEEN @FromDateID
								AND @ToDateID

					SELECT @Chain_Records_Affected = @@rowcount
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_CHAIN_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
					ELSE
						DELETE
						FROM VW_CHAIN_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)

					SELECT @Chain_Records_Affected = @@rowcount
				END
			END
		END
		ELSE -- delete variable list
		BEGIN
			SELECT @textpos = 1,
				@maxlen = 250,
				@stoppos = 1,
				@colNameCMD = ' '

			WHILE @stoppos > 0
			BEGIN
				SELECT @maxlen = datalength(@Delete_Variables_List) - @textpos + 1

				SELECT @slice = substring(@Delete_Variables_List, @textpos, @maxlen)

				SELECT @stoppos = charindex(@delim, @slice)

				IF @stoppos > 0
					SELECT @colName = left(@slice, @stoppos - 1)
				ELSE
					SELECT @colName = @slice

				IF @debug <> 0
					SET @colNameCMD = @colNameCMD + @colName
				ELSE
					SET @colNameCMD = @colNameCMD + ' ' + @colName + ' = null'

				SELECT @textpos = @textpos + @stoppos -- On the other side of the delimeter.

				IF @stoppos <> 0
					SET @colNameCMD = @colNameCMD + ', '
			END

			IF @Delete_All_Dates <> 0 -- deleting all dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, TIME_ID, ' + @colNameCMD + ' from VW_CHAIN_FORECAST_WEEK where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>')
					ELSE
						SET @CMD = 'update VW_CHAIN_FORECAST_WEEK set ' + @colNameCMD + ' where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>')
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, TIME_ID, ' + @colNameCMD + ' from VW_CHAIN_FORECAST_WEEK where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and HN_RID in (select HN_RID from #t)'
					ELSE
						SET @CMD = 'update VW_CHAIN_FORECAST_WEEK set ' + @colNameCMD + ' where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and HN_RID in (select HN_RID from #t)'
				END
			END
			ELSE -- delete range of dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, TIME_ID, ' + @colNameCMD + ' from VW_CHAIN_FORECAST_WEEK where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999')
					ELSE
						SET @CMD = 'update VW_CHAIN_FORECAST_WEEK set ' + @colNameCMD + ' where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999')
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, TIME_ID, ' + @colNameCMD + ' from VW_CHAIN_FORECAST_WEEK where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and HN_RID in (select HN_RID from #t)'
					ELSE
						SET @CMD = 'update VW_CHAIN_FORECAST_WEEK set ' + @colNameCMD + ' where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and HN_RID in (select HN_RID from #t)'
				END
			END

			IF @debug <> 0
				PRINT @CMD

			EXEC (@CMD)

			SELECT @Chain_Records_Affected = @@rowcount
		END
	END
END

IF @Delete_Store_Daily_Values <> 0
BEGIN
	IF @FV_RID = @Actual_FV_RID -- actuals
	BEGIN
		IF @Delete_All_Variables <> 0 -- deleting all variables
		BEGIN
			IF @Delete_All_Dates <> 0 -- deleting all dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @Delete_All_Stores <> 0 -- deleting all stores
					BEGIN
						SELECT @Store_Daily_Records_Affected = count(*)
						FROM VW_STORE_HISTORY_DAY

						IF @debug <> 0
							PRINT 'STORE_HISTORY_DAY will be truncated'
						ELSE
						BEGIN
							TRUNCATE TABLE STORE_HISTORY_DAY0

							TRUNCATE TABLE STORE_HISTORY_DAY1

							TRUNCATE TABLE STORE_HISTORY_DAY2

							TRUNCATE TABLE STORE_HISTORY_DAY3

							TRUNCATE TABLE STORE_HISTORY_DAY4

							TRUNCATE TABLE STORE_HISTORY_DAY5

							TRUNCATE TABLE STORE_HISTORY_DAY6

							TRUNCATE TABLE STORE_HISTORY_DAY7

							TRUNCATE TABLE STORE_HISTORY_DAY8

							TRUNCATE TABLE STORE_HISTORY_DAY9
						END
					END
					ELSE
					BEGIN
						IF @debug <> 0
							SELECT *
							FROM VW_STORE_HISTORY_DAY
							WHERE ST_RID IN (
									SELECT ST_RID
									FROM #s
									)
						ELSE
							DELETE
							FROM VW_STORE_HISTORY_DAY
							WHERE ST_RID IN (
									SELECT ST_RID
									FROM #s
									)

						SELECT @Store_Daily_Records_Affected = @@rowcount
					END
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_STORE_HISTORY_DAY
						WHERE HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)
					ELSE
						DELETE
						FROM VW_STORE_HISTORY_DAY
						WHERE HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)

					SELECT @Store_Daily_Records_Affected = @@rowcount
				END
			END
			ELSE -- delete range of dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_STORE_HISTORY_DAY
						WHERE TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)
					ELSE
						DELETE
						FROM VW_STORE_HISTORY_DAY
						WHERE TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)

					SELECT @Store_Daily_Records_Affected = @@rowcount
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_STORE_HISTORY_DAY
						WHERE TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)
					ELSE
						DELETE
						FROM VW_STORE_HISTORY_DAY
						WHERE TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)

					SELECT @Store_Daily_Records_Affected = @@rowcount
				END
			END
		END
		ELSE -- delete variable list
		BEGIN
			SELECT @textpos = 1,
				@maxlen = 250,
				@stoppos = 1,
				@colNameCMD = ' '

			WHILE @stoppos > 0
			BEGIN
				SELECT @maxlen = datalength(@Delete_Variables_List) - @textpos + 1

				SELECT @slice = substring(@Delete_Variables_List, @textpos, @maxlen)

				SELECT @stoppos = charindex(@delim, @slice)

				IF @stoppos > 0
					SELECT @colName = left(@slice, @stoppos - 1)
				ELSE
					SELECT @colName = @slice

				IF @debug <> 0
					SET @colNameCMD = @colNameCMD + @colName
				ELSE
					SET @colNameCMD = @colNameCMD + ' ' + @colName + ' = null'

				SELECT @textpos = @textpos + @stoppos -- On the other side of the delimeter.

				IF @stoppos <> 0
					SET @colNameCMD = @colNameCMD + ', '
			END

			IF @Delete_All_Dates <> 0 -- deleting all dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, ST_RID, TIME_ID, ' + @colNameCMD + ' from VW_STORE_HISTORY_DAY where ST_RID in (select ST_RID from #s)'
					ELSE
						SET @CMD = 'update VW_STORE_HISTORY_DAY set ' + @colNameCMD + ' where ST_RID in (select ST_RID from #s)'
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, ST_RID, TIME_ID, ' + @colNameCMD + ' from VW_STORE_HISTORY_DAY where HN_RID in (select HN_RID from #t) and ST_RID in (select ST_RID from #s)'
					ELSE
						SET @CMD = 'update VW_STORE_HISTORY_DAY set ' + @colNameCMD + ' where HN_RID in (select HN_RID from #t) and ST_RID in (select ST_RID from #s)'
				END
			END
			ELSE -- delete range of dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, ST_RID, TIME_ID, ' + @colNameCMD + ' from VW_STORE_HISTORY_DAY where TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and ST_RID in (select ST_RID from #s)'
					ELSE
						SET @CMD = 'update VW_STORE_HISTORY_DAY set ' + @colNameCMD + ' where TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and ST_RID in (select ST_RID from #s)'
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, ST_RID, TIME_ID, ' + @colNameCMD + ' from VW_STORE_HISTORY_DAY where TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and HN_RID in (select HN_RID from #t) and ST_RID in (select ST_RID from #s)'
					ELSE
						SET @CMD = 'update VW_STORE_HISTORY_DAY set ' + @colNameCMD + ' where TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and HN_RID in (select HN_RID from #t) and ST_RID in (select ST_RID from #s)'
				END
			END

			IF @debug <> 0
				PRINT @CMD

			EXEC (@CMD)

			SELECT @Store_Daily_Records_Affected = @@rowcount
		END
	END
END

IF @Delete_Store_Weekly_Values <> 0
BEGIN
	IF @FV_RID = @Actual_FV_RID -- actuals
	BEGIN
		IF @Delete_All_Variables <> 0 -- deleting all variables
		BEGIN
			IF @Delete_All_Dates <> 0 -- deleting all dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @Delete_All_Stores <> 0 -- deleting all stores
					BEGIN
						SELECT @Store_Weekly_Records_Affected = count(*)
						FROM VW_STORE_HISTORY_WEEK

						IF @debug <> 0
							PRINT 'STORE_HISTORY_WEEK will be truncated'
						ELSE
						BEGIN
							TRUNCATE TABLE STORE_HISTORY_WEEK0

							TRUNCATE TABLE STORE_HISTORY_WEEK1

							TRUNCATE TABLE STORE_HISTORY_WEEK2

							TRUNCATE TABLE STORE_HISTORY_WEEK3

							TRUNCATE TABLE STORE_HISTORY_WEEK4

							TRUNCATE TABLE STORE_HISTORY_WEEK5

							TRUNCATE TABLE STORE_HISTORY_WEEK6

							TRUNCATE TABLE STORE_HISTORY_WEEK7

							TRUNCATE TABLE STORE_HISTORY_WEEK8

							TRUNCATE TABLE STORE_HISTORY_WEEK9
						END
					END
					ELSE
					BEGIN
						IF @debug <> 0
							SELECT *
							FROM VW_STORE_HISTORY_WEEK
							WHERE ST_RID IN (
									SELECT ST_RID
									FROM #s
									)
						ELSE
							DELETE
							FROM VW_STORE_HISTORY_WEEK
							WHERE ST_RID IN (
									SELECT ST_RID
									FROM #s
									)

						SELECT @Store_Weekly_Records_Affected = @@rowcount
					END
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_STORE_HISTORY_WEEK
						WHERE HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)
					ELSE
						DELETE
						FROM VW_STORE_HISTORY_WEEK
						WHERE HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)

					SELECT @Store_Weekly_Records_Affected = @@rowcount
				END
			END
			ELSE -- delete range of dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_STORE_HISTORY_WEEK
						WHERE TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)
					ELSE
						DELETE
						FROM VW_STORE_HISTORY_WEEK
						WHERE TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)

					SELECT @Store_Weekly_Records_Affected = @@rowcount
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_STORE_HISTORY_WEEK
						WHERE TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)
					ELSE
						DELETE
						FROM VW_STORE_HISTORY_WEEK
						WHERE TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)

					SELECT @Store_Weekly_Records_Affected = @@rowcount
				END
			END
		END
		ELSE -- delete variable list
		BEGIN
			SELECT @textpos = 1,
				@maxlen = 250,
				@stoppos = 1,
				@colNameCMD = ' '

			WHILE @stoppos > 0
			BEGIN
				SELECT @maxlen = datalength(@Delete_Variables_List) - @textpos + 1

				SELECT @slice = substring(@Delete_Variables_List, @textpos, @maxlen)

				SELECT @stoppos = charindex(@delim, @slice)

				IF @stoppos > 0
					SELECT @colName = left(@slice, @stoppos - 1)
				ELSE
					SELECT @colName = @slice

				IF @debug <> 0
					SET @colNameCMD = @colNameCMD + @colName
				ELSE
					SET @colNameCMD = @colNameCMD + ' ' + @colName + ' = null'

				SELECT @textpos = @textpos + @stoppos -- On the other side of the delimeter.

				IF @stoppos <> 0
					SET @colNameCMD = @colNameCMD + ', '
			END

			IF @Delete_All_Dates <> 0 -- deleting all dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, ST_RID, TIME_ID, ' + @colNameCMD + ' from VW_STORE_HISTORY_WEEK where ST_RID in (select ST_RID from #s)'
					ELSE
						SET @CMD = 'update VW_STORE_HISTORY_WEEK set ' + @colNameCMD + ' where ST_RID in (select ST_RID from #s)'
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, ST_RID, TIME_ID, ' + @colNameCMD + ' from VW_STORE_HISTORY_WEEK where HN_RID in (select HN_RID from #t) and ST_RID in (select ST_RID from #s)'
					ELSE
						SET @CMD = 'update VW_STORE_HISTORY_WEEK set ' + @colNameCMD + ' where HN_RID in (select HN_RID from #t) and ST_RID in (select ST_RID from #s)'
				END
			END
			ELSE -- delete range of dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, ST_RID, TIME_ID, ' + @colNameCMD + ' from VW_STORE_HISTORY_WEEK where TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and ST_RID in (select ST_RID from #s)'
					ELSE
						SET @CMD = 'update VW_STORE_HISTORY_WEEK set ' + @colNameCMD + ' where TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and ST_RID in (select ST_RID from #s)'
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, ST_RID, TIME_ID, ' + @colNameCMD + ' from VW_STORE_HISTORY_WEEK where TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and HN_RID in (select HN_RID from #t) and ST_RID in (select ST_RID from #s)'
					ELSE
						SET @CMD = 'update VW_STORE_HISTORY_WEEK set ' + @colNameCMD + ' where TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and HN_RID in (select HN_RID from #t) and ST_RID in (select ST_RID from #s)'
				END
			END

			IF @debug <> 0
				PRINT @CMD

			EXEC (@CMD)

			SELECT @Store_Weekly_Records_Affected = @@rowcount
		END
	END
	ELSE -- forecasts
	BEGIN
		IF @Delete_All_Variables <> 0 -- deleting all variables
		BEGIN
			IF @Delete_All_Dates <> 0 -- deleting all dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_STORE_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)
					ELSE
						DELETE
						FROM VW_STORE_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)

					SELECT @Store_Weekly_Records_Affected = @@rowcount
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_STORE_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)
					ELSE
						DELETE
						FROM VW_STORE_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)

					SELECT @Store_Weekly_Records_Affected = @@rowcount
				END
			END
			ELSE -- delete range of dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_STORE_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)
					ELSE
						DELETE
						FROM VW_STORE_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)

					SELECT @Store_Weekly_Records_Affected = @@rowcount
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SELECT *
						FROM VW_STORE_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)
					ELSE
						DELETE
						FROM VW_STORE_FORECAST_WEEK
						WHERE FV_RID = @FV_RID
							AND TIME_ID BETWEEN @FromDateID
								AND @ToDateID
							AND HN_RID IN (
								SELECT HN_RID
								FROM #t
								)
							AND ST_RID IN (
								SELECT ST_RID
								FROM #s
								)

					SELECT @Store_Weekly_Records_Affected = @@rowcount
				END
			END
		END
		ELSE -- delete variable list
		BEGIN
			SELECT @textpos = 1,
				@maxlen = 250,
				@stoppos = 1,
				@colNameCMD = ' '

			WHILE @stoppos > 0
			BEGIN
				SELECT @maxlen = datalength(@Delete_Variables_List) - @textpos + 1

				SELECT @slice = substring(@Delete_Variables_List, @textpos, @maxlen)

				SELECT @stoppos = charindex(@delim, @slice)

				IF @stoppos > 0
					SELECT @colName = left(@slice, @stoppos - 1)
				ELSE
					SELECT @colName = @slice

				IF @debug <> 0
					SET @colNameCMD = @colNameCMD + @colName
				ELSE
					SET @colNameCMD = @colNameCMD + ' ' + @colName + ' = null'

				SELECT @textpos = @textpos + @stoppos -- On the other side of the delimeter.

				IF @stoppos <> 0
					SET @colNameCMD = @colNameCMD + ', '
			END

			IF @Delete_All_Dates <> 0 -- deleting all dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, ST_RID, TIME_ID, ' + @colNameCMD + ' from VW_STORE_FORECAST_WEEK where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and ST_RID in (select ST_RID from #s)'
					ELSE
						SET @CMD = 'update VW_STORE_FORECAST_WEEK set ' + @colNameCMD + ' where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and ST_RID in (select ST_RID from #s)'
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, ST_RID, TIME_ID, ' + @colNameCMD + ' from VW_STORE_FORECAST_WEEK where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and HN_RID in (select HN_RID from #t) and ST_RID in (select ST_RID from #s)'
					ELSE
						SET @CMD = 'update VW_STORE_FORECAST_WEEK set ' + @colNameCMD + ' where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and HN_RID in (select HN_RID from #t) and ST_RID in (select ST_RID from #s)'
				END
			END
			ELSE -- delete range of dates
			BEGIN
				IF @Delete_All_Nodes <> 0 -- deleting all nodes
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, ST_RID, TIME_ID, ' + @colNameCMD + ' from VW_STORE_FORECAST_WEEK where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and ST_RID in (select ST_RID from #s)'
					ELSE
						SET @CMD = 'update VW_STORE_FORECAST_WEEK set ' + @colNameCMD + ' where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and ST_RID in (select ST_RID from #s)'
				END
				ELSE
				BEGIN
					IF @debug <> 0
						SET @CMD = 'select HN_RID, ST_RID, TIME_ID, ' + @colNameCMD + ' from VW_STORE_FORECAST_WEEK where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and HN_RID in (select HN_RID from #t) and ST_RID in (select ST_RID from #s)'
					ELSE
						SET @CMD = 'update VW_STORE_FORECAST_WEEK set ' + @colNameCMD + ' where FV_RID = ' + isnull(CONVERT(NVARCHAR, @FV_RID), '<NULL>') + ' and TIME_ID between ' + isnull(CONVERT(NVARCHAR, @FromDateID), '0000000') + ' and ' + + isnull(CONVERT(NVARCHAR, @ToDateID), '9999999') + ' and HN_RID in (select HN_RID from #t) and ST_RID in (select ST_RID from #s)'
				END
			END

			IF @debug <> 0
				PRINT @CMD

			EXEC (@CMD)

			SELECT @Store_Weekly_Records_Affected = @@rowcount
		END
	END
END

IF (
		SELECT object_id('tempdb.dbo.#t')
		) > 0
	DROP TABLE #t

IF (
		SELECT object_id('tempdb.dbo.#s')
		) > 0
	DROP TABLE #s

RETURN @Return_Code
GO


