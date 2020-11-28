--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Deletes chain weekly history for purge
--dv History: TT#3599 - JSmith - Issue with Purge in MID Test
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CHAIN_HISTORY_WEEK_DELETE_FOR_PURGE]
	@COMMIT_LIMIT INT
AS
BEGIN
	SET NOCOUNT ON;

	DELETE TOP (@COMMIT_LIMIT) chw WITH (TABLOCK)
	FROM CHAIN_HISTORY_WEEK chw WITH (INDEX(CHAIN_HISTORY_WEEK_PK))
	INNER JOIN PURGE_DATES pd ON pd.HN_RID = chw.HN_RID
    WHERE chw.TIME_ID < pd.PURGE_WEEKLY_HISTORY

	SELECT @@ROWCOUNT;

END
GO