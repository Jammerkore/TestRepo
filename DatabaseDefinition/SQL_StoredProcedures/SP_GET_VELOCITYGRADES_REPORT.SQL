CREATE PROCEDURE [dbo].[SP_GET_VELOCITYGRADES_REPORT] 
	@SELECTED_NODE_RID INT,
	@LOWER_LEVEL INT
AS
BEGIN
	DECLARE @SELECTED_NODE_LEVEL AS INT

	SELECT @SELECTED_NODE_LEVEL = HOME_LEVEL
	FROM HIERARCHY_NODE
	WHERE HN_RID = @SELECTED_NODE_RID

	DECLARE @MAX_PHL_SEQUENCE AS INT

	SELECT @MAX_PHL_SEQUENCE = MAX(PHL_SEQUENCE)
	FROM PRODUCT_HIERARCHY_LEVELS

	IF (@LOWER_LEVEL = 0)
	BEGIN
		SELECT @LOWER_LEVEL = @MAX_PHL_SEQUENCE
	END

	DECLARE @LEVEL AS INT

	SELECT @LEVEL = @LOWER_LEVEL - 1

	CREATE TABLE #TEMP (
		PARENT_HN_RID INT,
		HN_RID INT,
		BN_ID CHAR(356) COLLATE Latin1_General_CS_AS,
		BN_NAME CHAR(50) COLLATE Latin1_General_CS_AS,
		BN_DESCRIPTION CHAR(250) COLLATE Latin1_General_CS_AS
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

    -- Begin TT#5382 - JSmith - Product descriptor not displayed when the Store Eligibility is set at the color level
	SELECT dbo.UDF_MID_GET_NODE_DISPLAY (vg.HN_RID) AS [DISPLAY_TEXT],
	--SELECT CASE 
	--		WHEN phl.PHL_DISPLAY_OPTION_ID = 800701
	--			THEN RTRIM(hn.BN_ID)
	--		WHEN phl.PHL_DISPLAY_OPTION_ID = 800702
	--			THEN RTRIM(hn.BN_DESCRIPTION)
	--		WHEN phl.PHL_DISPLAY_OPTION_ID = 800703
	--			THEN RTRIM(hn.BN_ID) + ' [' + RTRIM(hn.BN_NAME) + ']'
	--		WHEN phl.PHL_DISPLAY_OPTION_ID = 800704
	--			THEN RTRIM(hn.BN_ID) + ' [' + RTRIM(hn.BN_DESCRIPTION) + ']'
	--		WHEN phl.PHL_DISPLAY_OPTION_ID = 800705
	--			THEN RTRIM(hn.BN_NAME) + ' [' + RTRIM(hn.BN_DESCRIPTION) + ']'
	--		WHEN phl.PHL_DISPLAY_OPTION_ID = 800706
	--			THEN RTRIM(hn.BN_ID) + ' [' + RTRIM(hn.BN_NAME) + ']' + ' [' + RTRIM(hn.BN_DESCRIPTION) + ']'
	--		ELSE RTRIM(hn.BN_ID) + ' [' + RTRIM(hn.BN_NAME) + ']'
	--		END AS [DISPLAY_TEXT],
	-- End TT#5382 - JSmith - Product descriptor not displayed when the Store Eligibility is set at the color level
		--Begin TT#881 - JScott - Node Properties Audit Report is not including Sell Thru
		--vg.BOUNDARY,vg.GRADE_CODE,vg.SELL_THRU_PCT
		vg.BOUNDARY,
		vg.GRADE_CODE,
		CASE 
			WHEN vg.MINIMUM_STOCK = - 1
				THEN NULL
			ELSE vg.MINIMUM_STOCK
			END AS [MINIMUM_STOCK],
		CASE 
			WHEN vg.MAXIMUM_STOCK = - 1
				THEN NULL
			ELSE vg.MAXIMUM_STOCK
			END AS MAXIMUM_STOCK,
		CASE 
			WHEN vg.MINIMUM_AD = - 1
				THEN NULL
			ELSE vg.MINIMUM_AD
			END AS MINIMUM_AD,
		vg.SELL_THRU_PCT
	--End TT#881 - JScott - Node Properties Audit Report is not including Sell Thru
	FROM (
		--Begin TT#881 - JScott - Node Properties Audit Report is not including Sell Thru
		--SELECT vg.HN_RID,vg.BOUNDARY,vg.GRADE_CODE,SELL_THRU_PCT
		SELECT vg.HN_RID,
			vg.BOUNDARY,
			vg.GRADE_CODE,
			vg.MINIMUM_STOCK,
			vg.MAXIMUM_STOCK,
			vg.MINIMUM_AD,
			SELL_THRU_PCT
		--End TT#881 - JScott - Node Properties Audit Report is not including Sell Thru
		FROM (
			SELECT vg1.HN_RID,
				(
					SELECT count(*)
					FROM VELOCITY_GRADE vg2
					WHERE vg2.BOUNDARY > vg1.BOUNDARY
						AND vg1.HN_RID = vg2.HN_RID
					) AS SequenceNo,
				--Begin TT#881 - JScott - Node Properties Audit Report is not including Sell Thru
				--vg1.BOUNDARY,vg1.GRADE_CODE
				vg1.BOUNDARY,
				vg1.GRADE_CODE,
				vg1.MINIMUM_STOCK,
				vg1.MAXIMUM_STOCK,
				vg1.MINIMUM_AD
			--End TT#881 - JScott - Node Properties Audit Report is not including Sell Thru
			FROM VELOCITY_GRADE vg1
			) vg
		LEFT OUTER JOIN (
			SELECT st.HN_RID,
				(
					SELECT count(*)
					FROM SELL_THRU st1
					WHERE st.SELL_THRU_PCT < st1.SELL_THRU_PCT
						AND st.HN_RID = st1.HN_RID
					) AS SequenceNo,
				st.SELL_THRU_PCT
			FROM SELL_THRU st
			) st ON st.HN_RID = vg.HN_RID
			AND st.SequenceNo = vg.SequenceNo
		
		UNION
		
		--Begin TT#881 - JScott - Node Properties Audit Report is not including Sell Thru
		--SELECT st.HN_RID,NULL,NULL, st.SELL_THRU_PCT
		SELECT st.HN_RID,
			NULL,
			NULL,
			NULL,
			NULL,
			NULL,
			st.SELL_THRU_PCT
		--End TT#881 - JScott - Node Properties Audit Report is not including Sell Thru
		FROM SELL_THRU st
		WHERE NOT EXISTS (
				SELECT *
				FROM VELOCITY_GRADE
				WHERE HN_RID = st.HN_RID
				)
		) vg
	INNER JOIN HIERARCHY_NODE hen ON hen.HN_RID = vg.HN_RID
	LEFT OUTER JOIN PRODUCT_HIERARCHY_LEVELS phl ON phl.PHL_SEQUENCE = hen.HOME_LEVEL
	INNER JOIN (
		SELECT PARENT_HN_RID,
			HN_RID,
			BN_ID,
			BN_NAME,
			BN_DESCRIPTION
		FROM #TEMP
		) hn ON hn.HN_RID = vg.HN_RID
	ORDER BY hn.PARENT_HN_RID,
		hn.BN_ID,
		GRADE_CODE

	IF (
			SELECT object_id('tempdb.dbo.#TEMP')
			) > 0
		DROP TABLE #TEMP
END
GO


