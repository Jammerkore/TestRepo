--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads all users
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		USER_NAME, 
		USER_ACTIVE_IND, 
		USER_PASSWORD, 
		USER_RID
	FROM APPLICATION_USER	

END
GO