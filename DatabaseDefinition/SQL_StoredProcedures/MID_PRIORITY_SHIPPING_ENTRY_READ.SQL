--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Reads a priority shipping entry based on the eligibility model rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_PRIORITY_SHIPPING_ENTRY_READ]
	@EM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		pse.EM_RID, 
		pse.PRI_SHIP_EM_SEQUENCE, 
		pse.CDR_RID,
		cdr.CDR_START, 
		cdr.CDR_END, 
		cdr.CDR_DATE_TYPE_ID, 
		cdr.CDR_RELATIVE_TO, 
		cdr.CDR_RANGE_TYPE_ID
	FROM PRIORITY_SHIPPING_ENTRY pse, 
		 CALENDAR_DATE_RANGE cdr
	WHERE EM_RID=@EM_RID
		  AND pse.CDR_RID = cdr.CDR_RID
	ORDER BY PRI_SHIP_EM_SEQUENCE

END
GO
