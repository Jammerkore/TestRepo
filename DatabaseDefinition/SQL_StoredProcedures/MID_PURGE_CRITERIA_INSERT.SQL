--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Inserts a row for purge criteria
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PURGE_CRITERIA_INSERT]
	@HN_RID int,
	@PURGE_DAILY_HISTORY int,
	@PURGE_WEEKLY_HISTORY int,
	@PURGE_PLANS int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO PURGE_CRITERIA(HN_RID, PURGE_DAILY_HISTORY, PURGE_WEEKLY_HISTORY, PURGE_PLANS)
	VALUES (@HN_RID, @PURGE_DAILY_HISTORY, @PURGE_WEEKLY_HISTORY, @PURGE_PLANS)

	SELECT @@ROWCOUNT;

END
GO
