--dv =============================================
--dv Create date: 2/7/2014
--dv Description: Reads color min max overrides for the override method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_OVERRIDE_COLOR_MINMAX_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		0 AS RowPosition, 
		COLOR_CODE_RID AS ColorCodeRID,
		COLOR_MIN AS Minimum, 
		COLOR_MAX AS Maximum 
	FROM METHOD_OVERRIDE_COLOR_MINMAX
	WHERE METHOD_RID = @METHOD_RID
            
END
GO