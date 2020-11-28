--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes chain forecast history locks for the provided time ids and node
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_FORECAST_WEEK_LOCK_DELETE_FROM_NODE]
	@HN_RID INT,
	@FV_RID INT,
	@TIME_LIST TIME_ID_TYPE READONLY
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM CHAIN_FORECAST_WEEK_LOCK WITH (ROWLOCK)
	WHERE HN_RID = @HN_RID
		  AND FV_RID = @FV_RID
		  AND TIME_ID IN (SELECT TIME_ID FROM @TIME_LIST)

	SELECT @@ROWCOUNT;

END
GO