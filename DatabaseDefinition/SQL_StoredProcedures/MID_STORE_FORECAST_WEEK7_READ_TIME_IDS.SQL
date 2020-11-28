--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads the time ids for the node and version
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_FORECAST_WEEK7_READ_TIME_IDS]
	@HN_RID int,
	@FV_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
		TIME_ID 
	FROM STORE_FORECAST_WEEK7
	WHERE HN_RID = @HN_RID
		  AND FV_RID = @FV_RID
	ORDER BY TIME_ID

END
GO