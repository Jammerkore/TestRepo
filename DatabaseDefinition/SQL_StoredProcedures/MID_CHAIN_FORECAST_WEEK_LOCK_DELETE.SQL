---dv =============================================
--dv Create date:	2/3/2014
--dv Description:	Deletes a Chain Forecast week lock
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_FORECAST_WEEK_LOCK_DELETE]
	@HN_RID int,
	@FV_RID int,
	@TIME_ID_START int,
	@TIME_ID_END int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM CHAIN_FORECAST_WEEK_LOCK 
	WHERE
	HN_RID = @HN_RID 
	AND FV_RID = @FV_RID
	AND TIME_ID between @TIME_ID_START and @TIME_ID_END

	SELECT @@ROWCOUNT;
END
GO