--dv =============================================
--dv Create date:	2/3/2014
--dv Description:	Updates Forecast Bal Model table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_BAL_MODEL_UPDATE]
	@FBMOD_RID int,
	@FBMOD_ID varchar(50),
	@MATRIX_TYPE int,
	@ITERATIONS_TYPE int,
	@ITERATIONS_COUNT int,
	@BALANCE_MODE int,
	@CALC_MODE varchar(50)

AS
BEGIN
	SET NOCOUNT ON;

	UPDATE FORECAST_BAL_MODEL 
	SET 
		FBMOD_ID= @FBMOD_ID,
		MATRIX_TYPE = @MATRIX_TYPE,
		ITERATIONS_TYPE = @ITERATIONS_TYPE,
		ITERATIONS_COUNT = @ITERATIONS_COUNT,
		BALANCE_MODE = @BALANCE_MODE,
		CALC_MODE = @CALC_MODE
	WHERE FBMOD_RID = @FBMOD_RID

	SELECT @@ROWCOUNT;
END
GO