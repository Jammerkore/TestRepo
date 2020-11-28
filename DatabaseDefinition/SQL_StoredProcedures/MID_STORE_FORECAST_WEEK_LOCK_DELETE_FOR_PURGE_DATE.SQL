--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Deletes top X rows from the forecast week lock table
--dv History: TT#3599 - JSmith - Issue with Purge in MID Test
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE_DATE] 
	@COMMIT_LIMIT int,
	@TIME_ID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE TOP (@COMMIT_LIMIT) sfwl WITH (TABLOCK) 
	FROM STORE_FORECAST_WEEK_LOCK sfwl WITH (INDEX(STORE_FORECAST_WEEK_LOCK_PK))
    WHERE TIME_ID < @TIME_ID
	
	SELECT @@ROWCOUNT;

END
GO