--dv =============================================
--dv Create date: 02/04/2014
--dv Description: Get distinct non-null purge dates for weekly history.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY_NON_NULL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		PURGE_WEEKLY_HISTORY 
	FROM PURGE_DATES WITH (NOLOCK) 
	WHERE PURGE_WEEKLY_HISTORY IS NOT NULL

END
GO