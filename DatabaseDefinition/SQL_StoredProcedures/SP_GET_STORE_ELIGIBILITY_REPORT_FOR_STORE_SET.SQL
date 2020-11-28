--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description:	Reads store eligibility for the nodes and store list
--dv History:	TT#1145 - JSmith - Shows inactive stores
--dv			TT#1146 - JSmith - Eligibilty Models visible in Node Properties Audit, but not MID Node Properties
--dv			TT#1585-MD - RMatelic - Data Layer Request - Stored Procedure changes needed for Node Properties Override Report (TT#4388)
--dv =============================================
CREATE PROCEDURE [dbo].[SP_GET_STORE_ELIGIBILITY_REPORT_FOR_STORE_SET] 
	@SELECTED_NODE_RID INT,
	@SELECTED_NODE_LEVEL INT,
	@LOWER_LEVEL INT,
	@STORE_RID_LIST TEXT,
	@ShowEligibility INT,
	@ShowModifiers INT,
	@ShowSimilarStore INT
AS
BEGIN
	DECLARE @LEVEL AS INT

	SELECT @LEVEL = @LOWER_LEVEL - 1

	CREATE TABLE #STORE_tbl (storeRID INT)

	INSERT #STORE_tbl
	SELECT *
	FROM dbo.UDF_MID_SPLIT_CSV_INT(@STORE_RID_LIST, ',')

	CREATE TABLE #TEMP (
		PARENT_HN_RID INT,
		HN_RID INT,
		BN_ID VARCHAR(356) COLLATE Latin1_General_CS_AS,
		BN_NAME VARCHAR(50) COLLATE Latin1_General_CS_AS,
		BN_DESCRIPTION VARCHAR(250) COLLATE Latin1_General_CS_AS
		)

	INSERT #TEMP (
		PARENT_HN_RID,
		HN_RID,
		BN_ID,
		BN_NAME,
		BN_DESCRIPTION
		)
	EXEC SP_GET_ALL_DESCENDANTS_BY_LEVEL_REPORT @SELECTED_NODE_RID,
		@LEVEL

	SELECT 'StoreEligibility',
		hen.HOME_PH_RID,
		hn.PARENT_HN_RID,
		se.HN_RID,
		st.ST_ID,
		st.STORE_NAME,
		hn.BN_ID,
		hn.BN_NAME,
		-- Begin TT#5382 - JSmith - Product descriptor not displayed when the Store Eligibility is set at the color level
		dbo.UDF_MID_GET_NODE_DISPLAY (se.HN_RID) AS [DISPLAY_TEXT],
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
		CASE 
			WHEN se.USE_ELIGIBILITY = 1
				AND se.INELIGIBLE = 1
				THEN NULL
			ELSE em.EM_ID
			END AS [EM_ID],
		CASE 
			WHEN se.USE_ELIGIBILITY = 1
				THEN se.INELIGIBLE
			WHEN se.USE_ELIGIBILITY = 0
				THEN NULL
			END AS [INELIGIBLE],
		CASE 
			WHEN se.STKMOD_TYPE = 2
				THEN sm.STKMOD_ID
			WHEN se.STKMOD_TYPE = 1
				THEN CONVERT(VARCHAR(50), se.STKMOD_PCT)
			END AS [STKMOD_ID],
		CASE 
			WHEN se.SLSMOD_TYPE = 2
				THEN slm.SLSMOD_ID
			WHEN se.SLSMOD_TYPE = 1
				THEN CONVERT(VARCHAR(50), se.SLSMOD_PCT)
			END AS [SLSMOD_ID],
		CASE 
			WHEN se.FWOSMOD_TYPE = 2
				THEN fm.FWOSMOD_ID
			WHEN se.FWOSMOD_TYPE = 1
				THEN CONVERT(VARCHAR(50), se.FWOSMOD_PCT)
			END AS [FWOSMOD_ID],
		SimilarStore,
		CASE 
			WHEN se.SIMILAR_STORE_RATIO = 0.0
				OR se.SIMILAR_STORE_RATIO = - 1.0
				OR se.SIMILAR_STORE_RATIO = 100.0
				THEN NULL
			ELSE se.SIMILAR_STORE_RATIO
			END AS [SIMILAR_STORE_RATIO],
		per.PERIOD,
		cd.CDR_RANGE_TYPE_ID,
		cd.CDR_RID,
		per.ST_RID AS STORE_RID
	FROM STORE_ELIGIBILITY se
	LEFT OUTER JOIN CALENDAR_DATE_RANGE cd ON cd.CDR_RID = se.UNTIL_DATE
	INNER JOIN HIERARCHY_NODE hen ON se.HN_RID = hen.HN_RID
	LEFT OUTER JOIN PRODUCT_HIERARCHY_LEVELS phl ON phl.PHL_SEQUENCE = hen.HOME_LEVEL
	INNER JOIN STORES st ON st.ST_RID = se.ST_RID
		AND st.ACTIVE_IND = '1' 
	INNER JOIN #STORE_tbl ON storeRID = st.ST_RID
	LEFT OUTER JOIN ELIGIBILITY_MODEL em ON se.EM_RID = em.EM_RID
		AND @ShowEligibility = 1
	LEFT OUTER JOIN STOCK_MODIFIER_MODEL sm ON se.STKMOD_RID = sm.STKMOD_RID
		AND @ShowModifiers = 1
	LEFT OUTER JOIN SALES_MODIFIER_MODEL slm ON se.SLSMOD_RID = slm.SLSMOD_RID
		AND @ShowModifiers = 1
	LEFT OUTER JOIN FWOS_MODIFIER_MODEL fm ON se.FWOSMOD_RID = fm.FWOSMOD_RID
		AND @ShowModifiers = 1
	LEFT OUTER JOIN (
		(
			SELECT sms.HN_RID,
				sms.ST_RID,
				st.ST_ID + '[' + st.STORE_NAME + ']' AS [SimilarStore]
			FROM SIMILAR_STORES sms
			INNER JOIN STORES st ON sms.SS_RID = st.ST_RID
				AND st.ACTIVE_IND = '1' 
			INNER JOIN (
				SELECT HN_RID,
					ST_RID
				FROM SIMILAR_STORES
				GROUP BY HN_RID,
					ST_RID
				HAVING COUNT(SS_RID) = 1
				) s ON s.HN_RID = sms.HN_RID
				AND s.ST_RID = sms.ST_RID
			)

		UNION

		(
			SELECT sms.HN_RID,
				sms.ST_RID,
				'(Average of stores)' AS [SimilarStore]
			FROM SIMILAR_STORES sms
			INNER JOIN STORES st ON sms.SS_RID = st.ST_RID
				AND st.ACTIVE_IND = '1' 
			INNER JOIN (
				SELECT HN_RID,
					ST_RID
				FROM SIMILAR_STORES
				GROUP BY HN_RID,
					ST_RID
				HAVING COUNT(SS_RID) > 1
				) s ON s.HN_RID = sms.HN_RID
				AND s.ST_RID = sms.ST_RID
			GROUP BY sms.HN_RID,
				sms.ST_RID
			)
		) sms1 ON sms1.HN_RID = se.HN_RID
		AND sms1.ST_RID = se.ST_RID
		AND @ShowSimilarStore = 1
		AND se.SIMILAR_STORE_TYPE != 0 /* TT#4388-VStuart-Node Properties Report */
	LEFT OUTER JOIN (
		SELECT HN_RID,
			ST_RID,
			cd.CDR_NAME AS PERIOD,
			cd.CDR_RANGE_TYPE_ID,
			cd.CDR_RID,
			NULL AS STORE_RID
		FROM STORE_ELIGIBILITY se
		INNER JOIN CALENDAR_DATE_RANGE cd ON cd.CDR_RID = se.UNTIL_DATE
		WHERE UNTIL_DATE IS NOT NULL
			AND cd.CDR_NAME != ''

		UNION

		SELECT HN_RID,
			ST_RID,
			CASE 
				WHEN CDR_DATE_TYPE_ID = 800853
					THEN 'Periods ' + CONVERT(VARCHAR(50), CDR_START) + ' to ' + CONVERT(VARCHAR(50), CDR_END)
				WHEN CDR_DATE_TYPE_ID = 800851
					THEN CASE 
							WHEN CDR_END IS NOT NULL
								AND CDR_END > 0
								THEN '+' + CONVERT(VARCHAR(50), CDR_START) + 'Weeks '
							WHEN CDR_START IS NOT NULL
								AND CDR_START < 0
								THEN CONVERT(VARCHAR(50), CDR_START) + 'Weeks '
							END
				END AS PERIOD,
			cd.CDR_RANGE_TYPE_ID,
			cd.CDR_RID,
			NULL AS STORE_RID
		FROM STORE_ELIGIBILITY se
		INNER JOIN CALENDAR_DATE_RANGE cd ON cd.CDR_RID = se.UNTIL_DATE
		WHERE UNTIL_DATE IS NOT NULL
			AND CDR_RANGE_TYPE_ID = 800858

		UNION

		SELECT HN_RID,
			ST_RID,
			'Weeks ' + SUBSTRING(CONVERT(VARCHAR(6), fw.FISCAL_WEEK), 5, 6) + '/' + SUBSTRING(CONVERT(VARCHAR(6), fw.FISCAL_WEEK), 1, 4) + ' - ' + SUBSTRING(CONVERT(VARCHAR(6), fw1.FISCAL_WEEK), 5, 6) + '/' + SUBSTRING(CONVERT(VARCHAR(6), fw1.FISCAL_WEEK), 1, 4) AS PERIOD,
			cd.CDR_RANGE_TYPE_ID,
			cd.CDR_RID,
			NULL AS STORE_RID
		FROM STORE_ELIGIBILITY se
		INNER JOIN CALENDAR_DATE_RANGE cd ON cd.CDR_RID = se.UNTIL_DATE
		INNER JOIN FISCAL_WEEKS fw ON fw.FIRST_DAY_OF_WEEK = cd.CDR_START
		INNER JOIN FISCAL_WEEKS fw1 ON fw1.FIRST_DAY_OF_WEEK = cd.CDR_END
		WHERE UNTIL_DATE IS NOT NULL
			AND CDR_RANGE_TYPE_ID = 800857
		) per ON per.HN_RID = se.HN_RID
		AND per.ST_RID = se.ST_RID
		AND @ShowSimilarStore = 1
		AND se.SIMILAR_STORE_TYPE != 0 /* TT#4388-VStuart-Node Properties Report */
	INNER JOIN (
		SELECT PARENT_HN_RID,
			HN_RID,
			BN_ID,
			BN_NAME,
			BN_DESCRIPTION
		FROM #TEMP
		) hn ON hn.HN_RID = se.HN_RID
	WHERE (
			se.INELIGIBLE = 1
			AND se.USE_ELIGIBILITY = 1
			AND @ShowEligibility = 1
			)
		OR (
			em.EM_ID IS NOT NULL
			AND @ShowEligibility = 1
			)
		OR (
			(
				se.STKMOD_TYPE > 0
				OR se.SLSMOD_TYPE > 0
				OR se.FWOSMOD_TYPE > 0
				)
			AND @ShowModifiers = 1
			)
		OR (
			SimilarStore IS NOT NULL
			AND @ShowSimilarStore = 1
			AND se.SIMILAR_STORE_TYPE != 0 /* TT#4388-VStuart-Node Properties Report */
			)
		OR (
			SIMILAR_STORE_RATIO IS NOT NULL
			AND SIMILAR_STORE_RATIO > 0
			AND SIMILAR_STORE_RATIO != 100.0
			AND @ShowSimilarStore = 1
			AND se.SIMILAR_STORE_TYPE != 0 /* TT#4388-VStuart-Node Properties Report */
			)
	ORDER BY st.ST_ID,
		hen.HOME_PH_RID,
		hn.PARENT_HN_RID,
		hn.BN_ID

	IF (SELECT object_id('tempdb.dbo.#TEMP')) > 0 DROP TABLE #TEMP

	DROP TABLE #STORE_tbl

END

GO


