--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads versions for rollup items
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_READ_VERSIONS]
	@PROCESS int,
	@PH_RID int,
	@ITEM_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT 
		FV_RID 
	FROM ROLLUP_ITEM 
	WHERE PROCESS = @PROCESS
		  AND PH_RID = @PH_RID
		  AND ITEM_TYPE = @ITEM_TYPE
		  AND ITEM_PROCESSED IS NULL

END
GO