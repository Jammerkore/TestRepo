--dv =============================================
--dv Create date:	01/27/2014
--dv Description:	Reads a Velocity Basis Method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_GROUP_LEVEL_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		mvgl.SGL_RID as 'SglRID', 
		mvgl.NO_ONHAND_RULE as 'NoOnHandRule', 
		mvgl.NO_ONHAND_QUANTITY as 'NoOnHandQty',
		mvgl.MATRIX_MODE_IND as 'ModeInd',
		mvgl.MATRIX_MODE_AVERAGE_RULE as 'AverageRule',
		mvgl.MATRIX_MODE_AVERAGE_QUANITY as 'AverageQty',
		mvgl.MATRIX_SPREAD_IND as 'SpreadInd'
    FROM METHOD_VELOCITY_GROUP_LEVEL mvgl
    WHERE METHOD_RID = @METHOD_RID
	ORDER BY 
		SGL_RID ASC
END
GO