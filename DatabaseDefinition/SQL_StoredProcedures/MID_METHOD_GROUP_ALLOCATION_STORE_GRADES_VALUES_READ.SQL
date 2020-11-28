--dv =============================================
--dv Create date: 8/14/2014
--dv Description: Reads a store grade values for a group allocation
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES_READ]
	@METHOD_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		0 AS RowPosition,
		SGL_RID,
		BOUNDARY AS Boundary,
		MINIMUM_GROUP AS MinGroup, 
		MAXIMUM_GROUP AS MaxGroup,
		MINIMUM_HEADER AS MinHeader,
		MAXIMUM_HEADER AS MaxHeader
	FROM METHOD_GROUP_ALLOCATION_STORE_GRADES_VALUES
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY SGL_RID, BOUNDARY DESC

END
GO