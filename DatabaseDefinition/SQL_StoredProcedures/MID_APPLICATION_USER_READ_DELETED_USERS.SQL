--dv =============================================
--dv Create date: 1/15/2014
--dv Description: Reads deleted users
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_READ_DELETED_USERS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		USER_RID, 
		USER_NAME, 
		COALESCE(USER_FULLNAME,' ') USER_FULLNAME,
		COALESCE(USER_DESCRIPTION,' ') USER_DESCRIPTION, 
		USER_ACTIVE_IND, 
		USER_DELETE_DATETIME
	FROM APPLICATION_USER 
	WHERE USER_DELETE_IND = '1'

END
GO