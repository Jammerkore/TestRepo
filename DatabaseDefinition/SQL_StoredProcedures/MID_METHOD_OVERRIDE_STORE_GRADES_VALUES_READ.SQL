--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads store grade overrides for the override method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_STORE_GRADES_VALUES_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		0 AS RowPosition,
		SGL_RID,      
		BOUNDARY AS Boundary,
		MINIMUM_STOCK AS MinStock, 
		MAXIMUM_STOCK AS MaxStock,
		MINIMUM_AD AS MinAd, 
		MINIMUM_COLOR AS MinColor,
		MAXIMUM_COLOR AS MaxColor,
		SHIP_UP_TO AS ShipUpTo  
	FROM METHOD_OVERRIDE_STORE_GRADES_VALUES
	WHERE METHOD_RID = @METHOD_RID
	ORDER BY SGL_RID, BOUNDARY DESC

END
GO