--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Inserts a modify sales method matrix
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MOD_SALES_MATRIX_INSERT] 
	@METHOD_RID int,
	@SGL_RID int, 
	@BOUNDARY int, 
	@SELL_THRU int, 
	@MATRIX_RULE int, 
	@MATRIX_RULE_QUANTITY float
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO METHOD_MOD_SALES_MATRIX
		(
		METHOD_RID, 
		SGL_RID, 
		BOUNDARY, 
		SELL_THRU, 
		MATRIX_RULE, 
		MATRIX_RULE_QUANTITY
		)
	VALUES
		(
		@METHOD_RID, 
		@SGL_RID, 
		@BOUNDARY, 
		@SELL_THRU, 
		@MATRIX_RULE, 
		@MATRIX_RULE_QUANTITY
		)

	SELECT @@ROWCOUNT;

END
GO

