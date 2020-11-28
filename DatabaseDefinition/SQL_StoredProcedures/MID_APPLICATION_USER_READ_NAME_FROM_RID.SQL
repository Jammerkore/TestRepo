--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads the user name for the provided user rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_READ_NAME_FROM_RID]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		USER_NAME 
	FROM APPLICATION_USER 
	WHERE USER_RID = @USER_RID

END
GO