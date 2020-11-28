--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes forecast history locks for the provided time ids, node and stores
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_FORECAST_WEEK3_DELETE_FROM_NODE]
	@HN_RID INT,
	@FV_RID INT,
	@TIME_LIST TIME_ID_TYPE READONLY,
	@STORE_LIST STORE_RID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STORE_FORECAST_WEEK3 WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID
	AND TIME_ID IN (SELECT TIME_ID FROM @TIME_LIST)
	AND ST_RID IN (SELECT ST_RID FROM @STORE_LIST)

	SELECT @@ROWCOUNT;

END
GO