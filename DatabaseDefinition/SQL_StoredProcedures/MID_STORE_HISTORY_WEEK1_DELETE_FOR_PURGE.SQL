--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Deletes top X rows from the store history week table
--dv History: TT#3599 - JSmith - Issue with Purge in MID Test
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_HISTORY_WEEK1_DELETE_FOR_PURGE] 
	@COMMIT_LIMIT int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE TOP (@COMMIT_LIMIT) sh WITH (TABLOCK)
    FROM STORE_HISTORY_WEEK1 sh WITH (INDEX(STORE_HISTORY_WEEK1_PK))
		 INNER JOIN PURGE_DATES pd on pd.HN_RID = sh.HN_RID
    WHERE sh.TIME_ID < pd.PURGE_WEEKLY_HISTORY
	
	SELECT @@ROWCOUNT;

END
GO