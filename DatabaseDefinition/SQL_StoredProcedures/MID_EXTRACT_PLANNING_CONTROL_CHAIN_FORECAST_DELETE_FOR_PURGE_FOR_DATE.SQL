--dv =============================================
--dv Create date:	4/16/2018
--dv Description:	Deletes top X rows from the extract planning control table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_EXTRACT_PLANNING_CONTROL_CHAIN_FORECAST_DELETE_FOR_PURGE_FOR_DATE] 
	@COMMIT_LIMIT int,
	@TIME_ID int
AS
BEGIN
	SET NOCOUNT ON;
	
	DELETE TOP (@COMMIT_LIMIT) epc WITH (TABLOCK)
    FROM EXTRACT_PLANNING_CONTROL epc WITH (INDEX(EXTRACT_PLANNING_CONTROL_PK))
    WHERE epc.TIME_ID < @TIME_ID
	  AND epc.FV_RID <> 1
	  AND epc.PLAN_TYPE = 0
	
	SELECT @@ROWCOUNT;

END
GO