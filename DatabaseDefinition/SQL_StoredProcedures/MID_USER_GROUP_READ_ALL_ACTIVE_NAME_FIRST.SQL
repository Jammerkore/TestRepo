--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads all active user groups, returning group name as the first column
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_READ_ALL_ACTIVE_NAME_FIRST]
AS
BEGIN
	SET NOCOUNT ON;

	SELECT
		GROUP_NAME,
		GROUP_RID, 
		COALESCE(GROUP_DESCRIPTION,' ') GROUP_DESCRIPTION, 
		GROUP_ACTIVE_IND
    FROM USER_GROUP 
	WHERE GROUP_ACTIVE_IND = '1' 
	ORDER BY GROUP_NAME

END
GO