--dv =============================================
--dv Create date: 12/12/2013
--dv Description: Get a user's theme values.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_THEME_READ_FROM_USER]
	@USER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM THEME
	WHERE USER_RID = @USER_RID
	ORDER BY THEMENAME

END
GO