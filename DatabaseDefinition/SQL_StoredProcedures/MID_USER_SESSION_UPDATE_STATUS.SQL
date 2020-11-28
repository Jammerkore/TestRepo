--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Updates the user session status
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_SESSION_UPDATE_STATUS]
	@SESSION_RID int,
	@SESSION_STATUS int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE USER_SESSION 
	SET
		SESSION_STATUS = @SESSION_STATUS
    WHERE SESSION_RID = @SESSION_RID

	SELECT @@ROWCOUNT;
END
GO