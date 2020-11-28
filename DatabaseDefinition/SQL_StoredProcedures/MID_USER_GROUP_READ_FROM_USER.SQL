--dv =============================================
--dv Create date: 1/16/2014
--dv Description: Reads the groups for the user
--dv =============================================
CREATE PROCEDURE [dbo].[MID_USER_GROUP_READ_FROM_USER]
	@USER_RID int
AS
BEGIN
	SET NOCOUNT ON;

	SELECT 
		UG.GROUP_RID GROUP_RID, 
		GROUP_NAME, 
		COALESCE(GROUP_DESCRIPTION,' ') GROUP_DESCRIPTION, 
		GROUP_ACTIVE_IND
	FROM USER_GROUP UG, USER_GROUP_JOIN UGJ
	WHERE UG.GROUP_RID = UGJ.GROUP_RID 
		  AND UGJ.USER_RID = @USER_RID
	ORDER BY GROUP_ACTIVE_IND DESC, GROUP_NAME

END
GO