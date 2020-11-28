--dv =============================================
--dv Create date:	4/4/2014
--dv Description:	Reads filtered FWOS Max models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FWOS_MAX_MODEL_READ_FILTERED]
	@FWOSMAX_ID_FILTER varchar(200)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		FWOSMAX_RID as MODEL_RID, 
		FWOSMAX_ID as MODEL_NAME 
	FROM FWOS_MAX_MODEL
    WHERE FWOSMAX_ID LIKE @FWOSMAX_ID_FILTER
	ORDER BY FWOSMAX_ID

END
GO