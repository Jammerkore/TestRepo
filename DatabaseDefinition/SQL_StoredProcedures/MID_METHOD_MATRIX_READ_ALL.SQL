--dv =============================================
--dv Create date:	12/9/2013
--dv Description:	Reads all rows from the method matrix table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MATRIX_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		METHOD_RID, 
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
		OLL_RID
	FROM METHOD_MATRIX

END
GO











