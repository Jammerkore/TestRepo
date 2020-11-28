--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Reads the modify sales method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MOD_SALES_READ] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @NoRid int = -1;
	DECLARE @UndefinedCalendarDateRange int = 1;
	DECLARE @UndefinedStoreFilter int = 1;

	SELECT 
		COALESCE(HN_RID, @NoRid) HN_RID,
		COALESCE(CDR_RID, @UndefinedCalendarDateRange) CDR_RID,
		COALESCE(STORE_FILTER_RID, @UndefinedStoreFilter) STORE_FILTER_RID,
		AVERAGE_STORE 
	FROM METHOD_MOD_SALES 
	WHERE METHOD_RID = @METHOD_RID

END
GO





