--dv =============================================
--dv Create date:	2/3/2014
--dv Description:	Updates Forecast Model table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_MODEL_UPDATE]
	@FORECAST_MOD_RID int,
	@FORECAST_MOD_ID varchar(50),
	@DEFAULT_IND char(1),
	@CALC_MODE varchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE FORECAST_MODEL 
	SET 
		FORECAST_MOD_ID = @FORECAST_MOD_ID,
		DEFAULT_IND = @DEFAULT_IND,
		CALC_MODE = @CALC_MODE
	WHERE FORECAST_MOD_RID = @FORECAST_MOD_RID

	SELECT @@ROWCOUNT;
END
GO