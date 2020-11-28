--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Updates a user name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_UPDATE_NAME]
	@USER_RID int,
	@USER_NAME varchar(256)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE APPLICATION_USER 
	SET
		USER_NAME = @USER_NAME
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;
END
GO