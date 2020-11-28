--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Updates the active indicator for a user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_UPDATE_ACTIVE_IND]
	@USER_RID int,
	@USER_ACTIVE_IND char(1)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE APPLICATION_USER 
	SET
		USER_ACTIVE_IND = @USER_ACTIVE_IND
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;
END
GO