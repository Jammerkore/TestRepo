--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes store forecast locks less than the date for the nodes provided
--dv History: TT#3599 - JSmith - Issue with Purge in MID Test
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE]
	@COMMIT_LIMIT INT,
	@TIME_ID INT,
	@NODE_LIST HN_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE TOP (@COMMIT_LIMIT) sfwl WITH (TABLOCK)
	FROM STORE_FORECAST_WEEK_LOCK sfwl WITH (INDEX(STORE_FORECAST_WEEK_LOCK_PK))
	WHERE TIME_ID < @TIME_ID
	AND HN_RID IN (SELECT HN_RID FROM @NODE_LIST)

	SELECT @@ROWCOUNT;

END
GO