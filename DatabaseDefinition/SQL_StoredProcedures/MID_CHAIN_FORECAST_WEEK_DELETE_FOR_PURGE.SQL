--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes chain forecast history for purge
--dv History: TT#3599 - JSmith - Issue with Purge in MID Test
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE]
	@COMMIT_LIMIT INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE TOP (@COMMIT_LIMIT) cfw WITH (TABLOCK)
	FROM CHAIN_FORECAST_WEEK cfw WITH (INDEX(CHAIN_FORECAST_WEEK_PK))
    INNER JOIN PURGE_DATES pd ON pd.HN_RID = cfw.HN_RID
    WHERE cfw.TIME_ID < pd.PURGE_PLANS

	SELECT @@ROWCOUNT;

END
GO