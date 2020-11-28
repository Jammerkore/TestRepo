--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Deletes a modify sales method child data
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MOD_SALES_DELETE_CHILD_DATA] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE FROM METHOD_MOD_SALES_MATRIX WHERE METHOD_RID = @METHOD_RID
	DELETE FROM METHOD_MOD_SALES_GRADE WHERE METHOD_RID = @METHOD_RID
	DELETE FROM METHOD_MOD_SALES_SELL_THRU WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO



