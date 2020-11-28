--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads grade boundarys for the override method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_STORE_GRADES_BOUNDARY_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		0 AS RowPosition, 
		GRADE_CODE AS GradeCode, 
		BOUNDARY AS Boundary
	FROM METHOD_OVERRIDE_STORE_GRADES_BOUNDARY
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY BOUNDARY DESC

END
GO