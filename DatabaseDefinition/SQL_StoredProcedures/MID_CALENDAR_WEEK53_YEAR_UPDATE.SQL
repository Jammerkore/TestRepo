--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Updates the CALENDAR_WEEK53 table based on the week 53 fiscal year
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_WEEK53_YEAR_UPDATE]
	@WEEK53_FISCAL_YEAR int,
	@CM_RID int,
	@CMP_SEQUENCE int,
	@OFFSET_ID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE CALENDAR_WEEK53_YEAR
	SET
		CM_RID = @CM_RID,
		CMP_SEQUENCE = @CMP_SEQUENCE,
		OFFSET_ID = @OFFSET_ID
	WHERE WEEK53_FISCAL_YEAR = @WEEK53_FISCAL_YEAR	

	SELECT @@ROWCOUNT;

END
GO
