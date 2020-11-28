--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Deletes from the CALENDAR_WEEK53 table based on the calendar model rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_WEEK53_YEAR_DELETE]
	@CM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM CALENDAR_WEEK53_YEAR 
	WHERE CM_RID = @CM_RID		

	SELECT @@ROWCOUNT;

END
GO
