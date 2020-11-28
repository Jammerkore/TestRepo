--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description:	Reads store capacity for the nodes and store list
--dv History: TT#1145 - JSmith - Shows inactive stores
--dv =============================================
CREATE PROCEDURE [dbo].[SP_GET_STORE_CAPACITY_FOR_STORE_SET] 
	@SELECTED_NODE_RID INT,
	@SELECTED_NODE_LEVEL INT,
	@LOWER_LEVEL INT,
	@STORE_RID_LIST TEXT
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
		BN_DESCRIPTION VARCHAR(250) COLLATE Latin1_General_CS_AS,
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
	EXEC SP_GET_ALL_DESCENDANTS_BY_LEVEL_REPORT @SELECTED_NODE_RID,
		@LEVEL

	SELECT 'StoreCapacity',
		hn.PARENT_HN_RID,
		sct.HN_RID,
		hen.HOME_LEVEL,
		dbo.UDF_MID_GET_STORE_TEXT (st.ST_RID) AS [STORE_TEXT],
		st.ST_ID,
		st.STORE_NAME,
		hn.BN_ID,
		hn.BN_NAME,
		-- Begin TT#5382 - JSmith - Product descriptor not displayed when the Store Eligibility is set at the color level
		dbo.UDF_MID_GET_NODE_DISPLAY (sct.HN_RID) AS [DISPLAY_TEXT],
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
		sct.ST_CAPACITY
	FROM STORE_CAPACITY sct
	INNER JOIN HIERARCHY_NODE hen ON hen.HN_RID = sct.HN_RID
	LEFT OUTER JOIN PRODUCT_HIERARCHY_LEVELS phl ON phl.PHL_SEQUENCE = hen.HOME_LEVEL
	INNER JOIN BASE_NODE bn ON sct.HN_RID = bn.HN_RID
	INNER JOIN STORES st ON st.ST_RID = sct.ST_RID
		AND st.ACTIVE_IND = '1' 
	INNER JOIN #STORE_tbl ON storeRID = st.ST_RID
	INNER JOIN (
		SELECT PARENT_HN_RID,
			HN_RID,
			BN_ID,
			BN_NAME,
			BN_DESCRIPTION
		FROM #TEMP
		) hn ON hn.HN_RID = sct.HN_RID

	IF (SELECT object_id('tempdb.dbo.#TEMP')) > 0 DROP TABLE #TEMP

	DROP TABLE #STORE_tbl

END
GO


