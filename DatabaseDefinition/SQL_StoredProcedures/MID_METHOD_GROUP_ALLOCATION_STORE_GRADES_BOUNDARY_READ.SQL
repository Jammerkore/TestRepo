--dv =============================================
--dv Create date: 8/14/2014
--dv Description: Reads store grade boundaries for a group allocation method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY_READ]
	@METHOD_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		0 AS RowPosition, 
		GRADE_CODE AS GradeCode, 
		BOUNDARY AS Boundary
    FROM METHOD_GROUP_ALLOCATION_STORE_GRADES_BOUNDARY
    WHERE METHOD_RID = @METHOD_RID
    ORDER BY BOUNDARY DESC

END
GO