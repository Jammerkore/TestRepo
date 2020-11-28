--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Reads all calendar models
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_MODEL_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		CM_RID, 
		CM_ID, 
		START_DATE, 
		FISCAL_YEAR 
	FROM CALENDAR_MODEL 
	ORDER BY START_DATE

END
GO
