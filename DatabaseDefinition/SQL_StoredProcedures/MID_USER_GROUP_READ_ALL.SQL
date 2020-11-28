--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads all user groups
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_READ_ALL]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		GROUP_RID, 
		GROUP_NAME, 
		COALESCE(GROUP_DESCRIPTION,' ') GROUP_DESCRIPTION, 
		GROUP_ACTIVE_IND,
		COALESCE(GROUP_DELETE_IND,'0') GROUP_DELETE_IND, 
		GROUP_DELETE_DATETIME
	FROM USER_GROUP 
	ORDER BY GROUP_ACTIVE_IND DESC, GROUP_NAME

END
GO