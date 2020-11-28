--dv =============================================
--dv Create date:	01/30/2014
--dv Description:	Returns all Forecast Model Variables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_MODEL_VARIABLE_READ_ALL]
	@FORECAST_MOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		fmv.*
    FROM FORECAST_MODEL_VARIABLE fmv
    WHERE fmv.FORECAST_MOD_RID = @FORECAST_MOD_RID
	ORDER BY fmv.VARIABLE_SEQUENCE

END
GO