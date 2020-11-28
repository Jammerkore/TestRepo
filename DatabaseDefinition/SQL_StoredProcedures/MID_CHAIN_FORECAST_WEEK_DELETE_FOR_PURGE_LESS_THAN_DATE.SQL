--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes chain forecast history for purge less than the provided time
--dv History: TT#3599 - JSmith - Issue with Purge in MID Test
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_FORECAST_WEEK_DELETE_FOR_PURGE_LESS_THAN_DATE]
	@COMMIT_LIMIT INT,
	@TIME_ID INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE TOP (@COMMIT_LIMIT) cfw WITH (TABLOCK) 
	FROM CHAIN_FORECAST_WEEK cfw WITH (INDEX(CHAIN_FORECAST_WEEK_PK))
    WHERE TIME_ID < @TIME_ID

	SELECT @@ROWCOUNT;

END
GO