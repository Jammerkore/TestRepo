--dv =============================================
--dv Create date:	11/21/2013
--dv Description:	Reads periods for the calendar model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_MODEL_PERIODS_READ] 
	@CM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		CM_RID, 
		CMP_SEQUENCE, 
		CMP_ID, 
		CMP_ABBREVIATION, 
		NO_OF_TIME_PERIODS, 
		CMP_TYPE 
	FROM CALENDAR_MODEL_PERIODS
	WHERE CM_RID = @CM_RID
	ORDER BY CMP_TYPE, CMP_SEQUENCE

END
GO