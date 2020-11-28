--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Deletes a user security version assignment based on user, version, action, and security type
--dv =============================================
CREATE PROCEDURE [dbo].[MID_SECURITY_USER_VERSION_DELETE_FROM_ACTION_AND_TYPE]
	@USER_RID int,
	@FV_RID int,
	@ACTION_ID int,
	@SEC_TYPE int
AS
BEGIN
	SET NOCOUNT ON;

	DELETE 
	FROM SECURITY_USER_VERSION 
	WHERE USER_RID = @USER_RID
		  AND FV_RID = @FV_RID
		  AND ACTION_ID = @ACTION_ID
		  AND SEC_TYPE = @SEC_TYPE

	SELECT @@ROWCOUNT;

END
GO