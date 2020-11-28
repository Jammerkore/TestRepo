--dv =============================================
--dv Create date: 12/11/2013
--dv Description: Deletes a spread forecast method
--dv =============================================
CREATE PROCEDURE [dbo].[MID_METHOD_SPREAD_FORECAST_DELETE] 
	@METHOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM METHOD_SPREAD_FORECAST
	WHERE METHOD_RID = @METHOD_RID

	SELECT @@ROWCOUNT;

END
GO



