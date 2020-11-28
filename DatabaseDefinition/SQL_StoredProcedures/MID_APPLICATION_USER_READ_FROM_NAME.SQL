--dv =============================================
--dv Create date: 12/12/2013
--dv Description: Reads the user from the user name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_READ_FROM_NAME]
	@USER_NAME varchar(256)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		USER_ACTIVE_IND, 
		USER_PASSWORD, 
		USER_RID, 
		COALESCE(USER_DELETE_IND,'0') USER_DELETE_IND 
	FROM APPLICATION_USER
	WHERE USER_NAME = @USER_NAME

END
GO