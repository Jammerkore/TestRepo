--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads a user group
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_READ]
	@GROUP_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		GROUP_RID, 
		GROUP_NAME, 
		COALESCE(GROUP_DESCRIPTION,' ') GROUP_DESCRIPTION, 
		GROUP_ACTIVE_IND
	FROM USER_GROUP
	WHERE GROUP_RID = @GROUP_RID

END
GO