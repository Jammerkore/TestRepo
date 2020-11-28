--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description:	Reads daily percentages for nodes for the store
--dv History: TT#1145 - JSmith - Shows inactive stores
--dv =============================================
CREATE PROCEDURE [dbo].[SP_GET_DAILYPERCENTAGES_REPORT_FOR_STORE] 
	@SELECTED_NODE_RID INT,
	@SELECTED_NODE_LEVEL INT,
	@LOWER_LEVEL INT,
	@STORE_ID VARCHAR(255)
AS
BEGIN
	DECLARE @LEVEL AS INT

	SELECT @LEVEL = @LOWER_LEVEL - 1

	CREATE TABLE #TEMP (
		PARENT_HN_RID INT,
		HN_RID INT,
		BN_ID CHAR(356) COLLATE Latin1_General_CS_AS,
		BN_NAME CHAR(50) COLLATE Latin1_General_CS_AS,
		BN_DESCRIPTION CHAR(250) COLLATE Latin1_General_CS_AS,
		HOME_LEVEL INT
		)

	INSERT #TEMP (
		PARENT_HN_RID,
		HN_RID,
		BN_ID,
		BN_NAME,
		BN_DESCRIPTION,
		HOME_LEVEL
		)
	EXEC SP_GET_ALL_DESCENDANTS_BY_LEVEL_REPORT @SELECTED_NODE_RID, @LEVEL

	SELECT 'DailyPercentages',
		hn.PARENT_HN_RID,
		dpd.HN_RID,
		'[Defaults]' AS [DateRange],
		dbo.UDF_MID_GET_STORE_TEXT (st.ST_RID) AS [STORE_TEXT],
		st.ST_ID,
		st.STORE_NAME,
		hn.BN_ID,
		hn.BN_NAME,
		-- Begin TT#5382 - JSmith - Product descriptor not displayed when the Store Eligibility is set at the color level
		dbo.UDF_MID_GET_NODE_DISPLAY (dpd.HN_RID) AS [DISPLAY_TEXT],
		--CASE 
		--	WHEN phl.PHL_DISPLAY_OPTION_ID = 800701
		--		THEN RTRIM(hn.BN_ID)
		--	WHEN phl.PHL_DISPLAY_OPTION_ID = 800702
		--		THEN RTRIM(hn.BN_DESCRIPTION)
		--	WHEN phl.PHL_DISPLAY_OPTION_ID = 800703
		--		THEN RTRIM(hn.BN_ID) + ' [' + RTRIM(hn.BN_NAME) + ']'
		--	WHEN phl.PHL_DISPLAY_OPTION_ID = 800704
		--		THEN RTRIM(hn.BN_ID) + ' [' + RTRIM(hn.BN_DESCRIPTION) + ']'
		--	WHEN phl.PHL_DISPLAY_OPTION_ID = 800705
		--		THEN RTRIM(hn.BN_NAME) + ' [' + RTRIM(hn.BN_DESCRIPTION) + ']'
		--	WHEN phl.PHL_DISPLAY_OPTION_ID = 800706
		--		THEN RTRIM(hn.BN_ID) + ' [' + RTRIM(hn.BN_NAME) + ']' + ' [' + RTRIM(hn.BN_DESCRIPTION) + ']'
		--	ELSE RTRIM(hn.BN_ID) + ' [' + RTRIM(hn.BN_NAME) + ']'
		--	END AS [DISPLAY_TEXT],
		-- End TT#5382 - JSmith - Product descriptor not displayed when the Store Eligibility is set at the color level
		dpd.DAY1 + dpd.DAY2 + dpd.DAY3 + dpd.DAY4 + dpd.DAY5 + dpd.DAY6 + dpd.DAY7 AS TOTALPCT,
		dpd.DAY1,
		dpd.DAY2,
		dpd.DAY3,
		dpd.DAY4,
		dpd.DAY5,
		dpd.DAY6,
		dpd.DAY7
	FROM DAILY_PERCENTAGES_DEFAULTS dpd
	INNER JOIN HIERARCHY_NODE hen ON hen.HN_RID = dpd.HN_RID
	LEFT OUTER JOIN PRODUCT_HIERARCHY_LEVELS phl ON phl.PHL_SEQUENCE = hen.HOME_LEVEL
	-- Begin TT#5382 - JSmith - Product descriptor not displayed when the Store Eligibility is set at the color level
	--INNER JOIN BASE_NODE bn ON dpd.HN_RID = bn.HN_RID
	LEFT OUTER JOIN BASE_NODE bn ON dpd.HN_RID = bn.HN_RID
	-- End TT#5382 - JSmith - Product descriptor not displayed when the Store Eligibility is set at the color level
		AND (
			DAY1 > 0
			OR DAY2 > 0
			OR DAY3 > 0
			OR DAY4 > 0
			OR DAY5 > 0
			OR DAY6 > 0
			OR DAY7 > 0
			)
	INNER JOIN STORES st ON st.ST_RID = dpd.ST_RID
		AND st.ST_ID = @STORE_ID
		AND st.ACTIVE_IND = '1'
	INNER JOIN (
		SELECT PARENT_HN_RID,
			HN_RID,
			BN_ID,
			BN_NAME,
			BN_DESCRIPTION
		FROM #TEMP
		) hn ON hn.HN_RID = dpd.HN_RID

	UNION

	SELECT 'DailyPercentages',
		hn.PARENT_HN_RID,
		dp.HN_RID,
		CASE 
			WHEN cdr.CDR_RID > 1
				AND (
					CDR_RANGE_TYPE_ID = 800858
					OR CDR_RANGE_TYPE_ID = 800859
					)
				THEN ap1.TEXT_VALUE + ' ' + ap2.TEXT_VALUE + '(s) ' + CONVERT(VARCHAR(3), cdr.CDR_START) + '-' + CONVERT(VARCHAR(3), cdr.CDR_END)
			WHEN cdr.CDR_RID > 1
				AND CDR_RANGE_TYPE_ID = 800857
				THEN 'Weeks ' + SUBSTRING(CONVERT(VARCHAR(6), fw.FISCAL_WEEK), 5, 6) + '/' + SUBSTRING(CONVERT(VARCHAR(6), fw.FISCAL_WEEK), 1, 4) + ' - ' + SUBSTRING(CONVERT(VARCHAR(6), fw1.FISCAL_WEEK), 5, 6) + '/' + SUBSTRING(CONVERT(VARCHAR(6), fw1.FISCAL_WEEK), 1, 4)
			ELSE '[Defaults]'
			END AS [DateRange],
		dbo.UDF_MID_GET_STORE_TEXT (st.ST_RID) AS [STORE_TEXT],
		st.ST_ID,
		st.STORE_NAME,
		hn.BN_ID,
		hn.BN_NAME,
		-- Begin TT#5382 - JSmith - Product descriptor not displayed when the Store Eligibility is set at the color level
		dbo.UDF_MID_GET_NODE_DISPLAY (dp.HN_RID) AS [DISPLAY_TEXT],
		--CASE 
		--	WHEN phl.PHL_DISPLAY_OPTION_ID = 800701
		--		THEN RTRIM(hn.BN_ID)
		--	WHEN phl.PHL_DISPLAY_OPTION_ID = 800702
		--		THEN RTRIM(hn.BN_DESCRIPTION)
		--	WHEN phl.PHL_DISPLAY_OPTION_ID = 800703
		--		THEN RTRIM(hn.BN_ID) + ' [' + RTRIM(hn.BN_NAME) + ']'
		--	WHEN phl.PHL_DISPLAY_OPTION_ID = 800704
		--		THEN RTRIM(hn.BN_ID) + ' [' + RTRIM(hn.BN_DESCRIPTION) + ']'
		--	WHEN phl.PHL_DISPLAY_OPTION_ID = 800705
		--		THEN RTRIM(hn.BN_NAME) + ' [' + RTRIM(hn.BN_DESCRIPTION) + ']'
		--	WHEN phl.PHL_DISPLAY_OPTION_ID = 800706
		--		THEN RTRIM(hn.BN_ID) + ' [' + RTRIM(hn.BN_NAME) + ']' + ' [' + RTRIM(hn.BN_DESCRIPTION) + ']'
		--	ELSE RTRIM(hn.BN_ID) + ' [' + RTRIM(hn.BN_NAME) + ']'
		--	END AS [DISPLAY_TEXT],
		-- End TT#5382 - JSmith - Product descriptor not displayed when the Store Eligibility is set at the color level
		dp.DAY1 + dp.DAY2 + dp.DAY3 + dp.DAY4 + dp.DAY5 + dp.DAY6 + dp.DAY7 AS TOTALPCT,
		dp.DAY1,
		dp.DAY2,
		dp.DAY3,
		dp.DAY4,
		dp.DAY5,
		dp.DAY6,
		dp.DAY7
	FROM DAILY_PERCENTAGES dp
	INNER JOIN HIERARCHY_NODE hen ON hen.HN_RID = dp.HN_RID
	LEFT OUTER JOIN PRODUCT_HIERARCHY_LEVELS phl ON phl.PHL_SEQUENCE = hen.HOME_LEVEL
	INNER JOIN CALENDAR_DATE_RANGE cdr ON cdr.CDR_RID = dp.CDR_RID
	LEFT OUTER JOIN FISCAL_WEEKS fw ON fw.FIRST_DAY_OF_WEEK = cdr.CDR_START
	LEFT OUTER JOIN FISCAL_WEEKS fw1 ON fw1.FIRST_DAY_OF_WEEK = cdr.CDR_END
	-- Begin TT#5382 - JSmith - Product descriptor not displayed when the Store Eligibility is set at the color level
	--INNER JOIN BASE_NODE bn ON dp.HN_RID = bn.HN_RID
	LEFT OUTER JOIN BASE_NODE bn ON dp.HN_RID = bn.HN_RID
	-- End TT#5382 - JSmith - Product descriptor not displayed when the Store Eligibility is set at the color level
		AND (
			DAY1 > 0
			OR DAY2 > 0
			OR DAY3 > 0
			OR DAY4 > 0
			OR DAY5 > 0
			OR DAY6 > 0
			OR DAY7 > 0
			)
	INNER JOIN APPLICATION_TEXT ap1 ON ap1.TEXT_CODE = cdr.CDR_RANGE_TYPE_ID
	INNER JOIN APPLICATION_TEXT ap2 ON ap2.TEXT_CODE = cdr.CDR_DATE_TYPE_ID
	INNER JOIN STORES st ON st.ST_RID = dp.ST_RID
		AND st.ST_ID = @STORE_ID
		AND st.ACTIVE_IND = '1' 
	INNER JOIN (
		SELECT PARENT_HN_RID,
			HN_RID,
			BN_ID,
			BN_NAME,
			BN_DESCRIPTION
		FROM #TEMP
		) hn ON hn.HN_RID = dp.HN_RID
	ORDER BY st.ST_ID,
		hn.PARENT_HN_RID,
		hn.BN_ID

	IF (SELECT object_id('tempdb.dbo.#TEMP')) > 0 DROP TABLE #TEMP

END
GO


