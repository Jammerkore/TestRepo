--dv =============================================
--dv Create date: 12/05/2013
--dv Description: Get the application user name.
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_READ_NAME] 
	@USER_RID INT
AS
BEGIN
	SET NOCOUNT ON;

	SELECT USER_NAME
	FROM APPLICATION_USER
	WHERE USER_RID = @USER_RID
END
GO


