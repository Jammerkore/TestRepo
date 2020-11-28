--dv =============================================
--dv Modified date: 2/18/2014
--dv Flags: REFERENCED_FROM_SQL_ONLY
--dv Description: Utility stored procedure. Deletes the last two weeks of intransit.
--dv =============================================
CREATE PROCEDURE [dbo].[SP_MID_INTRANSIT_DELETE_2_WKS_PRIOR] 
	@returnCode INT OUTPUT
AS
BEGIN
	/*=========================================================================*/
	/* For WET SEAL */
	/*=========================================================================*/
	/*====================================================================*/
	/* Deletes All Internal Intransit two weeks prior to the current week */
	/*====================================================================*/
	DECLARE @debug INT
	DECLARE @currentDate DATETIME
	DECLARE @currentJulian INT
	DECLARE @currentWeek INT
	DECLARE @deleteDate DATETIME
	DECLARE @deleteYYYY CHAR(4)
	DECLARE @deleteDDD CHAR(3)
	DECLARE @deleteJulian INT
	DECLARE @deleteWeek INT
	DECLARE @rowsDeleted INT

	SET @returnCode = 0
	SET @debug = 0

	--Create table
	IF NOT EXISTS (
			SELECT *
			FROM dbo.sysobjects
			WHERE id = object_id(N'[dbo].[STORE_INTRANSIT_DELETED_WORK]')
				AND OBJECTPROPERTY(id, N'IsUserTable') = 1
			)
		CREATE TABLE [dbo].[STORE_INTRANSIT_DELETED_WORK] (
			HN_RID INT,
			TIME_ID INT,
			ST_RID INT,
			UNITS INT,
			DELETE_DATE DATETIME
			)

	-- Get current date and subtract 2 weeks from it
	SELECT @currentJulian = CURR_DATE_YYYYDDD
	FROM POSTING_DATE

	SELECT @currentDate = CURR_DATE
	FROM POSTING_DATE

	SELECT @currentWeek = FISCAL_WEEK
	FROM FISCAL_WEEKS
	WHERE @currentJulian >= FIRST_DAY_OF_WEEK
		AND @currentJulian <= LAST_DAY_OF_WEEK

	-- subtract 2 weeks from current date
	SET @deleteDate = DATEADD(week, - 2, @currentDate)
	-- Get @deleteJulian from @deleteDate
	SET @deleteYYYY = cast(datepart(yyyy, @deleteDate) AS CHAR(4))
	SET @deleteDDD = cast(datepart(dy, @deleteDate) AS CHAR(3))
	SET @deleteDDD = left('000', 3 - len(@deleteDDD)) + @deleteDDD
	SET @deleteJulian = cast(@deleteYYYY + @deleteDDD AS INT)

	SELECT @deleteWeek = FISCAL_WEEK
	FROM FISCAL_WEEKS
	WHERE @deleteJulian >= FIRST_DAY_OF_WEEK
		AND @deleteJulian <= LAST_DAY_OF_WEEK

	--Set @deleteJulian to last day of @DeleteWeek
	SELECT @deleteJulian = LAST_DAY_OF_WEEK
	FROM FISCAL_WEEKS
	WHERE FISCAL_WEEK = @deleteWeek

	IF @debug = 1
		SELECT @currentDate 'Current Date',
			@currentWeek 'Current Week',
			@currentJulian 'Current Julian',
			@deleteWeek 'Delete Week',
			@deleteJulian 'Delete Julian'

	-- Insert selected values into table
	INSERT INTO STORE_INTRANSIT_DELETED_WORK (
		HN_RID,
		TIME_ID,
		ST_RID,
		UNITS,
		DELETE_DATE
		)
	SELECT HN_RID,
		TIME_ID,
		ST_RID,
		UNITS,
		GETDATE()
	FROM STORE_INTRANSIT
	WHERE TIME_ID <= @deleteJulian

	-- Delete Intransit
	DELETE
	FROM STORE_INTRANSIT
	WHERE TIME_ID <= @deleteJulian

	SET @rowsDeleted = @@rowcount

	IF @debug = 1
		SELECT @rowsDeleted 'Rows Deleted'

	IF @rowsDeleted > 0
		SET @returnCode = 1

	RETURN @returnCode
END
GO


