--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Reads all calendar date range names
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_DATE_RANGE_NAMES_READ_ALL]
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
	FROM CALENDAR_DATE_RANGE 
	WHERE CDR_NAME > '' 
	      AND CDR_NAME <> 'Undefined' 
	ORDER BY CDR_NAME

END
GO
