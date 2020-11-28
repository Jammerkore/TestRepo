--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads a user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_READ]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		USER_RID, 
		USER_NAME, 
		COALESCE(USER_FULLNAME,' ') USER_FULLNAME, 
		COALESCE(USER_DESCRIPTION,' ') USER_DESCRIPTION, 
		USER_ACTIVE_IND
	FROM APPLICATION_USER
	WHERE USER_RID = @USER_RID

END
GO