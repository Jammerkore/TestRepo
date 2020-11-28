--dv =============================================
--dv Create date:	01/27/2014
--dv Description:	Reads a Velocity Basis Method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_SELL_THRU_READ]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		0 as 'RowPosition',
		mvst.VELOCITY_SELL_THRU_INDEX as 'SellThruIndex'
    FROM METHOD_VELOCITY_SELL_THRU mvst
    WHERE METHOD_RID = @METHOD_RID
	ORDER BY 
		VELOCITY_SELL_THRU_INDEX DESC
END
GO