--dv =============================================
--dv Create date:	4/4/2014
--dv Description:	Reads filtered FWOS Max models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FWOS_MAX_MODEL_READ_FILTERED_CASE_INSENSITIVE]
	@FWOSMAX_ID_FILTER varchar(200)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		FWOSMAX_RID as MODEL_RID, 
		FWOSMAX_ID as MODEL_NAME 
	FROM FWOS_MAX_MODEL
    WHERE FWOSMAX_ID LIKE @FWOSMAX_ID_FILTER collate SQL_Latin1_General_CP1_CI_AS --case insensitive
	ORDER BY FWOSMAX_ID

END
GO