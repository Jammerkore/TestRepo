--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads data for the given user rid
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_READ_FROM_RID]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		USER_ACTIVE_IND, 
		USER_PASSWORD, 
		USER_RID
	FROM APPLICATION_USER 
	WHERE USER_RID = @USER_RID

END
GO