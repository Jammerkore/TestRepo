--dv =============================================
--dv Create date: 12/13/2013
--dv Description: Reads user sessions based on user and status
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_SESSION_READ_ACTIVE_FROM_USERNAME]
	@USER_NAME varchar(30)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @eSessionStatus_LoggedIn int = 802861;

	SELECT * 
	FROM USER_SESSION 
	WHERE USER_RID = (SELECT USER_RID FROM APPLICATION_USER WHERE USER_NAME = @USER_NAME)
		  AND SESSION_STATUS = @eSessionStatus_LoggedIn

END
GO