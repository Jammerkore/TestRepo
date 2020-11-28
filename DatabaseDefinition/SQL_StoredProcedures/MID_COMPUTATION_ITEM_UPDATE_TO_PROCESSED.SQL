--dv =============================================
--dv Create date:	11/22/2013
--dv Description:	Sets items to processed based on the process, computation group, item type, node, version, and fiscal weeks
--dv =============================================
CREATE PROCEDURE [dbo].[MID_COMPUTATION_ITEM_UPDATE_TO_PROCESSED]
	@PROCESS int,
	@CG_RID int,
	@ITEM_TYPE int,
	@HN_RID int,
	@FV_RID int,
	@FROM_FISCAL_YEAR_WEEK int,
	@TO_FISCAL_YEAR_WEEK int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE COMPUTATION_ITEM
	SET 
		ITEM_PROCESSED = '1'
	WHERE PROCESS = @PROCESS
		  AND CG_RID = @CG_RID
		  AND ITEM_TYPE = @ITEM_TYPE
		  AND HN_RID = @HN_RID
		  AND FV_RID = @FV_RID
		  AND FISCAL_YEAR_WEEK BETWEEN @FROM_FISCAL_YEAR_WEEK AND @TO_FISCAL_YEAR_WEEK

	SELECT @@ROWCOUNT;

END
GO


