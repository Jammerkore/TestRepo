--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Reads a sales modifier model entry
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SALES_MODIFIER_MODEL_ENTRY_READ]
	@SLSMOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		smme.SLSMOD_RID, 
		smme.SLSMOD_SEQUENCE,
		smme.SLSMOD_VALUE, 
		smme.CDR_RID,
		cdr.CDR_START, 
		cdr.CDR_END, 
		cdr.CDR_DATE_TYPE_ID, 
		cdr.CDR_RELATIVE_TO, 
		cdr.CDR_RANGE_TYPE_ID 
	FROM SALES_MODIFIER_MODEL_ENTRY smme, 
		 CALENDAR_DATE_RANGE cdr
	WHERE SLSMOD_RID = @SLSMOD_RID
		 AND smme.CDR_RID = cdr.CDR_RID 
	ORDER BY SLSMOD_SEQUENCE

END
GO
