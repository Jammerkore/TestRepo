--dv =============================================
--dv Create date: 02/04/2014
--dv Description: Get distinct non-null purge dates for plans.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_DATES_READ_DISTINCT_PLANS_NON_NULL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		PURGE_PLANS 
	FROM PURGE_DATES WITH (NOLOCK) 
	WHERE PURGE_PLANS IS NOT NULL

END
GO


