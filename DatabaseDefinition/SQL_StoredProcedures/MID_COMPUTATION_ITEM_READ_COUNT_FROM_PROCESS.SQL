--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Counts non-processed items for the provided process
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_ITEM_READ_COUNT_FROM_PROCESS]
	@PROCESS int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM COMPUTATION_ITEM
	WHERE PROCESS = @PROCESS
		  AND ITEM_PROCESSED IS NULL

END
GO

