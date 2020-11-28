--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes chain forecast history locks for purge
--dv History: TT#3599 - JSmith - Issue with Purge in MID Test
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE]
	@COMMIT_LIMIT INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE TOP (@COMMIT_LIMIT) cfwl WITH (TABLOCK)
	FROM CHAIN_FORECAST_WEEK_LOCK cfwl WITH (INDEX(CHAIN_FORECAST_WEEK_LOCK_PK))
    INNER JOIN PURGE_DATES pd ON pd.HN_RID = cfwl.HN_RID
    WHERE cfwl.TIME_ID < pd.PURGE_PLANS

	SELECT @@ROWCOUNT;

END
GO