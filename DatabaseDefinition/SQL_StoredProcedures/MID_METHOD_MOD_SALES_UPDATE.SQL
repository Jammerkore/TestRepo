--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Updates a modify sales method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MOD_SALES_UPDATE] 
	@METHOD_RID int,
	@HN_RID int, 
	@CDR_RID int, 
	@STORE_FILTER_RID int, 
	@AVERAGE_STORE int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE METHOD_MOD_SALES 
	SET
		HN_RID = @HN_RID,
		CDR_RID = @CDR_RID,
		STORE_FILTER_RID = @STORE_FILTER_RID,
		AVERAGE_STORE = @AVERAGE_STORE
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO


