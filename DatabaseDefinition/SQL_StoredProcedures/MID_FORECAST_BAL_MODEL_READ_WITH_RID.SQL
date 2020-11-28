--dv =============================================
--dv Create date:	02/4/2014
--dv Description:	Returns all Forecast BAL Model 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_BAL_MODEL_READ_WITH_RID]
	@FBMOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		fbm.* 
	FROM FORECAST_BAL_MODEL fbm
	WHERE fbm.FBMOD_RID = @FBMOD_RID

END
GO