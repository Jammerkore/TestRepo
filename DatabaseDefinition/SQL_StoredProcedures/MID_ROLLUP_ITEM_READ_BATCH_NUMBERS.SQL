--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads rollup itme batch numbers
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_READ_BATCH_NUMBERS]
	@PROCESS int,
	@PH_RID int,
	@FV_RID int,
	@ITEM_TYPE int,
	@HOME_LEVEL int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT DISTINCT
		HN_MOD, 
		BATCH_NUMBER 
	FROM ROLLUP_ITEM
	WHERE PROCESS = @PROCESS
		  AND PH_RID = @PH_RID
		  AND FV_RID = @FV_RID
		  AND ITEM_TYPE = @ITEM_TYPE
		  AND HOME_LEVEL = @HOME_LEVEL
	 ORDER BY HN_MOD ASC, BATCH_NUMBER ASC

END
GO