--dv =============================================
--dv Create date:	10/29/2013
--dv Description:	Inserts a row into the modify sales matrix table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_AUDIT_MODIFY_SALES_MATRIX_INSERT]
	@AUDIT_FORECAST_RID int, 
	@SGL_RID int, 
	@BOUNDARY int, 
	@GRADE_CODE varchar(20),
	@SELL_THRU int, 
	@NUMBER_OF_STORES int, 
	@MATRIX_RULE int, 
	@MATRIX_RULE_QUANTITY float
AS
BEGIN
	SET NOCOUNT ON;

	INSERT INTO AUDIT_MODIFY_SALES_MATRIX
		(
		AUDIT_FORECAST_RID, 
		SGL_RID, 
		BOUNDARY, 
		GRADE_CODE,
		SELL_THRU, 
		NUMBER_OF_STORES, 
		MATRIX_RULE, 
		MATRIX_RULE_QUANTITY
		)
	VALUES 
		(
		@AUDIT_FORECAST_RID, 
		@SGL_RID, 
		@BOUNDARY, 
		@GRADE_CODE,
		@SELL_THRU, 
		@NUMBER_OF_STORES, 
		@MATRIX_RULE, 
		@MATRIX_RULE_QUANTITY
		)
	
	SELECT @@ROWCOUNT;

END
GO
