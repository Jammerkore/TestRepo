--dv =============================================
--dv Create date: 12/12/2013
--dv Description: Get theme's RID.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_THEME_READ_THEME_RID]
	@THEMENAME VARCHAR(50),
	@USER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT THEME_RID 
	  FROM THEME AS t 
	  WHERE (t.THEMENAME = @THEMENAME) AND (t.USER_RID = @USER_RID)

END
GO