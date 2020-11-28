--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Closes all user sessions
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_SESSION_UPDATE_ALL_FOR_CLOSE]
	@LOGOUT_DATETIME datetime
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eSessionStatus_LoggedIn INT = 802861;
	DECLARE @eSessionStatus_LoggedOut INT = 802862;

	UPDATE USER_SESSION 
	SET
		SESSION_STATUS = @eSessionStatus_LoggedOut,
		LOGOUT_DATETIME = @LOGOUT_DATETIME
	WHERE SESSION_STATUS = @eSessionStatus_LoggedIn

	SELECT @@ROWCOUNT;
END
GO