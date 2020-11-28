--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Updates a calendar date range name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_DATE_RANGE_UPDATE_NAME]
	@CDR_RID int,
	@CDR_NAME varchar(50)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE CALENDAR_DATE_RANGE
	SET 
		CDR_NAME = @CDR_NAME
	WHERE CDR_RID = @CDR_RID

	SELECT @@ROWCOUNT;

END
GO
