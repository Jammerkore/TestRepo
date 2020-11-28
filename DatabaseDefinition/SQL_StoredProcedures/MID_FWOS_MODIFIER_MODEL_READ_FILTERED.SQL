--dv =============================================
--dv Create date:	4/4/2014
--dv Description:	Reads filtered FWOS Modifier models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FWOS_MODIFIER_MODEL_READ_FILTERED]
	@FWOSMOD_ID_FILTER varchar(200)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		FWOSMOD_RID as MODEL_RID, 
		FWOSMOD_ID as MODEL_NAME 
	FROM FWOS_MODIFIER_MODEL
    WHERE FWOSMOD_ID LIKE @FWOSMOD_ID_FILTER
	ORDER BY FWOSMOD_ID

END
GO