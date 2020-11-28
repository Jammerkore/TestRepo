--dv =============================================
--dv Create date: 2/3/2014
--dv Description: Updates the theme on the user option table
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_OPTIONS_UPDATE_THEME]
	@USER_RID int,
	@THEME_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_OPTIONS 
	SET
		THEME_RID = @THEME_RID
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT
END
GO
