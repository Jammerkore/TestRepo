--dv =============================================
--dv Create date:	2/3/2014
--dv Description:	Deletes a Store Forecast Week Lock
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_FORECAST_WEEK_LOCK_DELETE]
	@HN_RID int,
	@FV_RID int,
	@TIME_ID_START int,
	@TIME_ID_END int,
	@STORE_RID_LIST STORE_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STORE_FORECAST_WEEK_LOCK 
	WHERE
	HN_RID = @HN_RID 
	AND FV_RID = @FV_RID
	AND TIME_ID between @TIME_ID_START and @TIME_ID_END
	AND ST_RID IN (SELECT ST_RID FROM @STORE_RID_LIST)
	 
	SELECT @@ROWCOUNT;
END
GO