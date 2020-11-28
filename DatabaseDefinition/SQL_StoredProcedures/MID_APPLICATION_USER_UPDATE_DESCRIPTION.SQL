--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Updates a user description
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_UPDATE_DESCRIPTION]
	@USER_RID int,
	@USER_DESCRIPTION varchar(250)
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE APPLICATION_USER 
	SET
		USER_DESCRIPTION = @USER_DESCRIPTION
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;
END
GO