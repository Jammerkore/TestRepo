--dv =============================================
--dv Create date:	10/28/2013
--dv Description:	Deletes a calendar model based on the calendar model rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_CALENDAR_MODEL_DELETE]
	@CM_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM CALENDAR_MODEL 
	WHERE CM_RID = @CM_RID

	SELECT @@ROWCOUNT;

END
GO
