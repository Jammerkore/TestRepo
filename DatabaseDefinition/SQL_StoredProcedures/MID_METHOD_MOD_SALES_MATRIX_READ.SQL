--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Reads the modify sales method matrix values
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MOD_SALES_MATRIX_READ] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		METHOD_RID, 
		SGL_RID, 
		BOUNDARY, 
		SELL_THRU, 
		MATRIX_RULE,
		COALESCE(MATRIX_RULE_QUANTITY, 0) MATRIX_RULE_QUANTITY
	FROM METHOD_MOD_SALES_MATRIX
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY SGL_RID, BOUNDARY DESC, SELL_THRU DESC

END
GO

