--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Updates a user password
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_UPDATE_PWD]
	@USER_RID int,
	@USER_PASSWORD varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE APPLICATION_USER 
	SET
		USER_PASSWORD = @USER_PASSWORD
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;
END
GO