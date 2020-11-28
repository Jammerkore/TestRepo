--dv =============================================
--dv Create date:	2/14/2014
--dv Description:	Returns Method Rollup Basis Detail
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_ROLLUP_BASIS_DETAIL_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		DETAIL_SEQ,
		' ', 
		FROM_LEVEL_HRID, 
		FROM_LEVEL_TYPE, 
		FROM_LEVEL_SEQ, 
		FROM_LEVEL_OFFSET,
		' ', 
		TO_LEVEL_HRID, 
		TO_LEVEL_TYPE, 
		TO_LEVEL_SEQ, 
		TO_LEVEL_OFFSET,
		' ', 
		' ', 
		' ', 
		STORE_IND, 
		CHAIN_IND, 
		STORE_TO_CHAIN_IND 
    FROM METHOD_ROLLUP_BASIS_DETAIL MR
	WHERE MR.METHOD_RID = @METHOD_RID
	ORDER BY DETAIL_SEQ

END
GO