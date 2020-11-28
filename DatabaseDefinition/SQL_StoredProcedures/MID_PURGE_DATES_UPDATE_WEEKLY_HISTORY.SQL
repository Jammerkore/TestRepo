--dv =============================================
--dv Create date: 02/04/2014
--dv Description: Update purge dates weekly history weeks.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_DATES_UPDATE_WEEKLY_HISTORY]
	@PURGE_WEEKLY_HISTORY INT,
	@PURGE_WEEKLY_HISTORY_WEEKS INT
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE PURGE_DATES
	SET PURGE_WEEKLY_HISTORY = @PURGE_WEEKLY_HISTORY
	WHERE PURGE_WEEKLY_HISTORY_WEEKS = @PURGE_WEEKLY_HISTORY_WEEKS

	SELECT @@ROWCOUNT
END
GO
