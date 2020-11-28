--dv =============================================
--dv Create date: 02/04/2014
--dv Description: Reads audit forecast rids less than the purge plan date
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_DATES_READ_AUDIT_FORECAST]
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eMethodType_OTSPlan INT = 802100;

	SELECT 
		AUDIT_FORECAST_RID 
	FROM AUDIT_FORECAST af WITH (NOLOCK)
	INNER JOIN PURGE_DATES pd ON af.HN_RID = pd.HN_RID
	INNER JOIN dbo.FISCAL_WEEKS fw ON af.TIME_RANGE_END = fw.FISCAL_WEEK
	WHERE af.METHOD_TYPE = @eMethodType_OTSPlan
		  AND fw.LAST_DAY_OF_WEEK <= pd.PURGE_PLANS

END
GO





