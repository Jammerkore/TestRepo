--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Updates a password from a user name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_UPDATE_PWD_FROM_NAME]
	@USER_NAME varchar(30),
	@USER_PASSWORD varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE APPLICATION_USER 
	SET 
		USER_PASSWORD = @USER_PASSWORD 
	WHERE USER_NAME = @USER_NAME

	SELECT @@ROWCOUNT;
END
GO