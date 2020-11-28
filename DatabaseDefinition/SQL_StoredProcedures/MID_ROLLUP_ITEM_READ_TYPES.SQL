--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads rollup types for rollup items
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_READ_TYPES]
	@PROCESS int,
	@PH_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		ITEM_TYPE 
	FROM ROLLUP_ITEM
	WHERE PROCESS = @PROCESS
		  AND PH_RID = @PH_RID
		  AND ITEM_PROCESSED IS NULL
	ORDER BY ITEM_TYPE

END
GO