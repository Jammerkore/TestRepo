--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Deletes all user sessions for the user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_SESSION_DELETE_FROM_USER]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM USER_SESSION
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;
END
GO