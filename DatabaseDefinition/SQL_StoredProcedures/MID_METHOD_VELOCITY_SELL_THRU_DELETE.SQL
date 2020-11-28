--dv =============================================
--dv Create date:	1/27/2014
--dv Description:	Deletes a Velocity Sell Through Method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_VELOCITY_SELL_THRU_DELETE]
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_VELOCITY_SELL_THRU  
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;
END
GO