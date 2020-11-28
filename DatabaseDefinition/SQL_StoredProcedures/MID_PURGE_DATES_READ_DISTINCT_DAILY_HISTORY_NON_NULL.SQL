--dv =============================================
--dv Create date: 02/04/2014
--dv Description: Get distinct non-null purge dates for daily history.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_DATES_READ_DISTINCT_DAILY_HISTORY_NON_NULL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		PURGE_DAILY_HISTORY 
	FROM PURGE_DATES WITH (NOLOCK) 
	WHERE PURGE_DAILY_HISTORY IS NOT NULL

END
GO