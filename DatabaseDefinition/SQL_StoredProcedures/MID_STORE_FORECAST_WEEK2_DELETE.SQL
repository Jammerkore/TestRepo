--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes forecast history for the provided time ids, node and version
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_FORECAST_WEEK2_DELETE]
	@HN_RID INT,
	@FV_RID INT,
	@TIME_LIST TIME_ID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM STORE_FORECAST_WEEK2 WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID
	AND FV_RID = @FV_RID
	AND TIME_ID IN (SELECT TIME_ID FROM @TIME_LIST)

	SELECT @@ROWCOUNT;

END
GO