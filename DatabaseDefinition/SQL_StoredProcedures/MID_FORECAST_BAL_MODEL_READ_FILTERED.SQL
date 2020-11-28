--dv =============================================
--dv Create date:	4/4/2014
--dv Description:	Reads filtered forecast balance models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FORECAST_BAL_MODEL_READ_FILTERED]
	@FBMOD_ID_FILTER varchar(200)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		FBMOD_RID as MODEL_RID, 
		FBMOD_ID as MODEL_NAME 
	FROM FORECAST_BAL_MODEL
    WHERE FBMOD_ID LIKE @FBMOD_ID_FILTER
	ORDER BY FBMOD_ID

END
GO