--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Reads rows from the CALENDAR_WEEK53 table based on the calendar model rid and calendar model period type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_WEEK53_YEAR_READ]
	@CM_RID int,
	@CMP_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		WEEK53_FISCAL_YEAR, 
		WK.CM_RID, 
		WK.CMP_SEQUENCE, 
		WK.OFFSET_ID, 
		PER.CMP_ID 
	FROM CALENDAR_WEEK53_YEAR WK 
	LEFT OUTER JOIN CALENDAR_MODEL_PERIODS PER ON WK.CMP_SEQUENCE = PER.CMP_SEQUENCE
	WHERE PER.CMP_TYPE = @CMP_TYPE
	      AND WK.CM_RID = PER.CM_RID 
		  AND WK.CM_RID = @CM_RID
	ORDER BY WEEK53_FISCAL_YEAR
				
END
GO
