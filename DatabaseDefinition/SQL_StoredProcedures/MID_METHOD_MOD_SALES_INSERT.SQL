--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Inserts a modify sales method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MOD_SALES_INSERT] 
	@METHOD_RID int,
	@HN_RID int,
	@CDR_RID int, 
	@STORE_FILTER_RID int, 
	@AVERAGE_STORE int
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_MOD_SALES
		(
		METHOD_RID, 
		HN_RID,
		CDR_RID, 
		STORE_FILTER_RID, 
		AVERAGE_STORE
		)
	VALUES
		(
		@METHOD_RID, 
		@HN_RID,
		@CDR_RID, 
		@STORE_FILTER_RID, 
		@AVERAGE_STORE
		)

	SELECT @@ROWCOUNT;

END
GO
