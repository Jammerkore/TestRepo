--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads forecast locks for the node, stores, and time frame
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_FORECAST_WEEK_LOCK_READ]
	@HN_RID int,
	@FV_RID int,
	@STORE_RID_LIST STORE_RID_TYPE READONLY,
	@START_WEEK int,
	@END_WEEK int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		ST_RID, 
		TIME_ID 
	FROM STORE_FORECAST_WEEK_LOCK 
	WHERE HN_RID = @HN_RID
		  AND FV_RID = @FV_RID
          AND ST_RID IN (SELECT ST_RID FROM @STORE_RID_LIST)
          AND TIME_ID BETWEEN @START_WEEK AND @END_WEEK
            

END
GO