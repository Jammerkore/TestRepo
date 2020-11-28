--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Updates a rollup item to clear batch numbers
--dv Modified date:	3/2/2017
--dv                TT#5485 - JSmith - Performance
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ROLLUP_ITEM_UPDATE_FOR_CLEAR]
	@PROCESS int,
	@FV_RID int,
	@ITEM_TYPE int,
	@PH_RID int,
	@HOME_LEVEL int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE ROLLUP_ITEM
	SET BATCH_NUMBER = NULL
	--WHERE PROCESS = @PROCESS
	--  AND FV_RID = @FV_RID
	--  AND ITEM_TYPE = @ITEM_TYPE
	--  AND PH_RID = @PH_RID
	--  AND HOME_LEVEL = @HOME_LEVEL
	WHERE FV_RID = @FV_RID
	  AND ITEM_TYPE = @ITEM_TYPE
	  AND PROCESS = @PROCESS
	  AND PH_RID = @PH_RID
	  AND HOME_LEVEL = @HOME_LEVEL

	SELECT @@ROWCOUNT;
END
GO
