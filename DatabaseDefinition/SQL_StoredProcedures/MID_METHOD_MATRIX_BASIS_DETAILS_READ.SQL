--dv =============================================
--dv Create date:	12/9/2013
--dv Description:	Reads rows from the method matrix details table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_MATRIX_BASIS_DETAILS_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SEQ_ID, 
		FV_RID, 
		CDR_RID, 
		WEIGHT,
		IS_INCLUDED_IND 
	FROM METHOD_MATRIX_BASIS_DETAILS
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY SEQ_ID

END
GO











