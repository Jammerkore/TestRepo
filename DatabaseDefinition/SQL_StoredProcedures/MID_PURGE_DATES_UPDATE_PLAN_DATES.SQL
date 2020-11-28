--dv =============================================
--dv Create date: 02/04/2014
--dv Description: Update purge dates for plan weeks.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_DATES_UPDATE_PLAN_DATES]
	@PURGE_PLANS INT,
	@PURGE_PLANS_WEEKS INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE PURGE_DATES
	SET PURGE_PLANS = @PURGE_PLANS
	WHERE PURGE_PLANS_WEEKS = @PURGE_PLANS_WEEKS

	SELECT @@ROWCOUNT
END
GO



