--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Closes the user session
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_SESSION_UPDATE_FOR_CLOSE]
	@SESSION_RID int,
	@SESSION_STATUS int,
	@LOGOUT_DATETIME datetime
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_SESSION 
	SET 
    SESSION_STATUS = @SESSION_STATUS,
    LOGOUT_DATETIME = @LOGOUT_DATETIME
    WHERE SESSION_RID = @SESSION_RID

	SELECT @@ROWCOUNT;
END
GO