--dv =============================================
--dv Create date:	10/25/2013
--dv Description:	Reads size curves for similar stores based on the hierarchy node passed in
--dv =============================================
CREATE PROCEDURE [dbo].[MID_NODE_SIZE_CURVE_SIMILAR_STORE_READ]
	@HN_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		nscss.*, 
		cdr.CDR_RID, 
		cdr.CDR_NAME, 
		cdr.CDR_START, 
		cdr.CDR_END, 
		cdr.CDR_RANGE_TYPE_ID, 
		cdr.CDR_DATE_TYPE_ID, 
		cdr.CDR_RELATIVE_TO 
	FROM NODE_SIZE_CURVE_SIMILAR_STORE nscss 
		 LEFT OUTER JOIN CALENDAR_DATE_RANGE cdr ON nscss.UNTIL_DATE = cdr.CDR_RID 
	WHERE HN_RID = @HN_RID

END
GO
