--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Reads the computation driver info
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_DRIVER_INFO_READ]
	@PROCESS_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		PROCESS_RID, 
		TOTAL_ITEMS, 
		CONCURRENT_PROCESSES, 
		RECS_WITH_ERRORS 
	FROM COMPUTATION_DRIVER_INFO
	WHERE PROCESS_RID = @PROCESS_RID

END
GO
