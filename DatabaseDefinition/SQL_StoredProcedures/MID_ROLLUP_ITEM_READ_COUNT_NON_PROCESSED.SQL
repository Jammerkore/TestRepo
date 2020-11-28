--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads a count of roll up items for the process and product hierarchy
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_READ_COUNT_NON_PROCESSED]
	@PROCESS int,
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		COUNT(*) AS MyCount 
	FROM ROLLUP_ITEM
	WHERE PROCESS = @PROCESS
		  AND PH_RID = @PH_RID
		  AND ITEM_PROCESSED IS NULL

END
GO