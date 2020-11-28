--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Unmarks the user for deletion
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_UPDATE_FOR_RECOVERY]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	UPDATE APPLICATION_USER 
	SET 
		USER_DELETE_IND = '0', 
		USER_ACTIVE_IND = '1'
	WHERE USER_RID = @USER_RID

	SELECT @@ROWCOUNT;
END
GO