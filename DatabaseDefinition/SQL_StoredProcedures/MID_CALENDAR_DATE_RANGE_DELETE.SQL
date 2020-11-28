--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Deletes a calendar date range based on the calendar date range rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_DATE_RANGE_DELETE]
	@CDR_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM CALENDAR_DATE_RANGE
	WHERE CDR_RID = @CDR_RID

	SELECT @@ROWCOUNT;

END
GO
