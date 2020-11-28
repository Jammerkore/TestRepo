--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Deletes top X rows from the store forecast week lock table
--dv History: TT#3599 - JSmith - Issue with Purge in MID Test
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STORE_FORECAST_WEEK_LOCK_DELETE_FOR_PURGE] 
	@COMMIT_LIMIT int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE TOP (@COMMIT_LIMIT) sfwl with (TABLOCK)
	FROM STORE_FORECAST_WEEK_LOCK sfwl with (INDEX(STORE_FORECAST_WEEK_LOCK_PK))
    INNER JOIN PURGE_DATES pd on pd.HN_RID = sfwl.HN_RID
    WHERE sfwl.TIME_ID < pd.PURGE_PLANS
	
	SELECT @@ROWCOUNT;

END
GO