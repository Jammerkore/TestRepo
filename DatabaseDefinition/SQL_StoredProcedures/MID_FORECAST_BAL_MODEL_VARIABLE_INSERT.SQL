--dv =============================================
--dv Create date:	2/3/2014
--dv Description:	Inserts a Forecast BAL Model Variable
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_BAL_MODEL_VARIABLE_INSERT]
	@FBMOD_RID int,
	@VARIABLE_SEQUENCE int,
	@VARIABLE_NUMBER int

AS
BEGIN
	SET NOCOUNT ON;

INSERT INTO FORECAST_BAL_MODEL_VARIABLE
		(
		FBMOD_RID,
		VARIABLE_SEQUENCE,
		VARIABLE_NUMBER
		)
	VALUES 
		(
		@FBMOD_RID,
		@VARIABLE_SEQUENCE,
		@VARIABLE_NUMBER
		)

	SELECT @@ROWCOUNT
END
GO