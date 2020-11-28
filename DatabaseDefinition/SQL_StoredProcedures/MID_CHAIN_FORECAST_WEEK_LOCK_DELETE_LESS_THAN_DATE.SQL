--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes chain forecast locks less than the date for the nodes provided
--dv History: TT#3599 - JSmith - Issue with Purge in MID Test
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_LESS_THAN_DATE]
	@COMMIT_LIMIT INT,
	@TIME_ID INT,
	@NODE_LIST HN_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE TOP (@COMMIT_LIMIT) cfwl WITH (TABLOCK)
	FROM CHAIN_FORECAST_WEEK_LOCK cfwl WITH (INDEX(CHAIN_FORECAST_WEEK_LOCK_PK))
	WHERE TIME_ID < @TIME_ID
	AND HN_RID IN (SELECT HN_RID FROM @NODE_LIST)

	SELECT @@ROWCOUNT;

END
GO