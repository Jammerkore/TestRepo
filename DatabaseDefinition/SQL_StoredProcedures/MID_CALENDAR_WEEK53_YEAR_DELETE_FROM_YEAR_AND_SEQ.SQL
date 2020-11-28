--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Deletes from the CALENDAR_WEEK53 table based on the calendar model rid, week 53 fiscal year, and period sequence
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_WEEK53_YEAR_DELETE_FROM_YEAR_AND_SEQ]
	@CM_RID int,
	@WEEK53_FISCAL_YEAR int,
	@CMP_SEQUENCE int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM CALENDAR_WEEK53_YEAR 
	WHERE WEEK53_FISCAL_YEAR = @WEEK53_FISCAL_YEAR
		  AND CM_RID = @CM_RID
		  AND CMP_SEQUENCE = @CMP_SEQUENCE		

	SELECT @@ROWCOUNT;

END
GO
