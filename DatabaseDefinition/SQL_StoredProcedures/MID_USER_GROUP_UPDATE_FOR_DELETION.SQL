--dv =============================================
--dv Create date: 1/15/2014
--dv Description: Inactivates and marks the user group for deletion
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_UPDATE_FOR_DELETION]
	@GROUP_RID int,
	@GROUP_DELETE_DATETIME datetime
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_GROUP
	SET
		GROUP_DELETE_IND = '1',
		GROUP_ACTIVE_IND = '0',
		GROUP_DELETE_DATETIME = @GROUP_DELETE_DATETIME
	WHERE GROUP_RID = @GROUP_RID

	SELECT @@ROWCOUNT;
END
GO
