--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Reads a stock modifier model entry row based on the stock modifier model rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_STOCK_MODIFIER_MODEL_ENTRY_READ]
	@STKMOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		smme.STKMOD_RID, 
		smme.STKMOD_SEQUENCE,
		smme.STKMOD_VALUE, 
		smme.CDR_RID,
		cdr.CDR_START, 
		cdr.CDR_END, 
		cdr.CDR_DATE_TYPE_ID, 
		cdr.CDR_RELATIVE_TO, 
		cdr.CDR_RANGE_TYPE_ID 
	 FROM STOCK_MODIFIER_MODEL_ENTRY smme, 
		  CALENDAR_DATE_RANGE cdr
	 WHERE STKMOD_RID=@STKMOD_RID
		   AND smme.CDR_RID = cdr.CDR_RID 
	 ORDER BY STKMOD_SEQUENCE

END
GO
