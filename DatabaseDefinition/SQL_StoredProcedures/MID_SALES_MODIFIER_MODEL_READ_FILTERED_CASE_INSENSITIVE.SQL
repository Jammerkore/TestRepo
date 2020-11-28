--dv =============================================
--dv Create date:	4/4/2014
--dv Description:	Reads filtered sales modifier models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SALES_MODIFIER_MODEL_READ_FILTERED_CASE_INSENSITIVE]
	@SLSMOD_ID_FILTER varchar(200)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		SLSMOD_RID as MODEL_RID, 
		SLSMOD_ID as MODEL_NAME 
	FROM SALES_MODIFIER_MODEL
    WHERE SLSMOD_ID LIKE @SLSMOD_ID_FILTER collate SQL_Latin1_General_CP1_CI_AS --case insensitive
	ORDER BY SLSMOD_ID

END
GO