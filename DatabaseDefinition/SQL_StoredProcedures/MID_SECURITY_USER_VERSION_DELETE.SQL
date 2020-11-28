--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Deletes a user security version assignment
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_VERSION_DELETE]
	@USER_RID int,
	@FV_RID int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SECURITY_USER_VERSION 
	WHERE USER_RID = @USER_RID
		  AND FV_RID = @FV_RID

	SELECT @@ROWCOUNT;

END
GO