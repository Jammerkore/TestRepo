--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Marks the user for deletion
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_UPDATE_FOR_DELETION]
	@USER_RID int,
	@USER_DELETE_DATETIME datetime
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE APPLICATION_USER 
	SET 
		USER_DELETE_IND = '1', 
		USER_ACTIVE_IND = '0',
		USER_DELETE_DATETIME = @USER_DELETE_DATETIME
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;
END
GO