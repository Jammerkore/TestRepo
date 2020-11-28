--dv =============================================
--dv Description:	Returns chain set percent and time id for the store group levels in the store group specified
--dv History:		5/13/2015 -TT#1517-MD -jsobek -Store Service Optimization
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_CHAIN_SET_PERCENT_SET_RETURN] 
	(
	@BEG_WEEK INT,
	@END_WEEK INT,
	@HN_RID INT,
	@SG_RID INT,
	@SG_VERSION INT   -- TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
	)
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @aBEG_WEEK INT,
		@bEND_WEEK INT,
		@CUR_WEEK INT,
		@SGL_RID INT,
		@SG_ID VARCHAR(250)

	SELECT @aBEG_WEEK = FISCAL_WEEK
	FROM FISCAL_WEEKS
	WHERE FIRST_DAY_OF_WEEK = @BEG_WEEK

	SELECT @bEND_WEEK = FISCAL_WEEK
	FROM FISCAL_WEEKS
	WHERE FIRST_DAY_OF_WEEK = @END_WEEK

	CREATE TABLE #STORE_tbl (
		STOREGROUPRID INT,
		STOREGROUPID VARCHAR(250),
		STOREGROUPLEVELRID INT,
		STOREGROUPLEVELID VARCHAR(250),
		PERCENTAGE DECIMAL(6, 3),
		TIMEID INT
		)

	DECLARE MYSGLRID CURSOR
	FOR
	SELECT sgl.SGL_RID,
		sg.SG_ID
	FROM STORE_GROUP_JOIN sgl
	JOIN STORE_GROUP sg ON sgl.SG_RID = sg.SG_RID
	WHERE sg.SG_RID = @SG_RID
	  AND sgl.SG_VERSION = @SG_VERSION  -- TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.

	OPEN MYSGLRID

	FETCH NEXT
	FROM MYSGLRID
	INTO @SGL_RID,
		@SG_ID

	WHILE @@FETCH_STATUS = 0
	BEGIN
		DECLARE MYTIMEID CURSOR
		FOR
		SELECT FIRST_DAY_OF_WEEK
		FROM FISCAL_WEEKS
		WHERE FISCAL_WEEK BETWEEN @aBEG_WEEK
				AND @bEND_WEEK

		OPEN MYTIMEID

		FETCH NEXT
		FROM MYTIMEID
		INTO @CUR_WEEK

		WHILE @@FETCH_STATUS = 0
		BEGIN
			IF EXISTS (
					SELECT SGL_RID
					FROM CHAIN_SET_PERCENT_SET
					WHERE HN_RID = @HN_RID
						AND TIME_ID = @CUR_WEEK
						AND SGL_RID IN (
							SELECT sgl.SGL_RID
							FROM STORE_GROUP_JOIN sgl
							JOIN STORE_GROUP sg ON sgl.SG_RID = sg.SG_RID
							WHERE sg.SG_RID = @SG_RID
							  AND sgl.SG_VERSION = @SG_VERSION  -- TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
							)
					)
			BEGIN
				INSERT #STORE_tbl
				SELECT @SG_RID,
					@SG_ID,
					sgl.SGL_RID,
					sgl.SGL_OVERRIDE_ID,
					PERCENTAGE = (
						SELECT PERCENTAGE
						FROM CHAIN_SET_PERCENT_SET csps
						WHERE HN_RID = @HN_RID
							AND SGL_RID = @SGL_RID
							AND TIME_ID = @CUR_WEEK
						),
					@CUR_WEEK
				FROM STORE_GROUP_JOIN sgl
				WHERE sgl.SGL_RID = @SGL_RID
				  AND sgl.SG_VERSION = @SG_VERSION  -- TT#1935-MD - JSmith - SVC - Node Properties- Chain Set Percent - Select Str Attribute, Date Range and type in % by week.  Select the Apply button and receive a DB Error.
			END

			FETCH NEXT
			FROM MYTIMEID
			INTO @CUR_WEEK
		END

		CLOSE MYTIMEID

		DEALLOCATE MYTIMEID

		FETCH NEXT
		FROM MYSGLRID
		INTO @SGL_RID,
			@SG_ID
	END

	CLOSE MYSGLRID

	DEALLOCATE MYSGLRID

	SELECT *
	FROM #STORE_tbl
	ORDER BY TIMEID,
		STOREGROUPLEVELID

	DROP TABLE #STORE_tbl
END
GO


