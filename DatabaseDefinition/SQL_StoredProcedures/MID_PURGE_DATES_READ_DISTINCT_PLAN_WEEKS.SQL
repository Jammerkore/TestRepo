--dv =============================================
--dv Create date: 02/03/2014
--dv Description: Get distinct purge dates by plan weeks.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_DATES_READ_DISTINCT_PLAN_WEEKS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT PURGE_PLANS_WEEKS
	FROM PURGE_DATES WITH (NOLOCK)
END
GO