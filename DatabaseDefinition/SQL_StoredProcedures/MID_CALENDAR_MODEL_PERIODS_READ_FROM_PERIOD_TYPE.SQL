--dv =============================================
--dv Author:		Jeff Sobek
--dv Create date:	10/28/2013
--dv Description:	Reads calendar model periods based on the calendar model rid and calendar model period type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_MODEL_PERIODS_READ_FROM_PERIOD_TYPE]
	@CM_RID int,
	@CMP_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		CM_RID, 
		CMP_TYPE, 
		CMP_SEQUENCE, 
		CMP_ID, 
		CMP_ABBREVIATION,
		NO_OF_TIME_PERIODS 
	FROM CALENDAR_MODEL_PERIODS 
	WHERE CM_RID = @CM_RID
		  AND CMP_TYPE = @CMP_TYPE
	ORDER BY CMP_SEQUENCE

END
GO