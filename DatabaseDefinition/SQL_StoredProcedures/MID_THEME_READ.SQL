--dv =============================================
--dv Create date: 12/12/2013
--dv Description: Get a theme's values.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_THEME_READ]
	@THEME_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT *
	FROM THEME
	WHERE THEME_RID = @THEME_RID

END
GO