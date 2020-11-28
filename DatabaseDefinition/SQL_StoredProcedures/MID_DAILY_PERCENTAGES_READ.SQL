--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Reads daily percentages for the given hierarchy node rid 
--dv =============================================
CREATE PROCEDURE [dbo].[MID_DAILY_PERCENTAGES_READ]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		dp.HN_RID, 
		dp.ST_RID, 
		dp.CDR_RID,
		dp.DAY1, 
		dp.DAY2, 
		dp.DAY3, 
		dp.DAY4, 
		dp.DAY5, 
		dp.DAY6, 
		dp.DAY7,
		cdr.CDR_START, 
		cdr.CDR_END, 
		cdr.CDR_DATE_TYPE_ID, 
		cdr.CDR_RELATIVE_TO, 
		cdr.CDR_RANGE_TYPE_ID 
	FROM DAILY_PERCENTAGES dp, 
		 CALENDAR_DATE_RANGE cdr
	WHERE HN_RID = @HN_RID
		 AND dp.CDR_RID = cdr.CDR_RID
	ORDER BY ST_RID

END
GO
