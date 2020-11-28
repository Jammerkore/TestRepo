--dv =============================================
--dv Create date:	4/4/2014
--dv Description:	Reads filtered stock modifier models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_MODIFIER_MODEL_READ_FILTERED]
	@STKMOD_ID_FILTER varchar(200)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		STKMOD_RID as MODEL_RID, 
		STKMOD_ID as MODEL_NAME 
	FROM STOCK_MODIFIER_MODEL
    WHERE STKMOD_ID LIKE @STKMOD_ID_FILTER
	ORDER BY STKMOD_ID

END
GO