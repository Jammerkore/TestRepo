--dv =============================================
--dv Create date: 1/15/2014
--dv Description: Reads deleted user groups
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_READ_DELETED_GROUPS]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		GROUP_RID, 
		GROUP_NAME,
		COALESCE(GROUP_DESCRIPTION,' ') GROUP_DESCRIPTION, 
		GROUP_ACTIVE_IND, 
		GROUP_DELETE_DATETIME
	FROM USER_GROUP 
	WHERE GROUP_DELETE_IND = '1'

END
GO