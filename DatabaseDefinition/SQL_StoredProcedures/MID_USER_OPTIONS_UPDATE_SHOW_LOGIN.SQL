--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Updates the show login on the user option table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_OPTIONS_UPDATE_SHOW_LOGIN]
	@USER_RID int,
	@SHOW_LOGIN char
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_OPTIONS 
	SET
		SHOW_LOGIN = @SHOW_LOGIN
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT
END
GO


