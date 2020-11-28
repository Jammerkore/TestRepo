--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Updates a user full name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_UPDATE_FULL_NAME]
	@USER_RID int,
	@USER_FULLNAME varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE APPLICATION_USER 
	SET
		USER_FULLNAME = @USER_FULLNAME
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;
END
GO