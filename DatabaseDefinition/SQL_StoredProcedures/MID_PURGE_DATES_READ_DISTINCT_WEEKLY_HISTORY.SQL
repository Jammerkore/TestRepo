--dv =============================================
--dv Create date: 02/03/2014
--dv Description: Get distinct purge history weeks.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_DATES_READ_DISTINCT_WEEKLY_HISTORY]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT PURGE_WEEKLY_HISTORY_WEEKS
	FROM PURGE_DATES WITH (NOLOCK)
END
GO
