--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Deletes top X rows from the store history table
--dv History: TT#3599 - JSmith - Issue with Purge in MID Test
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_HISTORY_DAY1_DELETE_FOR_PURGE_DATE] 
	@COMMIT_LIMIT int,
	@TIME_ID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE TOP (@COMMIT_LIMIT) sh WITH (TABLOCK)
    FROM STORE_HISTORY_DAY1 sh WITH (INDEX(STORE_HISTORY_DAY1_PK))
         INNER JOIN PURGE_DATES pd ON pd.HN_RID = sh.HN_RID
    WHERE sh.TIME_ID < @TIME_ID
	
	SELECT @@ROWCOUNT;

END
GO