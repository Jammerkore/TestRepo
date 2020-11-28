--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Deletes a calendar model period based on the calendar model rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_MODEL_PERIODS_DELETE]
	@CM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM CALENDAR_MODEL_PERIODS 
	WHERE CM_RID = @CM_RID

	SELECT @@ROWCOUNT;

END
GO
