--dv =============================================
--dv Create date:	01/27/2014
--dv Description:	Reads a Velocity Basis Method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_MATRIX_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		mvm.SGL_RID as 'SglRID', 
		mvm.BOUNDARY as 'Boundary', 
		mvm.VELOCITY_SELL_THRU_INDEX as 'SellThruIndex',
		mvm.VELOCITY_RULE as 'VelocityRule',
		mvm.VELOCITY_QUANTITY as 'VelocityQty',
		0 as 'Stores',
		0 as 'AvgWOS'
    FROM METHOD_VELOCITY_MATRIX mvm
    WHERE METHOD_RID = @METHOD_RID
	ORDER BY 
		SGL_RID ASC, 
		BOUNDARY DESC, 
		VELOCITY_SELL_THRU_INDEX DESC
END
GO