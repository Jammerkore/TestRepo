--dv =============================================
--dv Create date: 2/11/2014
--dv Description: Reads store grades for the asssortment property
--dv =============================================
CREATE PROCEDURE [dbo].[MID_ASSORTMENT_PROPERTIES_STORE_GRADE_READ]
	@HDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		HDR_RID, 
		STORE_GRADE_SEQ, 
		COALESCE(BOUNDARY_UNITS,0) as BOUNDARY_UNITS,
		COALESCE(BOUNDARY_INDEX,0) as BOUNDARY_INDEX, 
		GRADE_CODE 
	FROM ASSORTMENT_PROPERTIES_STORE_GRADE
	WHERE HDR_RID = @HDR_RID
	ORDER BY STORE_GRADE_SEQ

END
GO