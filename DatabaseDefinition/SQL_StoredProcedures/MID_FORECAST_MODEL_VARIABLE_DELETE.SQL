--dv =============================================
--dv Create date:	1/30/2014
--dv Description:	Deletes a Forecast Model Variable
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_MODEL_VARIABLE_DELETE]
	@FORECAST_MOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM FORECAST_MODEL_VARIABLE  
	WHERE
	FORECAST_MOD_RID = @FORECAST_MOD_RID 
	 
	SELECT @@ROWCOUNT;
END
GO