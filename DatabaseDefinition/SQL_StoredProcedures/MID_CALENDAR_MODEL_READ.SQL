--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Reads rows from the calendar model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_MODEL_READ] 
	@CM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		CM_RID, 
		CM_ID, 
		START_DATE, 
		FISCAL_YEAR 
	FROM CALENDAR_MODEL 
	WHERE CM_RID = @CM_RID

END
GO