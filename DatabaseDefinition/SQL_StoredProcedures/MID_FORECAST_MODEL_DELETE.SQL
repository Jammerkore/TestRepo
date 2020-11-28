--dv =============================================
--dv Create date:	2/3/2014
--dv Description:	Deletes a Forecast Model 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_MODEL_DELETE]
	@FORECAST_MOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM FORECAST_MODEL  
	WHERE
	FORECAST_MOD_RID = @FORECAST_MOD_RID 
	 
	SELECT @@ROWCOUNT;
END
GO