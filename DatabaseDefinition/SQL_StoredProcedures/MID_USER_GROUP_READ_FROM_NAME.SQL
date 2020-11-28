--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads a user group from the group name
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_READ_FROM_NAME]
	@GROUP_NAME varchar(30)
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		GROUP_RID, 
		GROUP_NAME, 
		COALESCE(GROUP_DESCRIPTION,' ') GROUP_DESCRIPTION, 
		GROUP_ACTIVE_IND
	FROM USER_GROUP
	WHERE GROUP_NAME = @GROUP_NAME

END
GO