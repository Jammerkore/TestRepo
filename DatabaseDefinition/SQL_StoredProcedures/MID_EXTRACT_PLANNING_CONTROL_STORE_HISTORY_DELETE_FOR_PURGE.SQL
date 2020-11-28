--dv =============================================
--dv Create date:	4/16/2018
--dv Description:	Deletes top X rows from the extract planning control table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_EXTRACT_PLANNING_CONTROL_STORE_HISTORY_DELETE_FOR_PURGE] 
	@COMMIT_LIMIT int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE TOP (@COMMIT_LIMIT) epc WITH (TABLOCK)
    FROM EXTRACT_PLANNING_CONTROL epc WITH (INDEX(EXTRACT_PLANNING_CONTROL_PK))
    INNER JOIN PURGE_DATES pd on pd.HN_RID = epc.HN_RID
    WHERE epc.TIME_ID < pd.PURGE_WEEKLY_HISTORY
	  AND epc.FV_RID = 1
	  AND epc.PLAN_TYPE = 1
	
	SELECT @@ROWCOUNT;

END
GO