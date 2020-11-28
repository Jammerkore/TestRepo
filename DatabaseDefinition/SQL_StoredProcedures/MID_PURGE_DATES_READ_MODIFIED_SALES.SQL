--dv =============================================
--dv Create date: 02/04/2014
--dv Description: Reads audit forecast rids less than the purge plan date, for modified sales
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_DATES_READ_MODIFIED_SALES]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eMethodType_ForecastModifySales INT = 802132;

	SELECT 
		AUDIT_FORECAST_RID 
	FROM AUDIT_FORECAST af WITH (NOLOCK)
	INNER JOIN PURGE_DATES pd ON af.HN_RID = pd.HN_RID
	INNER JOIN dbo.FISCAL_WEEKS fw ON af.TIME_RANGE_END = fw.FISCAL_WEEK
	WHERE af.METHOD_TYPE = @eMethodType_ForecastModifySales
		  AND fw.LAST_DAY_OF_WEEK <= pd.PURGE_WEEKLY_HISTORY

END
GO




