--dv =============================================
--dv Create date:	02/3/2014
--dv Description:	Returns all Forecast Models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_MODEL_READ_ALL_WITH_RID]
	@MODEL_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		fm.*
    FROM FORECAST_MODEL fm
    WHERE fm.FORECAST_MOD_RID = @MODEL_RID

END
GO