--dv =============================================
--dv Create date:	10/23/2013
--dv Description:	Reads rows from the stock eligibility entry table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_ELIGIBILITY_ENTRY_READ]
	@EM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		see.EM_RID, 
		see.STOCK_EM_SEQUENCE, 
		see.CDR_RID,
		cdr.CDR_START, 
		cdr.CDR_END, 
		cdr.CDR_DATE_TYPE_ID, 
		cdr.CDR_RELATIVE_TO, 
		cdr.CDR_RANGE_TYPE_ID,
		cdr.CDR_NAME
	FROM STOCK_ELIGIBILITY_ENTRY see, 
		 CALENDAR_DATE_RANGE cdr
	WHERE EM_RID=@EM_RID
		  AND see.CDR_RID = cdr.CDR_RID 
	ORDER BY STOCK_EM_SEQUENCE

END
GO
