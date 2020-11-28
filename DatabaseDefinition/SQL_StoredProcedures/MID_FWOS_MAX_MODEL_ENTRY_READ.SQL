--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Reads entries for a forward weeks of supply (FWOS) max model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FWOS_MAX_MODEL_ENTRY_READ]
	@FWOSMAX_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		smme.FWOSMAX_RID, 
		smme.FWOSMAX_SEQUENCE,
        smme.FWOSMAX_VALUE, 
		smme.CDR_RID,
        cdr.CDR_START, 
		cdr.CDR_END, 
		cdr.CDR_DATE_TYPE_ID, 
		cdr.CDR_RELATIVE_TO, 
		cdr.CDR_RANGE_TYPE_ID 
    FROM FWOS_MAX_MODEL_ENTRY smme, 
		 CALENDAR_DATE_RANGE cdr
    WHERE FWOSMAX_RID = @FWOSMAX_RID
          AND smme.CDR_RID = cdr.CDR_RID 
    ORDER BY FWOSMAX_SEQUENCE

END
GO
