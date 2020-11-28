--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Reads a calendar date range based on the calendar date range rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_DATE_RANGE_READ]
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		CDR_RID, 
		CDR_START, 
		CDR_END, 
		CDR_RANGE_TYPE_ID, 
		CDR_DATE_TYPE_ID, 
		CDR_RELATIVE_TO, 
		CDR_NAME, 
		CDR_DYNAMIC_SWITCH,
		COALESCE(CDR_DYNAMIC_SWITCH_DATE,0) CDR_DYNAMIC_SWITCH_DATE 
	FROM CALENDAR_DATE_RANGE WHERE CDR_RID = @CDR_RID

END
GO
