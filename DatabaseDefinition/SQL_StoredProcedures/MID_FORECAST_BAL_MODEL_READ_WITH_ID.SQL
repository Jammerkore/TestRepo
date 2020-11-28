--dv =============================================
--dv Create date:	02/4/2014
--dv Description:	Returns all Forecast BAL Model 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_BAL_MODEL_READ_WITH_ID]
	@FBMOD_ID varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		fbm.* 
	FROM FORECAST_BAL_MODEL fbm
	WHERE fbm.FBMOD_ID = @FBMOD_ID

END
GO