--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads names for active users
--dv =============================================
CREATE PROCEDURE [dbo].[MID_APPLICATION_USER_READ_NAMES]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		USER_RID, 
		USER_NAME 
	FROM APPLICATION_USER 
	WHERE USER_ACTIVE_IND=1

END
GO