--dv =============================================
--dv Create date:	12/9/2013
--dv Description:	Reads rows from the method matrix table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MATRIX_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		FILTER_RID, 
		HN_RID, 
		HIGH_LEVEL_FV_RID, 
		CDR_RID, 
		MODEL_RID, 
		MATRIX_TYPE,
		LOW_LEVEL_FV_RID, 
		LOW_LEVEL_TYPE, 
		LOW_LEVEL_OFFSET, 
		LOW_LEVEL_SEQUENCE, 
		INCLUDE_INELIGIBLE_STORES_IND, 
		INCLUDE_SIMILAR_STORES_IND, 
		VARIABLE_NUMBER, 
		ITERATIONS_TYPE, 
		ITERATIONS_COUNT, 
		BALANCE_MODE, 
		CALC_MODE, 
		COALESCE(OLL_RID, -1) OLL_RID 
	FROM METHOD_MATRIX 
	WHERE METHOD_RID = @METHOD_RID

END
GO











