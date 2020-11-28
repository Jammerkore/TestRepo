--dv =============================================
--dv Create date:	02/3/2014
--dv Description:	Returns all Forecast BAL Model Variables
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_BAL_MODEL_VARIABLE_READ_ALL_WITH_RID]
	@FBMOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		fbmv.*
    FROM FORECAST_BAL_MODEL_VARIABLE fbmv
    WHERE fbmv.FBMOD_RID = @FBMOD_RID

END
GO