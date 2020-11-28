--dv =============================================
--dv Create date:	10/24/2013
--dv Description:	Reads entries for a forward weeks of supply (FWOS) modifier model
--dv =============================================
CREATE PROCEDURE [dbo].[MID_FWOS_MODIFIER_MODEL_ENTRY_READ]
	@FWOSMOD_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		smme.FWOSMOD_RID, 
		smme.FWOSMOD_SEQUENCE,
		smme.FWOSMOD_VALUE, 
		smme.CDR_RID,
		cdr.CDR_START, 
		cdr.CDR_END, 
		cdr.CDR_DATE_TYPE_ID, 
		cdr.CDR_RELATIVE_TO, 
		cdr.CDR_RANGE_TYPE_ID 
	FROM FWOS_MODIFIER_MODEL_ENTRY smme, 
		 CALENDAR_DATE_RANGE cdr
	WHERE FWOSMOD_RID = @FWOSMOD_RID
		 AND smme.CDR_RID = cdr.CDR_RID 
	ORDER BY FWOSMOD_SEQUENCE

END
GO
