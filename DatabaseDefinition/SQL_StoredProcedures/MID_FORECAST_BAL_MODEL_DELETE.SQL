--dv =============================================
--dv Create date:	2/3/2014
--dv Description:	Deletes a Forecast BAL Model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_BAL_MODEL_DELETE]
	@FBMOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM FORECAST_BAL_MODEL 
	WHERE
	FBMOD_RID = @FBMOD_RID 
	 
	SELECT @@ROWCOUNT;
END
GO